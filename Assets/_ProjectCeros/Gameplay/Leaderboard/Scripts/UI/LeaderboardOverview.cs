/// <summary>
/// Displays the top podcasters in the leaderboard UI, including the player.
/// </summary>

/// <remarks>
/// 02/07/2025 by Damian Dalinger: Script Creation.
/// </remarks>

using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;

namespace ProjectCeros
{

    public class LeaderboardOverview : MonoBehaviour
    {
        #region Fields

        [HideInInspector] public Podcaster[] Sorted;

        [Header("Podcasters")]
        [Tooltip("List of rival podcasters to display.")]
        [SerializeField] private Podcaster[] _rivalPodcasters;

        [Tooltip("The player podcaster, included in the sorting.")]
        [SerializeField] private Podcaster _playerPodcaster;

        [Header("UI Slots (top to bottom)")]
        [Tooltip("UI elements for each leaderboard slot.")]
        [SerializeField] private LeaderboardSlot[] _slots;

        [Header("Rise/Fall Icons")]
        [Tooltip("Icon displayed when a podcaster's rank increases.")]
        [SerializeField] private Sprite _up;

        [Tooltip("Icon displayed when a podcaster's rank remains the same.")]
        [SerializeField] private Sprite _same;

        [Tooltip("Icon displayed when a podcaster's rank decreases.")]
        [SerializeField] private Sprite _down;

        [Header("Player Outside of Top 10")]
        [Tooltip("GameObject representing the player outside the top 10.")]
        [SerializeField] private GameObject _parent;

        [Tooltip("Text displaying the player's rank.")]
        [SerializeField] private TMP_Text _rank;

        [Tooltip("Text displaying the player's name.")]
        [SerializeField] private TMP_Text _name;

        [Tooltip("Icon representing the player's rank change.")]
        [SerializeField] private Image _riseFall;

        private Podcaster[] _allPodcasters;

        #endregion

        #region Lifecycle Methods

        private void OnEnable()
        {
            UpdateUI();
        }

        #endregion

        #region Private Methods

        // Returns the appropriate rank change icon based on rank difference.
        private Sprite GetRankChangeIcon(int rankChange)
        {
            return rankChange switch
            {
                1 => _up,
                2 => _same,
                3 => _down,
                _ => _same
            };
        }

        // Updates the leaderboard UI with current ranks and portraits.
        private void UpdateUI()
        {
            _allPodcasters = _rivalPodcasters.Append(_playerPodcaster).ToArray();
            Sorted = _allPodcasters.OrderBy(p => p.CurrentRank).ToArray();

            for (int i = 0; i < _slots.Length; i++)
            {
                var slot = _slots[i];
                if (slot.Slot == null || slot.Name == null || slot.Portrait == null)
                    continue;

                var rival = i < Sorted.Length ? Sorted[i] : null;
                UpdateSlot(slot, rival);
            }

            UpdatePlayerOutsideTop10();
        }

        // Displays player info if they are ranked outside the top 10.
        private void UpdatePlayerOutsideTop10()
        {
            if (_playerPodcaster == null || _playerPodcaster.CurrentRank <= 10)
            {
                _parent?.SetActive(false);
                return;
            }

            _parent.SetActive(true);
            _rank.text = $"#{_playerPodcaster.CurrentRank}";
            _name.text = _playerPodcaster.DisplayName;
            _riseFall.sprite = GetRankChangeIcon(_playerPodcaster.RankChange);
        }

        // Updates the content of an individual slot with podcaster data.
        private void UpdateSlot(LeaderboardSlot slot, Podcaster rival)
        {
            if (rival != null)
            {
                slot.Name.text = rival.DisplayName;
                slot.Portrait.sprite = rival.Portrait;
            }
        }

        #endregion
    }
}