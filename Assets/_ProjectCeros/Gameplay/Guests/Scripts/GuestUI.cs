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

        [SerializeField] private IntReference InvitationLimit;

        [SerializeField] private IntReference InvitationsSend;

        [Header("UI Elements")]
        [SerializeField] private TextMeshProUGUI _guestNameText;

        [SerializeField] private TextMeshProUGUI _descriptionText;

        [SerializeField] private TextMeshProUGUI _topicText;

        [SerializeField] private TextMeshProUGUI _chanceText;

        [SerializeField] private Image _guestImage;

        [SerializeField] private Button _inviteButton;

        [SerializeField] private TMP_Text _inviteButtonText;

        private GuestSO currentSelectedItem;

        public Transform starParent;
        public Sprite filledStar;
        public Sprite emptyStar;

        public Sprite _greyedSprite;
        public Sprite _regularSprite;
        public Image buttonImage;




        // Adjusts the GuestUI info.
        public void ShowGuestDetails(GuestSO item)
        {
            currentSelectedItem = item;

            _guestNameText.text = item.Name;
            _descriptionText.text = item.Description;
            _guestImage.sprite = item.GuestSprite;

            if (item.wasInterviewed)
            {
                _topicText.text = $"Favourite topic: {item.Topic}";
            }

            else
            {
                _topicText.text = $"Favourite topic: ???";
            }

            ShowStarRating(item.Rating);



            if (item.isRequested)
            {
                _inviteButtonText.text = "Requested";
                _inviteButton.interactable = false;
                buttonImage.sprite = _greyedSprite;
            }

            else if (item.hasAccepted)
            {
                _inviteButtonText.text = "Accepted";
                _inviteButton.interactable = false;
                buttonImage.sprite = _regularSprite;
            }

            else if (item.isOnCooldown)
            {
                _inviteButtonText.text = "Unavailable";

                buttonImage.sprite = _greyedSprite;
                _inviteButton.interactable = false;
            }

            else if (InvitationLimit.Value == InvitationsSend.Value)
            {
                _inviteButtonText.text = "No more invites";
                _inviteButton.interactable = false;
                buttonImage.sprite = _greyedSprite;
            }

            else
            {
                _inviteButtonText.text = "Request!";

                _inviteButton.interactable = true;
                buttonImage.sprite = _regularSprite;
            }

            _invitationLimiter.SetGuestToInvite(item);


            ShowItemChance();

        }


        public void ShowItemChance()
        {
            float chance = currentSelectedItem.Chance;

            if (chance <= 0.25)
            {
                _chanceText.text = "Probably won't come";
            }

            if (chance > 0.25 && chance < 0.5)
            {
                _chanceText.text = "Might come";
            }

            if (chance >= 0.5 && chance < 0.75)
            {
                _chanceText.text = "Will propably come";
            }

            if (chance >= 0.75)
            {
                _chanceText.text = "High chance of coming";
            }


        }


        private void Awake()
        {
            Instance = this;
            // _playerMoney.text = $"${_money.Value}";
            buttonImage = _inviteButton.GetComponent<Image>();
            _inviteButton.onClick.AddListener(BuyCurrentItem);
        }


        // Triggers the buying logic.
        private void BuyCurrentItem()
        {
            if (currentSelectedItem == null) return;

            Debug.Log($"Trying to invite {currentSelectedItem.Name}");

            //_moneyChecker.CheckMoney();
            _invitationLimiter.InviteGuest();

            ShowGuestDetails(currentSelectedItem);

        }


        public void ShowStarRating(int rating)
        {
            for (int i = 0; i < starParent.childCount; i++)
            {
                var image = starParent.GetChild(i).GetComponent<Image>();
                image.sprite = (i < rating) ? filledStar : emptyStar;
            }
        }

    }
}