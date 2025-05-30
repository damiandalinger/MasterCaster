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
            Value += extra;
        }

        public void Append(StringVariable extra)
        {
            Value += extra;
        }

        public void Clear()
        {
            Value = "";
        }

        public void SetValue(string value)
        {
            Value = value;
        }

        public void SetValue(StringVariable value)
        {
            Value = value.Value;
        }

        #endregion
    }
}