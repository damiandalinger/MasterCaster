/// <summary>
/// This script shows the correct item once the player clicks on the button that is connected to the
/// item info in the shop tab.
/// </summary>

/// <remarks>
/// 18/06/2025 by Unik Kelmendi: Initial creation.
/// </remarks>

using UnityEngine;
using UnityEngine.UI;


namespace ProjectCeros
{

    public class ItemButtonUI : MonoBehaviour
    {
        [SerializeField] private ItemSO _itemData;
        [SerializeField] private Button _button;

        private void Awake()
        {
            _button.onClick.AddListener(OnClick);
        }

        private void OnClick()
        {
            ShopUI.Instance.ShowItemDetails(_itemData);
        }
    }
}