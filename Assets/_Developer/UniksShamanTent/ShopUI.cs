/// <summary>
/// This script handles the important UI information for the Shop screen.
/// </summary>

/// <remarks>
/// 18/06/2025 by Unik Kelmendi: Initial creation.
/// </remarks>

using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace ProjectCeros
{

    public class ShopUI : MonoBehaviour
    {
        public static ShopUI Instance;

        [SerializeField] private MoneyChecker _moneyChecker;

        [SerializeField] private IntRuntimeSet _unlockedItemIDs;
 

        [Header("UI Elements")]
        [SerializeField] private TextMeshProUGUI _itemNameText;

        [SerializeField] private TextMeshProUGUI _descriptionText;

        [SerializeField] private TextMeshProUGUI _priceText;

        [SerializeField] private Image _itemImage;

        [SerializeField] private Button _buyButton;

        [SerializeField] private Image _alreadyBought;

        private ItemSO currentSelectedItem;

        private void Awake()
        {
            Instance = this;
            _buyButton.onClick.AddListener(BuyCurrentItem);
        }


        // Adjusts the ShopUI info.
        public void ShowItemDetails(ItemSO item)
        {
            currentSelectedItem = item;

            _itemNameText.text = item.ItemName;
            _descriptionText.text = item.Description;
            _priceText.text = $"${item.Price}";
            _itemImage.sprite = item.ItemSprite;

            _moneyChecker.SetItemToPurchase(item);

            ShowItemSold();
        }

        // Triggers the buying logic.
        private void BuyCurrentItem()
        {
            if (currentSelectedItem == null) return;

            Debug.Log($"Buying {currentSelectedItem.ItemName}");

            _moneyChecker.CheckMoney();

        }

        // Updates the UI when an item is Sold out.
        public void ShowItemSold()

        {
            if (_unlockedItemIDs.Items.Contains(currentSelectedItem.Id))

            {
                _alreadyBought.enabled = true;
            }

            else

            {
                _alreadyBought.enabled = false;
            }


        }

    }
}