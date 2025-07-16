/// <summary>
/// Displays a StringVariable in a textbox OnEnable.
/// </summary>

/// <remarks>
/// 09/07/2025 by Damian Dalinger: Script creation.
/// 16/07/2025 by Damian Dalinger: Made update optional.
/// </remarks>

using UnityEngine;
using TMPro;

namespace ProjectCeros
{
    public class StringToDisplay : MonoBehaviour
    {
        #region Fields

        [Tooltip("The string variable to read from.")]
        [SerializeField] private StringReference _stringVariable;

        [Tooltip("The TextMeshPro UI element to display the value in.")]
        [SerializeField] private TMP_Text _textDisplay;

        [Header("Optional Formatting")]
        [Tooltip("Text shown before the string value.")]
        [SerializeField] private string _prefix = "";

        [Tooltip("Text shown after the string value.")]
        [SerializeField] private string _suffix = "";

        [Tooltip("If the display should refresh every frame.")]
        [SerializeField] private bool _useUpdateMethod = false;

        #endregion

        #region Lifecycle Methods

        private void OnEnable()
        {
            UpdateDisplay();
        }

        private void Update()
        {
            if (_useUpdateMethod)
            {
                UpdateDisplay();
            }
        }

        #endregion

        #region Public Methods

        public void UpdateDisplay()
        {
            if (_stringVariable != null && _textDisplay != null)
            {
                _textDisplay.text = _prefix + _stringVariable.Value + _suffix;
            }
        }

        #endregion
    }
}
