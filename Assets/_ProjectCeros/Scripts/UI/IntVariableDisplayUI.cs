/// <summary>
/// Displays an IntVariable in a text field. 
/// </summary>

/// <remarks>
/// 09/05/2025 by Damian Dalinger: Script creation.
/// 18/06/2025 by Damian Dalinger: Added the update method.
/// 23/06/2025 by Damian Dalinger: Added the ordinal suffix option. 
/// </remarks>

using UnityEngine;
using TMPro;

namespace ProjectCeros
{

    public class IntVariableDisplayUI : MonoBehaviour
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
                string ordinal = _useOrdinalSuffix ? GetOrdinalSuffix(value) : "";
                _targetText.text = $"{_prefix}{value}{ordinal}{_suffix}";
            }
        }

        #endregion

        #region Private Methods

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