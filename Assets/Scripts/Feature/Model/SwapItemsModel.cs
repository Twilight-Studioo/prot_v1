#region

using System.Collections.Generic;
using System.Linq;
using UnityEngine;

#endregion

namespace Feature.Model
{
    public struct SwapItem
    {
        public int Id;
        public Vector3 Position;
    }

    public class SwapItemsModel
    {
        private int currentId;
        private List<SwapItem> swapItems;

        public SwapItemsModel()
        {
            swapItems = new();
            currentId = -1;
        }

        public void SetItems(List<Vector3> items)
        {
            swapItems = items
                .Select((item, index) =>
                    new SwapItem
                    {
                        Id = index,
                        Position = item,
                    }
                ).ToList();
        }

        public void ResetSelector()
        {
            currentId = -1;
        }
        
        public void UpdateItemPosition(int id, Vector3 position)
        {
            if (id < 0 || id >= swapItems.Count)
            {
                return;
            }

            swapItems[id] = new SwapItem
            {
                Id = id,
                Position = position,
            };
        }

        public void SetItem(int id)
        {
            if (id < 0 || id >= swapItems.Count)
            {
                return;
            }

            currentId = id;
        }

#nullable enable
        public SwapItem? GetCurrentItem()
        {
            if (currentId < 0 || currentId >= swapItems.Count)
            {
                return null;
            }

            return swapItems[currentId];
        }

        /// <summary>
        ///     Returns the closest object from a specified location in a preferred direction, but within a maximum distance
        /// </summary>
        /// <param name="position">The specified location from which we need to find the nearest object</param>
        /// <param name="direction">The preferred direction in which we need to find the object</param>
        /// <param name="maxDistance">The maximum distance from 'position' at which an item may be considered (squared distance) </param>
        /// <returns>The nearest object in the preferred direction within the maximum distance, null if no such item exists</returns>
        public SwapItem? GetNearestItem(Vector3 position, Vector3 direction, float maxDistance)
#nullable disable
        {
            var nearestItem = GetCurrentItem();
            var closestDistance = Mathf.Infinity;

            foreach (var item in swapItems)
            {
                var toItem = item.Position - position;
                if (!(Vector3.Dot(toItem, direction) > 0)) // Check if item is in the specified direction
                {
                    continue;
                }

                var distance = toItem.sqrMagnitude;
                if (!(distance < closestDistance) || !(distance <= maxDistance))
                {
                    continue;
                }

                nearestItem = item;
                closestDistance = distance;
            }

            // If the closest item is beyond the max distance, return null
            if (closestDistance > maxDistance)
            {
                return null;
            }

            return nearestItem;
        }
    }
}