#region

using System.Collections.Generic;
using System.Linq;
using Feature.Common.Parameter;
using Feature.Model;
using Feature.Views;
using UniRx;
using UnityEngine;
using VContainer;

#endregion

namespace Feature.Presenter
{
    public class SwapItemsPresenter
    {
        private readonly CharacterParams characterParams;
        private readonly SwapItemsModel swapItemsModel;
        private List<SwapItemView> swapItemViews;

        [Inject]
        public SwapItemsPresenter(
            SwapItemsModel swapItemsModel,
            CharacterParams characterParams
        )
        {
            this.swapItemsModel = swapItemsModel;
            this.characterParams = characterParams;
            var list = Object.FindObjectsOfType<SwapItemView>().ToList();
            SetItems(list);
        }

        private void SetItems(List<SwapItemView> items)
        {
            swapItemViews = items.Select((item, index) =>
            {
                item.SetHighlight(false);
                item.Position
                    .ObserveEveryValueChanged(x => x.Value)
                    .Subscribe(_ =>
                    {
                        swapItemsModel.UpdateItemPosition(index, item.transform.position);
                    });
                return item;
            }).ToList();
            swapItemsModel.SetItems(
                swapItemViews.Select(view => view.transform.position).ToList()
            );
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

        public SwapItemView SelectItem()
        {
            var item = swapItemsModel.GetCurrentItem();
            if (!item.HasValue || item.Value.Id < 0 || item.Value.Id >= swapItemViews.Count)
            {
                return null;
            }

            return swapItemViews[item.Value.Id];
        }
    }
}