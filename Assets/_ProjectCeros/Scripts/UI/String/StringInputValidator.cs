/// <summary>
/// Enables or disables a UI GameObject (e.g. Button or close icon) depending on whether all required string variables are non-empty.
/// </summary>

/// <remarks>
///07/07/2025 by Damian Dalinger: Script creation.
/// </remarks>

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ProjectCeros
{
    public class StringInputValidator : MonoBehaviour
    {
        #region Fields

        [Tooltip("All StringVariables that must be non-empty.")]
        [SerializeField] private List<StringVariable> _requiredInputs = new();

        [Tooltip("Target object to enable/disable (e.g. Button or close icon).")]
        [SerializeField] private GameObject _targetToEnable;

        #endregion

        #region Lifecycle Methods

        private void Update()
        {
            ValidateInputs();
        }

        #endregion

        #region Private Methods

        // Checks whether all required input fields are filled and updates the target object's visibility and interactability.
        private void ValidateInputs()
        {
            bool allFilled = true;

            foreach (var variable in _requiredInputs)
            {
                if (variable == null || string.IsNullOrWhiteSpace(variable.RuntimeValue))
                {
                    allFilled = false;
                    break;
                }
            }

            if (_targetToEnable != null)
            {
                _targetToEnable.SetActive(allFilled);

                if (_targetToEnable.TryGetComponent(out Button button))
                {
                    button.interactable = allFilled;
                }
            }
        }

        #endregion
    }
}
