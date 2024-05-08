#region

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

#endregion

namespace Feature.Model
{
    public struct SwapItem
    {
        public Guid Id;
        public Vector3 Position;

        public SwapItem(Guid id, Vector3 position)
        {
            Id = id;
            Position = position;
        }
    }

    public class SwapItemsModel
    {
        private Guid currentId;
        private readonly List<SwapItem> swapItems;

        public SwapItemsModel()
        {
            swapItems = new();
            currentId = Guid.Empty;
        }

        public void AddItems(List<SwapItem> items)
        {
            swapItems.AddRange(items);
        }

        public void RemoveItems(Predicate<Guid> match)
        {
            swapItems.RemoveAll(item =>
            {
                var found = match(item.Id);
                if (found && item.Id == currentId)
                {
                    currentId = Guid.Empty;
                }
                return found;
            });
        }

        public void RemoveItem(Guid id)
        {
            swapItems.RemoveAll(item => item.Id == id);
        }

        public void ResetSelector()
        {
            currentId = Guid.Empty;
        }

        public void UpdateItemPosition(Guid id, Vector3 position)
        {
            if (id == Guid.Empty)
            {
                return;
            }

            var index = swapItems.FindIndex(x => x.Id == id);
            if (index < 0 || index >= swapItems.Count)
            {
                return;
            }

            var swapItem = swapItems[index];
            swapItem.Position = position;
            swapItems[index] = swapItem;

        }

        public void SetItem(Guid id)
        {
            if (id == Guid.Empty)
            {
                return;
            }

            currentId = id;
        }

#nullable enable
        public SwapItem? GetCurrentItem()
        {
            if (currentId == Guid.Empty)
            {
                return null;
            }

            return swapItems.First(x => x.Id == currentId);
        }

        /// <summary>
        ///     Returns the closest object from a specified location in a preferred direction, but within a maximum distance
        /// </summary>
        /// <param name="position">The specified location from which we need to find the nearest object</param>
        /// <param name="direction">The preferred direction in which we need to find the object</param>
        /// <param name="maxDistance">The maximum distance from 'position' at which an item may be considered (squared distance) </param>
        /// <returns>The nearest object in the preferred direction within the maximum distance, null if no such item exists</returns>
        public SwapItem? GetNearestItem(Vector3 position, Vector3 direction, float maxDistance)
        {
            var nearestItem = GetCurrentItem();
            var nearestDirection = Mathf.Infinity;
            if (direction is { x: 0f, y: 0f, })
            {
                return null;
            }

            direction = new(direction.x, direction.y, direction.z);
            var items = swapItems
                .Where(x => Vector3.Distance(x.Position, position) < maxDistance);
            const float def = Mathf.Deg2Rad * 90f;
            foreach (var item in items)
            {
                var toItem = item.Position - position;
                var dir = Vector2.Dot(direction, toItem.normalized) - def;
                if (Math.Abs(nearestDirection) < Math.Abs(dir))
                {
                    continue;
                }

                nearestItem = item;
                nearestDirection = dir;
            }

            return nearestItem;
        }
    }
}