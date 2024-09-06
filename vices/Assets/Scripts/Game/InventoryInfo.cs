using System;
using System.Collections.Generic;
using UnityEngine;

namespace Vices.Scripts
{
    [CreateAssetMenu(fileName = nameof(InventoryInfo), menuName = "JabberwockyWorld/Game/" + nameof(InventoryInfo))]
    public class InventoryInfo : ScriptableObject
    {
        public Dictionary<string, InventoryItem> _inventoryItems = new Dictionary<string, InventoryItem>();
        public Action<Item> OnItemAdded;

        public void AddItem(Item item, int amount)
        {
            if (_inventoryItems.TryGetValue(item.Name, out var inventoryItem))
            {
                inventoryItem.Amount += amount;
            }

            OnItemAdded?.Invoke(item);
        }
    }
}
