/// <summary>
/// This script handles the important UI information for the guest screen.
/// </summary>

/// <remarks>
/// 25/06/2025 by Unik Kelmendi: Initial creation.
/// </remarks>

using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace ProjectCeros
{

    public class GuestUI : MonoBehaviour
    {
        public static GuestUI Instance;

        [SerializeField] private InvitationLimiter _invitationLimiter;

        [SerializeField] private GuestDatabaseSO _unlockedGuestIDs;

        // [SerializeField] private IntReference _money;


        [Header("UI Elements")]
        [SerializeField] private TextMeshProUGUI _guestNameText;

        [SerializeField] private TextMeshProUGUI _descriptionText;

        [SerializeField] private TextMeshProUGUI _topicText;

        // [SerializeField] private TextMeshProUGUI _playerMoney;

        [SerializeField] private Image _guestImage;

        [SerializeField] private Button _inviteButton;

       // [SerializeField] private Image _alreadyBought;

        private GuestSO currentSelectedItem;


        // Adjusts the GuestUI info.
        public void ShowGuestDetails(GuestSO item)
        {
            currentSelectedItem = item;

            _guestNameText.text = item.Name;
            _topicText.text = $"Favourite topic: {item.Topic}";
            _descriptionText.text = item.Description;
            // _priceText.text = $"${item.Price}";
            _guestImage.sprite = item.GuestSprite;

            _invitationLimiter.SetGuestToInvite(item);

            ShowItemSold();
        }


        // Updates the UI when an item is Sold out.
        public void ShowItemSold()

        {
            if (_unlockedGuestIDs.AllGuests.Contains(currentSelectedItem))

            {
                // _alreadyBought.enabled = true;
            }

            else

            {
                // _alreadyBought.enabled = false;
            }

        }

        /* Update the amount of money the Player has in the UI.
        public void UpdateMoney()
        {
            _playerMoney.text = $"${_money.Value}";

            Debug.Log($"Money value changed to: {_money.Value}");
        }

        */

        private void Awake()
        {
            Instance = this;
            // _playerMoney.text = $"${_money.Value}";
            _inviteButton.onClick.AddListener(BuyCurrentItem);
        }


        // Triggers the buying logic.
        private void BuyCurrentItem()
        {
            if (currentSelectedItem == null) return;

            Debug.Log($"Trying to invite {currentSelectedItem.Name}");

            //_moneyChecker.CheckMoney();
           _invitationLimiter.InviteGuest();

        }

    }
}