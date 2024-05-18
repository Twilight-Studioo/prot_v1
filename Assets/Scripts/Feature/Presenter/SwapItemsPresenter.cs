#region

using System;
using System.Collections.Generic;
using System.Linq;
using Core.Utilities;
using Feature.Common.Parameter;
using Feature.Interface.View;
using Feature.Model;
using UniRx;
using UnityEngine;
using VContainer;
using Object = UnityEngine.Object;

#endregion

namespace Feature.Presenter
{
    public class SwapItemsPresenter : IDisposable
    {
        private readonly CharacterParams characterParams;

        private readonly CompositeDisposable rememberItemPosition;
        private readonly SwapItemsModel swapItemsModel;
        private Dictionary<Guid, SwapItemViewBase> swapItemViews;

        [Inject]
        public SwapItemsPresenter(
            SwapItemsModel swapItemsModel,
            CharacterParams characterParams
        )
        {
            this.swapItemsModel = swapItemsModel;
            this.characterParams = characterParams;
            var list = Object.FindObjectsOfType<SwapItemViewBase>().ToList();
            rememberItemPosition = new();
            AddItems(list);
        }

        public void Dispose()
        {
            rememberItemPosition.Clear();
        }

        public void AddItems(List<SwapItemViewBase> items)
        {
            if (swapItemViews.IsNull())
            {
                swapItemViews = new();
            }

            var dats = items.Select(item =>
            {
                var id = Guid.NewGuid();
                item.SetHighlight(false);
                item.Position
                    .Subscribe(_ => { swapItemsModel.UpdateItemPosition(id, item.Position.Value); })
                    .AddTo(rememberItemPosition);
                return (id, item);
            }).ToList();

            if (Enumerable.Any(dats, valueTuple => !swapItemViews.TryAdd(valueTuple.id, valueTuple.item)))
            {
                throw new AlreadyAddedException(swapItemViews.ToString());
            }

            swapItemsModel.AddItems(
                dats.Select(data => new SwapItem
                    {
                        Id = data.id,
                        Position = data.item.Position.Value,
                    }
                ).ToList());
        }

        public void RemoveItems(List<SwapItemViewBase> items)
        {
            if (items == null)
            {
                return;
            }

            foreach (var swapItemViewBase in items)
            {
                var found = swapItemViews.First(x => x.Value == swapItemViewBase);
                swapItemsModel.RemoveItem(found.Key);
                swapItemViews.Remove(found.Key);
            }
        }

        public void Clear()
        {
            rememberItemPosition.Clear();
        }

        public void ResetSelector()
        {
            swapItemsModel.ResetSelector();
        }

        public void MoveSelector(Vector2 direction, Vector3 basePosition)
        {
            var item = swapItemsModel.GetCurrentItem();
            var select = swapItemsModel.GetNearestItem(basePosition, direction, characterParams.canSwapDistance);
            if (!select.HasValue)
            {
                return;
            }

            if (item.HasValue)
            {
                swapItemViews[item.Value.Id].SetHighlight(false);
            }

            swapItemViews[select.Value.Id].SetHighlight(true);
            swapItemsModel.SetItem(select.Value.Id);
        }

        public SwapItemViewBase SelectItem()
        {
            var item = swapItemsModel.GetCurrentItem();
            if (!item.HasValue || item.Value.Id == Guid.Empty)
            {
                return null;
            }

            return swapItemViews[item.Value.Id];
        }
    }
}