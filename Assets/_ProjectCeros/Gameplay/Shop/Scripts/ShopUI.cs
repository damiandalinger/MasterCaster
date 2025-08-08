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

        [SerializeField] private IntReference _money;


        [Header("UI Elements")]
        [SerializeField] private TextMeshProUGUI _itemNameText;

        [SerializeField] private TextMeshProUGUI _descriptionText;

        [SerializeField] private TextMeshProUGUI _priceText;

        [SerializeField] private TextMeshProUGUI _playerMoney;

        [SerializeField] private Image _itemImage;

        public GameObject _itemImageObject;

        [SerializeField] private Sprite _greySprite;

        [SerializeField] private Sprite _regularSprite;

        [SerializeField] private Button _buyButton;

        [SerializeField] private GameObject _alreadyBought;

        [SerializeField] private Transform _initialPosition;

        public Image buttonImage;

        private ItemSO currentSelectedItem;

        [SerializeField] private SmoothWobbleZ _wobble;

        [SerializeField] private BoolReference _isMoving;


        // Adjusts the ShopUI info.
        public void ShowItemDetails(ItemSO item)
        {
            currentSelectedItem = item;

            _itemNameText.text = item.ItemName;
            _descriptionText.text = item.Description;
            _priceText.text = $"${item.Price}";
            _itemImage.sprite = item.ItemSprite;

            _itemImageObject.transform.position = _initialPosition.position;

            _isMoving.Variable.SetValue(true);

            _moneyChecker.SetItemToPurchase(item);

            ShowItemSold();

        }


        // Updates the UI when an item is Sold out.
        public void ShowItemSold()

        {
            if (_unlockedItemIDs.Items.Contains(currentSelectedItem.Id))

            {
                _alreadyBought.SetActive(true);

                _buyButton.interactable = false;

                // Image buttonImage = _buyButton.GetComponent<Image>();
                buttonImage.sprite = _greySprite;

                _wobble.enabled = false;
                _buyButton.transform.rotation = Quaternion.Euler(0f, 0f, 0f);
            }

            else if (currentSelectedItem.Price > _money.Value)
            {
                _alreadyBought.SetActive(false);
                _buyButton.interactable = false;

                _wobble.enabled = false;

                // Image buttonImage = _buyButton.GetComponent<Image>();
                buttonImage.sprite = _greySprite;
            }

            else 
            {
                _alreadyBought.SetActive(false);
                _buyButton.interactable = true;

                _wobble.enabled = true;

                // Image buttonImage = _buyButton.GetComponent<Image>();
                buttonImage.sprite = _regularSprite;
            }

        }

        // Update the amount of money the Player has in the UI.
        public void UpdateMoney()
        {
            _playerMoney.text = $"${_money.Value}";

            Debug.Log($"Money value changed to: {_money.Value}");
        }

        private void Awake()
        {
            Instance = this;
            _playerMoney.text = $"${_money.Value}";
            _buyButton.onClick.AddListener(BuyCurrentItem);
        }


        // Triggers the buying logic.
        private void BuyCurrentItem()
        {
            if (currentSelectedItem == null) return;

            Debug.Log($"Buying {currentSelectedItem.ItemName}");

            _moneyChecker.CheckMoney();

        }

    }
}