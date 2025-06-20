/// <summary>
/// Handles podcast creation, listener gain calculation, and user feedback display based on genre and subgenre selection.
/// </summary>

/// <remarks>
/// 20/05/2025 by Damian Dalinger: Initial creation.
/// </remarks>

using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

namespace ProjectCeros
{
    public class PodcastInputHandler : MonoBehaviour
    {

        #region Fields

        [Header("UI")]
        [SerializeField] private TMP_Text _feedbackText;
        [SerializeField] private GameObject _selectionUI;
        [SerializeField] private GameObject _evaluationUI;

        [Header("Events")]
        [SerializeField] private GameEvent _onPodcastConfirmed;

        [Header("Core")]
        [SerializeField] private PodcastCalculator _calculator;
        [SerializeField] private PodcastResultVisualizer _resultVisualizer;
        [SerializeField] private TMP_InputField _titleInputField;
        [SerializeField] private StringRuntimeSet _podcastTitles;
        [SerializeField] private int _maxTitleLength = 32;
        private int _selectedGenre = -1;
        private int _selectedSpin = 0;
        private string _selectedSubgenre = string.Empty;

        #endregion

        #region LifeCycle Methods

        private void Start()
        {

            _titleInputField.characterLimit = _maxTitleLength;
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
            _selectedSpin = value;
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
                ResetSelection();
                _feedbackText.text = "Invalid selection – please choose a genre, spin, and subgenre.";
                return;
            }

            string title = _titleInputField.text.Trim();

            _podcastTitles.Add(title);


            var input = new PodcastInputData
            {
                Genre = _selectedGenre,
                Spin = _selectedSpin,
                Subgenre = _selectedSubgenre
            };

            _calculator.Calculate(input);

            _onPodcastConfirmed.Raise();
            _selectionUI.SetActive(false);
            _evaluationUI.SetActive(true);
        }

        #endregion

        #region Private Methods

        private bool IsValidSelection()
        {
            return _selectedGenre >= 1 && _selectedGenre <= 6 &&
                   (_selectedSpin == 1 || _selectedSpin == 2) &&
                    _titleInputField != null &&
           !string.IsNullOrWhiteSpace(_titleInputField.text);
        }

        private void ResetSelection()
        {
            _selectedGenre = -1;
            _selectedSpin = 0;
            _selectedSubgenre = string.Empty;
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

            text += _selectedSpin == 1
                ? "Spin: Positive\n"
                : _selectedSpin == 2
                    ? "Spin: Negative\n"
                    : "Spin: Not selected\n";

            text += !string.IsNullOrEmpty(_selectedSubgenre)
                ? $"Subgenre: {GetSubgenreDisplayName(_selectedSubgenre)}\n"
                : "Subgenre: Not selected\n";

            _feedbackText.text = text;
        }

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
    }
}