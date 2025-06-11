using UnityEngine;
using System.Collections.Generic;

namespace ProjectCeros
{


    [CreateAssetMenu(menuName = "Item/Item Database")]
    public class ItemDatabaseSO : ScriptableObject
    {
        public List<ItemSO> allItems;

        private Dictionary<int, ItemSO> lookup;

        public void Initialize()
        {
            lookup = new Dictionary<int, ItemSO>();
            foreach (var item in allItems)
            {
                if (!lookup.ContainsKey(item.id))
                    lookup[item.id] = item;
                else
                    Debug.LogWarning($"Duplicate ID {item.id} in ItemDatabase.");
            }
        }

        public ItemSO GetItemByID(int id)
        {
            if (lookup == null)
                Initialize();

            if (lookup.TryGetValue(id, out var item))
                return item;

            Debug.LogWarning($"Item ID {id} not found in ItemDatabase.");
            return null;
        }
    }
}