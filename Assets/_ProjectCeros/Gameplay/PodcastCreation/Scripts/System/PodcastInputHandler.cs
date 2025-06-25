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

        [SerializeField] private IntVariable _selectedGenre;
        [SerializeField] private IntVariable _selectedSpin;
        [SerializeField] private IntVariable _selectedSubgenre;

        #endregion

        #region LifeCycle Methods

        private void Start()
        {
            ResetSelection();
            _titleInputField.characterLimit = _maxTitleLength;

            if (_podcastTitles.Items.Count > 0)
            {
                _titleInputField.placeholder.GetComponent<TMP_Text>().text = _podcastTitles.Items.Last();
            }
            else
            {
                _titleInputField.placeholder.GetComponent<TMP_Text>().text = "Enter title...";
            }

            UpdateFeedback();

        }

        #endregion

        #region Public Methods

        // Sets the selected genre.
        public void SelectGenre(int genreId)
        {
            _selectedGenre.RuntimeValue = genreId;
            UpdateFeedback();
        }

        // Sets the selected spin (1 = Positive, 2 = Negative).
        public void SelectSpin(int spin)
        {
            _selectedSpin.RuntimeValue = spin;
            UpdateFeedback();
        }

        // Sets the selected subgenre.
        public void SelectSubgenre(int subgenre)
        {
            _selectedSubgenre.RuntimeValue = subgenre;
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

            if (string.IsNullOrWhiteSpace(title))
            {
                if (_podcastTitles.Items.Count > 0)
                {
                    string rawLastTitle = _podcastTitles.Items.Last();

                    // Entferne ggf. das letzte #X-Suffix
                    string baseTitle = rawLastTitle;
                    int lastHashIndex = rawLastTitle.LastIndexOf('#');
                    if (lastHashIndex > 0)
                    {
                        string maybeNumber = rawLastTitle.Substring(lastHashIndex + 1);
                        if (int.TryParse(maybeNumber, out _))
                        {
                            baseTitle = rawLastTitle.Substring(0, lastHashIndex).TrimEnd();
                        }
                    }

                    int count = 2;
                    string generatedTitle = $"{baseTitle} #{count}";
                    while (_podcastTitles.Items.Contains(generatedTitle))
                    {
                        count++;
                        generatedTitle = $"{baseTitle} #{count}";
                    }

                    title = generatedTitle;
                }
                else
                {
                    _feedbackText.text = "Please enter a title for your first podcast.";
                    return;
                }
            }

            _podcastTitles.Add(title);
            _titleInputField.text = "";
            _titleInputField.placeholder.GetComponent<TMP_Text>().text = title;
            _calculator.Calculate();

            _onPodcastConfirmed.Raise();
            _selectionUI.SetActive(false);
            _evaluationUI.SetActive(true);
        }

        #endregion

        #region Private Methods

        private bool IsValidSelection()
        {
            bool hasTitleOrFallback = _podcastTitles.Items.Count > 0 ||
                                      !string.IsNullOrWhiteSpace(_titleInputField.text);

            return _selectedGenre.RuntimeValue >= 1 && _selectedGenre.RuntimeValue <= 6 &&
                   _selectedSubgenre.RuntimeValue >= 1 && _selectedSubgenre.RuntimeValue <= 18 &&
                   (_selectedSpin.RuntimeValue == 1 || _selectedSpin.RuntimeValue == 2) &&
                   hasTitleOrFallback;
        }

        private void ResetSelection()
        {
            _selectedGenre.RuntimeValue = 0;
            _selectedSpin.RuntimeValue = 0;
            _selectedSubgenre.RuntimeValue = 0;
        }

        // Updates the feedback UI with current selection state.
        private void UpdateFeedback()
        {
            if (_feedbackText == null)
                return;

            var text = "Current Selection:\n";

            text += _selectedGenre.RuntimeValue >= 1 && _selectedGenre.RuntimeValue <= 6
                ? $"Genre: {GetGenreName(_selectedGenre.RuntimeValue)}\n"
                : "Genre: Not selected\n";

            text += _selectedSpin.RuntimeValue == 1
                ? "Spin: Positive\n"
                : _selectedSpin.RuntimeValue == 2
                    ? "Spin: Negative\n"
                    : "Spin: Not selected\n";

            text += _selectedSubgenre.RuntimeValue >= 1 && _selectedSubgenre
                ? $"Subgenre: {GetSubgenreDisplayName(_selectedSubgenre.RuntimeValue)}\n"
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

        private string GetSubgenreDisplayName(int subgenreId)
        {
            return subgenreId switch
            {
                1 => "FPS",
                2 => "Hero Shooter",
                3 => "Loot Shooter",
                4 => "Fighting Game",
                5 => "Stealth Game",
                6 => "Hack & Slash",
                7 => "Souls Like",
                8 => "Open World",
                9 => "MMORPG",
                10 => "RTS",
                11 => "Grand Strategy",
                12 => "TBS",
                13 => "Sport",
                14 => "Living Simulation",
                15 => "Job Simulation",
                16 => "Farming Game",
                17 => "Side Scroller",
                18 => "Roguelike",
                _ => "Unknown"
            };
        }

        #endregion
    }
}