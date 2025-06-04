
/// <summary>
/// 
/// </summary>

/// <remarks>
/// 02/06/2025 by Unik Kelmendi: Initial creation.
/// </remarks>


using UnityEngine;

namespace ProjectCeros
{
    public class ItemAdder : MonoBehaviour
    {
    public ItemSO itemToAdd; // Assign in Inspector
    public PlayerInventory inventory; // Assign in Inspector

    public void AddItemToInventory()
    {
        inventory.AddItem(itemToAdd);
    }
}
}
