/// <summary>
/// A ScriptableObject-based container for a bool value that allows runtime value modification.
/// Includes utility methods for setting and toggling the value.
/// </summary>

/// <remarks>
/// 09/04/2025 by Damian Dalinger: Script Creation.
/// 28/05/2025 by Damian Dalinger: Changed to BaseVariable for saving.
/// </remarks>

using UnityEngine;

namespace ProjectCeros
{
    [CreateAssetMenu(menuName = "Variables/Bool Variable")]
    public class BoolVariable : BaseVariable<bool>
    {
        #region Public Methods

        public void SetValue(bool value)
        {
            RuntimeValue = value;
        }

        public void SetValue(BoolVariable value)
        {
            RuntimeValue = value.RuntimeValue;
        }

        public void Toggle()
        {
            RuntimeValue = !RuntimeValue;
        }

        #endregion
    }
}