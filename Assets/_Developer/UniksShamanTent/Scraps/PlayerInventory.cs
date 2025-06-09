
/// <summary>
/// 
/// </summary>

/// <remarks>
/// 02/06/2025 by Unik Kelmendi: Initial creation.
/// </remarks>


using System.Collections.Generic;
using UnityEngine;

namespace ProjectCeros
{
    public class PlayerInventory : MonoBehaviour
    {
        [SerializeField] private List<ItemSO> inventory = new();

        public void AddItem(ItemSO item)
        {
            if (!inventory.Contains(item))
            {
                inventory.Add(item);
                Debug.Log("Added item to inventory: " + item.ItemName);
            }
            else
            {
                Debug.Log("Item already in inventory: " + item.ItemName);
            }
        }

        public List<ItemSO> GetInventory() => inventory;
    }
}
