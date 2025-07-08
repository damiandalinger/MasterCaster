/// <summary>
/// Displays the player's current rank and change icon in the leaderboard UI.
/// </summary>

/// <remarks>
/// 07/07/2025 by Damian Dalinger: Script Creation.
/// </remarks>

using UnityEngine;
using TMPro;
using UnityEngine.UI;

namespace ProjectCeros
{
    public class PlayerRankDisplay : MonoBehaviour
    {
        #region Fields

        [Tooltip("Reference to the player podcaster ScriptableObject.")]
        [SerializeField] private Podcaster _player;

        [Header("UI Elements")]
        [Tooltip("Text element to display the player's rank.")]
        [SerializeField] private TMP_Text _rank;

        [Tooltip("Icon indicating whether rank increased, stayed, or dropped.")]
        [SerializeField] private Image _rankChange;

        [Header("Rank Change Sprites")]
        [Tooltip("Icon for rank increase.")]
        [SerializeField] private Sprite _Up;

        [Tooltip("Icon for unchanged rank.")]
        [SerializeField] private Sprite _Same;

        [Tooltip("Icon for rank decrease.")]
        [SerializeField] private Sprite _Down;

        #endregion

        #region Lifecycle Methods

        private void OnEnable()
        {
            UpdateDisplay();
        }

        #endregion

        #region Public Methods

        // Updates the UI elements based on the player's current rank and change state.
        public void UpdateDisplay()
        {
          
            if (_rank != null)
                _rank.text = $"#{_player.CurrentRank}";

            if (_rankChange != null)
            {
                _rankChange.sprite = _player.RankChange switch
                {
                    1 => _Up,
                    2 => _Same,
                    3 => _Down,
                    _ => _Same
                };
            }
        }

        #endregion
    }
}