/// <summary>
/// Displays detailed information about a selected podcaster on the leaderboard.
/// </summary>

/// <remarks>
/// 02/07/2025 by Damian Dalinger: Script Creation.
/// </remarks>

using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

namespace ProjectCeros
{
    public class LeaderboardDetailUI : MonoBehaviour
    {
        #region Fields

        [Tooltip("Reference to the LeaderboardOverview script.")]
        [SerializeField] LeaderboardOverview _overview;

        [Header("Background Sprites")]
        [Tooltip("Sprite used for 1st place background.")]
        [SerializeField] private Sprite _gold;

        [Tooltip("Sprite used for 2nd place background.")]
        [SerializeField] private Sprite _silver;

        [Tooltip("Sprite used for 3rd place background.")]
        [SerializeField] private Sprite _bronze;

        [Tooltip("Sprite used for ranks 4 and lower.")]
        [SerializeField] private Sprite _standard;

        [Header("Rise/Fall Icons")]
        [Tooltip("Icon shown when rank improves.")]
        [SerializeField] private Sprite _up;

        [Tooltip("Icon shown when rank stays the same.")]
        [SerializeField] private Sprite _same;

        [Tooltip("Icon shown when rank decreases.")]
        [SerializeField] private Sprite _down;

        [Header("Detail View")]
        [Tooltip("Root panel GameObject of the detail UI.")]
        [SerializeField] private GameObject _panel;

        [Tooltip("Icon showing rank change direction.")]
        [SerializeField] private Image _riseFallIcon;

        [Tooltip("Background image of the detail panel.")]
        [SerializeField] private Image _backgroundImage;

        [Tooltip("Text displaying podcaster's display name.")]
        [SerializeField] private TMP_Text _name;

        [Tooltip("Text displaying podcaster's internal name.")]
        [SerializeField] private TMP_Text _personName;

        [Tooltip("Text displaying current listener count.")]
        [SerializeField] private TMP_Text _listenerCount;

        [Tooltip("Text displaying current rank.")]
        [SerializeField] private TMP_Text _rank;

        [Tooltip("Image showing the podcaster portrait.")]
        [SerializeField] private Image _portrait;

        [Tooltip("Text showing the podcaster description.")]
        [SerializeField] private TMP_Text _description;

        [Tooltip("Text fields to display liked genre names.")]
        public List<TMP_Text> _likedGenres;

        [Tooltip("Text fields to display disliked genre names.")]
        public List<TMP_Text> _dislikedGenres;

        #endregion

        #region Public Methods

        // Populates the detail view with data from the given podcaster index.
        public void ShowDetail(int index)
        {
            var rival = _overview.Sorted[index];

            _panel.SetActive(true);
            _name.text = rival.DisplayName;
            _personName.text = rival.PersonName;
            _listenerCount.text = $"{rival.CurrentListener:N0} listeners";
            _rank.text = $"#{rival.CurrentRank}";
            _portrait.sprite = rival.Portrait;
            _description.text = rival.Description;

            _riseFallIcon.sprite = GetRankChangeIcon(rival.RankChange);
            _backgroundImage.sprite = GetBackgroundSpriteForRank(rival.CurrentRank);

            for (int i = 0; i < _likedGenres.Count; i++)
            {
                _likedGenres[i].text = GetGenreText(rival.LikedGenres, i);
            }

            for (int i = 0; i < _dislikedGenres.Count; i++)
            {
                _dislikedGenres[i].text = GetGenreText(rival.DislikedGenres, i);
            }
        }

        #endregion

        #region Private Methods

        // Returns the background sprite based on a given rank.
        private Sprite GetBackgroundSpriteForRank(int rank)
        {
            return rank switch
            {
                1 => _gold,
                2 => _silver,
                3 => _bronze,
                _ => _standard
            };
        }

        // Returns the display name of a genre by index from a genre array.
        private string GetGenreText(int[] genres, int index)
        {
            return genres.Length > index ? GetSubgenreDisplayName(genres[index]) : "-";
        }

        // Returns the corresponding icon for a given rank change.
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

        // Maps a subgenre ID to a display name.
        private string GetSubgenreDisplayName(int subgenreId)
        {
            return SubgenreNames.TryGetValue(subgenreId, out var name) ? name : "Unknown";
        }

        private static readonly Dictionary<int, string> SubgenreNames = new()
        {
            [1] = "FPS",
            [2] = "Hero Shooter",
            [3] = "Loot Shooter",
            [4] = "Fighting Game",
            [5] = "Stealth Game",
            [6] = "Hack & Slash",
            [7] = "Souls Like",
            [8] = "Open World",
            [9] = "MMORPG",
            [10] = "RTS",
            [11] = "Grand Strategy",
            [12] = "TBS",
            [13] = "Sport",
            [14] = "Living Simulation",
            [15] = "Job Simulation",
            [16] = "Farming Game",
            [17] = "Side Scroller",
            [18] = "Roguelike"
        };

        #endregion
    }
}