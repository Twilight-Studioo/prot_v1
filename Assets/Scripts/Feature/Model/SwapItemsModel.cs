using System.Collections.Generic;
using UnityEngine;

namespace Feature.Model
{
    public struct SwapItem
    {
        public int Id;
        public Vector3 Position;
    }
    public class SwapItemsModel
    {
        private List<SwapItem> swapItems;
        
        public void SetItems(List<SwapItem> items)
        {
            swapItems = items;
        }
    }
}