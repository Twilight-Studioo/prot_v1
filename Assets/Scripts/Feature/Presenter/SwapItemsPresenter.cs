using System.Collections.Generic;
using System.Linq;
using Feature.Model;
using Feature.Views;
using UnityEngine;
using VContainer;

namespace Feature.Presenter
{
    public class SwapItemsPresenter
    {
        private List<SwapItemView> swapItemViews;
        private readonly SwapItemsModel swapItemsModel;
        
        [Inject]
        public SwapItemsPresenter(
            SwapItemsModel swapItemsModel
        )
        {
            this.swapItemsModel = swapItemsModel;
            var list = Object.FindObjectsOfType<SwapItemView>().ToList();
            SetItems(list);
        }
        
        private void SetItems(List<SwapItemView> items)
        {
            swapItemViews = items;
            swapItemsModel.SetItems(
                swapItemViews.Select(
                    (view, index) =>
                    {
                        view.SetHighlight(false);
                        return new SwapItem
                        {
                            Id = index,
                            Position = view.transform.position,
                        };
                    }
                ).ToList()
            );
        }
    }
}