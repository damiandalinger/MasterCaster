/// <summary>
/// Displays an IntVariable in a text field. 
/// </summary>

/// <remarks>
/// 09/05/2025 by Damian Dalinger: Script creation.
/// 18/06/2025 by Damian Dalinger: Added the update method.
/// 23/06/2025 by Damian Dalinger: Added the ordinal suffix option. 
/// 08/07/2025 by Damian Dalinger: Added the thousand seperator.
/// </remarks>

using UnityEngine;
using TMPro;

namespace ProjectCeros
{

    public class IntToDisplay : MonoBehaviour
    {
        #region Fields

        [Tooltip("The IntVariable whose value will be displayed.")]
        [SerializeField] private IntReference _valueToDisplay;

        [Tooltip("The TextMeshProUGUI element used to show the value.")]
        [SerializeField] private TextMeshProUGUI _targetText;

        [Tooltip("Optional text displayed before the value (e.g., 'Day ' or 'Listeners: ').")]
        [SerializeField] private string _prefix = "";

        [Tooltip("Optional text displayed after the value (e.g., ' viewers' or ' points').")]
        [SerializeField] private string _suffix = "";

        [Tooltip("If the display should refresh every frame.")]
        [SerializeField] private bool _useUpdateMethod = false;

        [Tooltip("Automatically adds ordinal suffixes like 'st', 'nd', 'rd', 'th' to the displayed number.")]
        [SerializeField] private bool _useOrdinalSuffix = false;

        [Tooltip("Display the number using thousands separators (e.g., 1,000,000).")]
        [SerializeField] private bool _useThousandsSeparator = false;

        [Tooltip("Display a '+' sign for positive values.")]
        [SerializeField] private bool _showPlusSignForPositiveValues = false;

        [Tooltip("If true, the number is interpreted as a genre ID and displays its name instead.")]
        [SerializeField] private bool _displayAsGenre = false;

        #endregion

        #region Lifecycle Methods

        private void Update()
        {
            if (_useUpdateMethod)
            {
                UpdateDisplay();
            }
        }

        private void OnEnable()
        {
            UpdateDisplay();
        }

        #endregion

        #region Public Methods

        // Updates the text field with the current value of the assigned IntReference.
        public void UpdateDisplay()
        {
            if (_targetText != null)
            {
                int value = _valueToDisplay.Value;

                if (_displayAsGenre)
                {
                    string genreName = GetGenreName(value);
                    _targetText.text = $"{_prefix}{genreName}{_suffix}";
                    return;
                }

                string sign = "";
                if (_showPlusSignForPositiveValues && value > 0)
                    sign = "+";

                string numberString = _useThousandsSeparator
                    ? Mathf.Abs(value).ToString("N0")  // Optional: ohne Minus für eigene Sign-Logik
                    : Mathf.Abs(value).ToString();

                string ordinal = _useOrdinalSuffix ? GetOrdinalSuffix(value) : "";

                _targetText.text = $"{_prefix}{sign}{numberString}{ordinal}{_suffix}";
            }
        }

        #endregion

        #region Private Methods

        private string GetGenreName(int genreId)
        {
            return genreId switch
            {
                1 => "Action",
                2 => "Indie",
                3 => "Role-Playing Game",
                4 => "Shooter",
                5 => "Simulation",
                6 => "Strategy",
                _ => "Unknown"
            };
        }

        // Returns the English ordinal suffix for a given integer (e.g., "st" for 1, "nd" for 2, etc.).
        private string GetOrdinalSuffix(int number)
        {
            if (number <= 0) return "";

            int lastTwoDigits = number % 100;
            int lastDigit = number % 10;

            if (lastTwoDigits >= 11 && lastTwoDigits <= 13)
            {
                return "th";
            }

            return lastDigit switch
            {
                1 => "st",
                2 => "nd",
                3 => "rd",
                _ => "th",
            };
        }

        #endregion
    }
}