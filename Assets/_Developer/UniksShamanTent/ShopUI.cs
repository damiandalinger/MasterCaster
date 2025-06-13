using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace ProjectCeros
{

    public class ShopUI : MonoBehaviour
    {
        public static ShopUI Instance;

        [Header("UI Elements")]
        [SerializeField] private TextMeshProUGUI itemNameText;
        [SerializeField] private TextMeshProUGUI descriptionText;
        [SerializeField] private TextMeshProUGUI priceText;
        [SerializeField] private Image itemImage;
        [SerializeField] private Button buyButton;

        private ItemSO currentSelectedItem;

        private void Awake()
        {
            Instance = this;
            buyButton.onClick.AddListener(BuyCurrentItem);
        }

        public void ShowItemDetails(ItemSO item)
        {
            currentSelectedItem = item;

            itemNameText.text = item.itemName;
            descriptionText.text = item.description;
            priceText.text = $"${item.price}";
            itemImage.sprite = item.itemSprite;
        }

        private void BuyCurrentItem()
        {
            if (currentSelectedItem == null) return;

            // Trigger your purchase logic here
            Debug.Log($"Buying {currentSelectedItem.itemName}");
            // Fire your parameterized GameEvent or deduct currency, etc.
        }
    }
}