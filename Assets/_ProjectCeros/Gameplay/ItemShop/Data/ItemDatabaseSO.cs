/// <summary>
/// Creates the item Database SO. In this, all items that the player can ever own are listed.
/// Then, based on the item id this script can return the ItemSO that is needed.
/// </summary>

/// <remarks>
/// 18/06/2025 by Unik Kelmendi: Initial creation.
/// </remarks>

using UnityEngine;
using System.Collections.Generic;

namespace ProjectCeros
{
    [CreateAssetMenu(menuName = "Item/Item Database")]
    public class ItemDatabaseSO : ScriptableObject
    {

        [Tooltip("Here go all items that the Player can potentially have.")]
        public List<ItemSO> AllItems;

        private Dictionary<int, ItemSO> _lookup;

        // Setup the dictionary with the item id and ItemSO.
        public void Initialize()
        {
            _lookup = new Dictionary<int, ItemSO>();
            foreach (var item in AllItems)
            {
                if (!_lookup.ContainsKey(item.Id))
                    _lookup[item.Id] = item;
                else
                    Debug.LogWarning($"Duplicate ID {item.Id} in ItemDatabase.");
            }
        }


        // Call this method to get the ItemSO in exchange for the item id.
        public ItemSO GetItemByID(int id)
        {
            if (_lookup == null)
                Initialize();

            if (_lookup.TryGetValue(id, out var item))
                return item;

            Debug.LogWarning($"Item ID {id} not found in ItemDatabase.");
            return null;
        }
    }
}