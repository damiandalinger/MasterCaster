/// <summary>
/// Handles podcast creation logic: title input, genre/spin/subgenre selection, and validation.
/// Triggers calculation and confirmation event if input is valid.
/// </summary>

/// <remarks>
/// 20/05/2025 by Damian Dalinger: Initial creation.
/// </remarks>

using System.Linq;
using TMPro;
using UnityEngine;

namespace ProjectCeros
{
    public class PodcastInputHandler : MonoBehaviour
    {

        #region Fields

        [Tooltip("Raised when the podcast is confirmed.")]
        [SerializeField] private GameEvent _onPodcastConfirmed;

        [Header("Core References")]
        [Tooltip("Performs the podcast listener gain calculation.")]
        [SerializeField] private PodcastCalculator _calculator;

        [Tooltip("Input field where the user enters the podcast title.")]
        [SerializeField] private TMP_InputField _titleInputField;

        [Tooltip("Confirmation button shown when selection is valid.")]
        [SerializeField] private GameObject _confirmButton;

        [Tooltip("List of all created podcast titles.")]
        [SerializeField] private StringRuntimeSet _podcastTitles;

        [Tooltip("Maximum number of characters allowed for podcast titles.")]
        [SerializeField] private IntReference _maxTitleLength;

        [Header("User Selection")]
        [Tooltip("Selected genre index (1–6).")]
        [SerializeField] private IntVariable _selectedGenre;

        [Tooltip("Selected spin index (1 = positive, 2 = negative).")]
        [SerializeField] private IntVariable _selectedSpin;

        [Tooltip("Selected subgenre index (1–18).")]
        [SerializeField] private IntVariable _selectedSubgenre;

        #endregion

        #region LifeCycle Methods

        private void Start()
        {
            ResetSelection();

            _titleInputField.characterLimit = _maxTitleLength;
            _titleInputField.onValueChanged.AddListener(_ => UpdateConfirmButtonVisibility());

            if (_podcastTitles.Items.Count > 0)
            {
                _titleInputField.placeholder.GetComponent<TMP_Text>().text = _podcastTitles.Items.Last();
            }
            else
            {
                _titleInputField.placeholder.GetComponent<TMP_Text>().text = "Enter title...";
            }
        }

        #endregion

        #region Public Methods

        // Confirms the current selection and triggers podcast calculation if valid.
        public void ConfirmSelection()
        {
            if (!IsValidSelection())
            {
                return;
            }

            string title = _titleInputField.text.Trim();

            if (string.IsNullOrWhiteSpace(title))
            {
                title = GenerateFallbackTitle();
                if (string.IsNullOrEmpty(title))
                    return;
            }

            _podcastTitles.Add(title);
            _titleInputField.text = "";
            _titleInputField.placeholder.GetComponent<TMP_Text>().text = title;

            _calculator.Calculate();
            _onPodcastConfirmed.Raise();
        }

        // Sets the selected genre index and refreshes button visibility.
        public void SelectGenre(int genreId)
        {
            _selectedGenre.RuntimeValue = genreId;
            UpdateConfirmButtonVisibility();

        }

        // Sets the selected spin index and refreshes button visibility.
        public void SelectSpin(int spin)
        {
            _selectedSpin.RuntimeValue = spin;
            UpdateConfirmButtonVisibility();
        }

        // Sets the selected subgenre index and refreshes button visibility.
        public void SelectSubgenre(int subgenre)
        {
            _selectedSubgenre.RuntimeValue = subgenre;
            UpdateConfirmButtonVisibility();
        }

        // Resets all selection values to their default state.
        public void ResetSelection()
        {
            _selectedGenre.RuntimeValue = 0;
            _selectedSpin.RuntimeValue = 0;
            _selectedSubgenre.RuntimeValue = 0;
        }

        #endregion

        #region Private Methods

        // Validates that a complete selection and title exists.
        private bool IsValidSelection()
        {
            bool hasTitleOrFallback = _podcastTitles.Items.Count > 0 ||
                                      !string.IsNullOrWhiteSpace(_titleInputField.text);

            return _selectedGenre.RuntimeValue >= 1 && _selectedGenre.RuntimeValue <= 6 &&
                   _selectedSubgenre.RuntimeValue >= 1 && _selectedSubgenre.RuntimeValue <= 18 &&
                   (_selectedSpin.RuntimeValue == 1 || _selectedSpin.RuntimeValue == 2) &&
                   hasTitleOrFallback;
        }

        // Generates a fallback title by incrementing the last used title.
        private string GenerateFallbackTitle()
        {
            if (_podcastTitles.Items.Count == 0)
                return "";

            string lastTitle = _podcastTitles.Items.Last();

            int hashIndex = lastTitle.LastIndexOf('#');
            string baseTitle = hashIndex > 0 && int.TryParse(lastTitle[(hashIndex + 1)..], out _)
                ? lastTitle.Substring(0, hashIndex).TrimEnd()
                : lastTitle;

            int count = 2;
            string candidate = $"{baseTitle} #{count}";

            while (_podcastTitles.Items.Contains(candidate))
                candidate = $"{baseTitle} #{++count}";

            return candidate;
        }

        // Updates the visibility of the confirm button based on current selection state.
        private void UpdateConfirmButtonVisibility()
        {
            _confirmButton.SetActive(IsValidSelection());
        }

        #endregion
    }
}