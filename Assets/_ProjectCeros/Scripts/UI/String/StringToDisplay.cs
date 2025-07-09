/// <summary>
/// Displays a StringVariable in a textbox OnEnable.
/// </summary>

/// <remarks>
///09/07/2025 by Damian Dalinger: Script creation.
/// </remarks>

using UnityEngine;
using TMPro;

namespace ProjectCeros
{
    public class StringToDisplay : MonoBehaviour
    {
        [Tooltip("The string variable to read from.")]
        [SerializeField] private StringReference _stringVariable;

        [Tooltip("The TextMeshPro UI element to display the value in.")]
        [SerializeField] private TMP_Text _textDisplay;

        [Header("Optional Formatting")]
        [Tooltip("Text shown before the string value.")]
        [SerializeField] private string _prefix = "";

        [Tooltip("Text shown after the string value.")]
        [SerializeField] private string _suffix = "";

        private void OnEnable()
        {
            if (_stringVariable != null && _textDisplay != null)
            {
                _textDisplay.text = _prefix + _stringVariable.Value + _suffix;
            }
        }
    }
}
