/// <summary>
/// Handles podcast creation, listener gain calculation, and user feedback display based on genre and subgenre selection.
/// </summary>

/// <remarks>
/// 20/05/2025 by Damian Dalinger: Initial creation.
/// </remarks>

using System.Linq;
using TMPro;
using UnityEngine;

namespace ProjectCeros
{
    public class PodcastCreator : MonoBehaviour
    {

        #region Fields

        [Tooltip("Current number of listeners.")]
        [SerializeField] private IntReference _currentListeners;

        [Tooltip("Text UI element for feedback.")]
        [SerializeField] private TMP_Text _feedbackText;

        [Tooltip("Event raised after podcast is confirmed.")]
        [SerializeField] private GameEvent _onPodcastConfirmed;
        
        [Tooltip("Database that holds selected important articles.")]
        [SerializeField] private ArticleDatabase _selectedImportantArticles;

        [Header("Listener Settings")]
        [Tooltip("Multiplier for previous listeners.")]
        [SerializeField] private FloatReference _previousListenerMod;

        [Tooltip("Penalty when selecting an incorrect genre.")]
        [SerializeField] private FloatReference _wrongGenrePenalty;

        [Tooltip("Bonus multiplier if the selected subgenre matches.")]
        [SerializeField] private FloatReference _subgenreBonus;

        private int _selectedGenre = -1;
        private int _selectedValue = 0;
        private string _selectedSubgenre = string.Empty;

        #endregion

        #region LifeCycle Methods

        private void Start()
        {
            UpdateFeedback();
        }

        #endregion

        #region Public Methods

        // Sets the selected genre.
        public void SelectGenre(int genreId)
        {
            _selectedGenre = genreId;
            UpdateFeedback();
        }

        // Sets the selected spin (1 = Positive, 2 = Negative).
        public void SelectSpin(int value)
        {
            _selectedValue = value;
            UpdateFeedback();
        }

        // Sets the selected subgenre.
        public void SelectSubgenre(string subgenre)
        {
            _selectedSubgenre = subgenre;
            UpdateFeedback();
        }

        // Confirms the current selection, performs listener gain calculation and updates feedback.
        public void ConfirmSelection()
        {
            if (!IsValidSelection())
            {
                ShowInvalidFeedback();
                return;
            }

            var result = CalculateListenerGain();
            ApplyListenerResult(result);
            ShowResultFeedback(result);
        }

        #endregion

        #region Private Methods

        // Returns true if the current selection is valid for processing.
        private bool IsValidSelection()
        {
            return _selectedGenre >= 1 && _selectedGenre <= 6 &&
                   (_selectedValue == 1 || _selectedValue == 2);
        }

        // Resets invalid selections and notifies the user via UI.
        private void ShowInvalidFeedback()
        {
            ResetSelection();
            _feedbackText.text = "Invalid selection – please choose a genre, spin, and subgenre.";
        }

        // Resets the internal selection state.
        private void ResetSelection()
        {
            _selectedGenre = -1;
            _selectedValue = 0;
            _selectedSubgenre = string.Empty;
        }

        // Calculates listener growth based on current selections and article match data.
        private PodcastResult CalculateListenerGain()
        {
            var articles = _selectedImportantArticles.Items;
            var matchingArticle = articles.FirstOrDefault(a => a.PairID / 1000 == _selectedGenre);

            var result = new PodcastResult
            {
                GenreMatched = false,
                SubgenreMatched = false,
                TopicMod = _wrongGenrePenalty.Value,
                SubgenreBonus = 0f,
                StrongSpin = false
            };

            var valueUsed = 1f;
            if (matchingArticle != null)
            {
                result.GenreMatched = true;
                valueUsed = _selectedValue == 1 ? matchingArticle.ValuePositive : matchingArticle.ValueNegative;
                result.TopicMod = valueUsed;

                var altValue = _selectedValue == 1 ? matchingArticle.ValueNegative : matchingArticle.ValuePositive;
                result.StrongSpin = valueUsed > altValue;

                if (!string.IsNullOrEmpty(matchingArticle.Subgenre) &&
                    matchingArticle.Subgenre == _selectedSubgenre)
                {
                    result.SubgenreBonus = _subgenreBonus.Value;
                    result.SubgenreMatched = true;
                }
            }

            var baseListeners = _currentListeners.Value;
            var growthMultiplier = _previousListenerMod.Value;
            result.BaseListeners = baseListeners;
            result.RawResult = 2 + (baseListeners * growthMultiplier) * result.TopicMod * (1 + result.SubgenreBonus);
            result.FinalListeners = Mathf.CeilToInt(result.RawResult);
            result.Gain = result.FinalListeners - baseListeners;

            return result;
        }

        // Applies the result of the listener gain calculation to the game state and logs the result.
        private void ApplyListenerResult(PodcastResult result)
        {
            _currentListeners.Variable.SetValue(result.FinalListeners);
            _onPodcastConfirmed?.Raise();

            Debug.Log(
                $"[Podcast Calculation Result]\n" +
                $"Genre Match: {result.GenreMatched}\n" +
                $"Subgenre Match: {result.SubgenreMatched}\n" +
                $"Base Listeners: {result.BaseListeners}\n" +
                $"Growth Multiplier: {_previousListenerMod.Value}\n" +
                $"Topic Modifier: {result.TopicMod}\n" +
                $"Subgenre Bonus: {result.SubgenreBonus}\n" +
                $"Raw Result: {result.RawResult}\n" +
                $"Final Listeners: {result.FinalListeners} (Gain: {result.Gain})"
            );
        }

        // Updates the UI with the result feedback based on selection and calculation.
        private void ShowResultFeedback(PodcastResult result)
        {
            var text = "Podcast completed!\n\n";
            text += $"Genre: {GetGenreName(_selectedGenre)} {(result.GenreMatched ? "Correct" : "Incorrect")}\n";
            text += $"Spin: {(_selectedValue == 1 ? "Positive" : "Negative")} {(result.StrongSpin ? "Strong choice" : "Weak impact")}\n";
            text += $"Subgenre: {(result.SubgenreMatched ? $"{GetSubgenreDisplayName(_selectedSubgenre)} (Matched)" : "No bonus")}\n\n";
            text += $"New listeners: {(result.Gain >= 0 ? "+" : "")}{result.Gain}\n";

            text += result.Gain >= 30
                ? "You broke through! The episode is going viral!"
                : result.Gain >= 15
                    ? "Great performance! The podcast is gaining momentum."
                    : result.Gain >= 5
                        ? "Solid start. You're slowly building an audience."
                        : result.Gain > 0
                            ? "A few curious listeners tuned in. Keep going!"
                            : "No traction this time. Maybe rethink your angle?";

            _feedbackText.text = text;
        }

        // Updates the feedback UI with current selection state.
        private void UpdateFeedback()
        {
            if (_feedbackText == null)
                return;

            var text = "Current Selection:\n";

            text += _selectedGenre >= 1 && _selectedGenre <= 6
                ? $"Genre: {GetGenreName(_selectedGenre)}\n"
                : "Genre: Not selected\n";

            text += _selectedValue == 1
                ? "Spin: Positive\n"
                : _selectedValue == 2
                    ? "Spin: Negative\n"
                    : "Spin: Not selected\n";

            text += !string.IsNullOrEmpty(_selectedSubgenre)
                ? $"Subgenre: {GetSubgenreDisplayName(_selectedSubgenre)}\n"
                : "Subgenre: Not selected\n";

            _feedbackText.text = text;
        }

        // Returns a display name for the selected genre ID.
        private string GetGenreName(int genreId)
        {
            return genreId switch
            {
                1 => "Action",
                2 => "Indie",
                3 => "RPG",
                4 => "Shooter",
                5 => "Simulation",
                6 => "Strategy",
                _ => "Unknown"
            };
        }

        // Returns a user-friendly display name for the subgenre.
        private string GetSubgenreDisplayName(string subgenreId)
        {
            return subgenreId.ToLower() switch
            {
                "fps" => "FPS",
                "heroshooter" => "Hero Shooter",
                "lootshooter" => "Loot Shooter",
                "fightinggame" => "Fighting Game",
                "stealthgame" => "Stealth Game",
                "hackandslash" => "Hack & Slash",
                "soulslike" => "Souls Like",
                "openworld" => "Open World",
                "mmorpg" => "MMORPG",
                "rts" => "RTS",
                "grandstrategy" => "Grand Strategy",
                "tbs" => "TBS",
                "sport" => "Sport",
                "livingsimulation" => "Living Simulation",
                "jobsimulation" => "Job Simulation",
                "farminggame" => "Farming Game",
                "sidescroller" => "Side Scroller",
                "roguelike" => "Roguelike",
                _ => subgenreId
            };
        }

        #endregion

        #region Nested Types

        // Container for all listener gain calculation results.
        private class PodcastResult
        {
            public int FinalListeners;
            public int Gain;
            public int BaseListeners;
            public float TopicMod;
            public float SubgenreBonus;
            public float RawResult;
            public bool GenreMatched;
            public bool SubgenreMatched;
            public bool StrongSpin;
        }

        #endregion
    }
}