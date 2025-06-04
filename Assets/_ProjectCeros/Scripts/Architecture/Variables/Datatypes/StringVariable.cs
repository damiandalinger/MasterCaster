/// <summary>
/// A ScriptableObject-based container for a string value that allows runtime value modification.
/// Includes utility methods for setting, appending and clearing the value.
/// </summary>

/// <remarks>
/// 09/04/2025 by Damian Dalinger: Script Creation.
/// 28/05/2025 by Damian Dalinger: Changed to BaseVariable for saving.
/// </remarks>

using UnityEngine;

namespace ProjectCeros
{
    [CreateAssetMenu(menuName = "Variables/String Variable")]
    public class StringVariable : BaseVariable<string>
    {
        #region Public Methods

        public void Append(string extra)
        {
            RuntimeValue += extra;
        }

        public void Append(StringVariable extra)
        {
            RuntimeValue += extra;
        }

        public void Clear()
        {
            RuntimeValue = "";
        }

        public void SetValue(string value)
        {
            RuntimeValue = value;
        }

        public void SetValue(StringVariable value)
        {
            RuntimeValue = value.RuntimeValue;
        }

        #endregion
    }
}