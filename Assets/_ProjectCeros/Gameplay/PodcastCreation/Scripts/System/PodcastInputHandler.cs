/// <summary>
/// Handles podcast creation logic: title input, genre/spin/subgenre selection, and validation.
/// Triggers calculation and confirmation event if input is valid.
/// </summary>

/// <remarks>
/// 20/05/2025 by Damian Dalinger: Initial creation.
/// 09/07/2025 by Damian Dalinger: Added the guest episode feature.
/// </remarks>

using System.Linq;
using TMPro;
using UnityEngine;

namespace ProjectCeros
{
    public class PodcastInputHandler : MonoBehaviour
    {

        #region Fields

        [Header("Core References")]
        [Tooltip("Performs the podcast listener gain calculation.")]
        [SerializeField] private PodcastCalculator _calculator;

        [Tooltip("Input field where the user enters the normal podcast title.")]
        [SerializeField] private TMP_InputField _titleInputFieldNormal;

        [Tooltip("Input field where the user enters the podcast title of the guest episode.")]
        [SerializeField] private TMP_InputField _titleInputFieldGuest;

        [Tooltip("Confirmation button shown when selection is valid.")]
        [SerializeField] private GameObject _confirmButtonNormal;

        [Tooltip("Confirmation button shown when selection is valid at the guest episode.")]
        [SerializeField] private GameObject _confirmButtonGuest;

        [Tooltip("List of all created podcast titles.")]
        [SerializeField] private StringRuntimeSet _podcastTitles;

        [Tooltip("Maximum number of characters allowed for podcast titles.")]
        [SerializeField] private IntReference _maxTitleLength;

        [Tooltip("Whether this is a guestEpisode or not")]
        [SerializeField] private bool _isGuestEpisode = false;

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

            _titleInputFieldNormal.characterLimit = _maxTitleLength;
            _titleInputFieldGuest.characterLimit = _maxTitleLength;

            _titleInputFieldNormal.onValueChanged.AddListener(_ => UpdateConfirmButtonVisibility());
            _titleInputFieldGuest.onValueChanged.AddListener(_ => UpdateConfirmButtonVisibility());

            if (_podcastTitles.Items.Count > 0)
            {
                string last = _podcastTitles.Items.Last();
                _titleInputFieldNormal.placeholder.GetComponent<TMP_Text>().text = last;
                _titleInputFieldGuest.placeholder.GetComponent<TMP_Text>().text = last;
            }
            else
            {
                _titleInputFieldNormal.placeholder.GetComponent<TMP_Text>().text = "Enter title...";
                _titleInputFieldGuest.placeholder.GetComponent<TMP_Text>().text = "Enter title...";
            }
        }

        #endregion

        #region Public Methods

        // Confirms the current selection and triggers podcast calculation if valid.
        public void ConfirmSelection()
        {
            if (!IsValidSelection())
                return;

            string title = GetActiveTitleInput().Trim();

            if (string.IsNullOrWhiteSpace(title))
            {
                title = GenerateFallbackTitle();
                if (string.IsNullOrEmpty(title))
                    return;
            }

            _podcastTitles.Add(title);

            // Reset both input fields
            _titleInputFieldNormal.text = "";
            _titleInputFieldGuest.text = "";

            _titleInputFieldNormal.placeholder.GetComponent<TMP_Text>().text = title;
            _titleInputFieldGuest.placeholder.GetComponent<TMP_Text>().text = title;

            _calculator.Calculate();
        }

        // Enables the special case of a guest episode. 
        public void EnableGuestMode()
        {
            _isGuestEpisode = true;

            _selectedGenre.RuntimeValue = 100;
            _selectedSpin.RuntimeValue = 100;
            _selectedSubgenre.RuntimeValue = 100;

            UpdateConfirmButtonVisibility();
        }

        // Sets the selected genre index and refreshes button visibility.
        public void SelectGenre(int genreId)
        {
            _selectedGenre.RuntimeValue = genreId;
            _isGuestEpisode = false;
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
                                       !string.IsNullOrWhiteSpace(GetActiveTitleInput());

            if (_isGuestEpisode)
                return hasTitleOrFallback;

            return _selectedGenre.RuntimeValue >= 1 && _selectedGenre.RuntimeValue <= 6 &&
                   _selectedSubgenre.RuntimeValue >= 1 && _selectedSubgenre.RuntimeValue <= 18 &&
                   (_selectedSpin.RuntimeValue == 1 || _selectedSpin.RuntimeValue == 2) &&
                   hasTitleOrFallback;
        }

        private string GetActiveTitleInput()
        {
            return _isGuestEpisode ? _titleInputFieldGuest.text : _titleInputFieldNormal.text;
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
            bool isValid = IsValidSelection();

            _confirmButtonNormal.SetActive(!_isGuestEpisode && isValid);
            _confirmButtonGuest.SetActive(_isGuestEpisode && isValid);
        }

        #endregion
    }
}