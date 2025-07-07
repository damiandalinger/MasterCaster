/// <summary>
/// Binds a TMP_InputField to a StringVariable ScriptableObject with optional character limit.
/// </summary>

/// <remarks>
///07/07/2025 by Damian Dalinger: Script creation.
/// </remarks>

using TMPro;
using UnityEngine;

namespace ProjectCeros
{
    [RequireComponent(typeof(TMP_InputField))]
    public class InputToStringVariable : MonoBehaviour
    {
        #region Fields

        [Tooltip("The target string variable to assign the input value to.")]
        [SerializeField] private StringReference _targetVariable;

        [Tooltip("Optional max length (from an IntVariable). Leave empty to disable.")]
        [SerializeField] private IntReference _maxLength;

        private TMP_InputField _inputField;

        #endregion

        #region Lifecycle Methods

        private void Awake()
        {
            _inputField = GetComponent<TMP_InputField>();
            ApplyCharacterLimit();

            _inputField.onValueChanged.AddListener(OnValueChanged);
        }

        #endregion

        #region Private Methods

        // Applies a character limit to the input field if one is set.
        private void ApplyCharacterLimit()
        {
            if (_maxLength != null && _maxLength.Value > 0)
            {
                _inputField.characterLimit = _maxLength.Value;
            }
        }

        // Called when the user changes the input value.
        private void OnValueChanged(string newValue)
        {
            if (_targetVariable == null) return;

            if (_maxLength != null && _maxLength.Value > 0 && newValue.Length > _maxLength.Value)
            {
                newValue = newValue.Substring(0, _maxLength.Value);
                _inputField.text = newValue;
            }

            _targetVariable.Variable.SetValue(newValue);
        }

        #endregion
    }
}
