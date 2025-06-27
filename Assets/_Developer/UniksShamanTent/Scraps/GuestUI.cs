/// <summary>
/// This script handles the important UI information for the guest screen.
/// </summary>

/// <remarks>
/// 25/06/2025 by Unik Kelmendi: Initial creation.
/// </remarks>

using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEditor.Search;
using UnityEditor;

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

        [SerializeField] private TextMeshProUGUI _chanceText;

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

            ShowItemChance();

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

        public void ShowItemChance()
        {
            float chance = currentSelectedItem.Chance;

            if (chance <= 0.3)
            {
                _chanceText.text = "Unlikely";
            }


            if (chance > 0.3 && chance < 0.7)
            {
                _chanceText.text = "Might come";
            }


            if (chance >= 0.7)
            {
                _chanceText.text = "High chance of comming";
            }


        }


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