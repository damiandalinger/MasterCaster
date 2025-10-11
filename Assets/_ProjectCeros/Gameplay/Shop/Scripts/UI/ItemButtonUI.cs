/// <summary>
/// This script shows the correct item once the player clicks on the button that is connected to the
/// item info in the shop tab.
/// </summary>

/// <remarks>
/// 18/06/2025 by Unik Kelmendi: Initial creation.
/// </remarks>

using UnityEngine;

namespace ProjectCeros
{

    public class ItemButtonUI : MonoBehaviour
    {
        [SerializeField] private ItemSO _itemData;
         
        // This tells the ShopUI the SO data.
        public void TransferData()
        {
            ShopUI.Instance.ShowItemDetails(_itemData);
        }
    }
}