/// <summary>
/// This Scripts only purpose is so that the Shop UI does not start empty when the player first opens it.
/// Attach it to the item with the lowest item id.
/// </summary>

/// <remarks>
/// 18/06/2025 by Unik Kelmendi: Initial creation.
/// </remarks>

using UnityEngine;

namespace ProjectCeros

{
    public class InitializeShopUIText : MonoBehaviour
    {  
         [SerializeField] private ItemSO _itemData;

        public void Start()
        {
            ShopUI.Instance.ShowItemDetails(_itemData);

        }
    }
}