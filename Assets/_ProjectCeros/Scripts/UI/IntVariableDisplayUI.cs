/// <summary>
/// Displays an IntVariable in a text field. 
/// </summary>

/// <remarks>
/// 09/05/2025 by Damian Dalinger: Script creation.
/// </remarks>

using UnityEngine;
using TMPro;

namespace ProjectCeros
{

    public class IntVariableDisplayUI : MonoBehaviour
    {
        [Tooltip("The IntVariable whose value will be displayed.")]
        [SerializeField] private IntReference _valueToDisplay;

        [Tooltip("The TextMeshProUGUI element used to show the value.")]
        [SerializeField] private TextMeshProUGUI _targetText;

        [Tooltip("Optional text displayed before the value (e.g., 'Day ' or 'Listeners: ').")]
        [SerializeField] private string _prefix = "";

        [Tooltip("Optional text displayed after the value (e.g., ' viewers' or ' points').")]
        [SerializeField] private string _suffix = "";

        private void OnEnable()
        {
            UpdateDisplay();
        }

        public void UpdateDisplay()
        {
            if (_targetText != null)
            {
                _targetText.text = $"{_prefix}{_valueToDisplay.Value}{_suffix}";
            }
        }
    }
}