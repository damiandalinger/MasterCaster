/// <summary>
/// A ScriptableObject-based container for a bool value that allows runtime value modification.
/// Includes utility methods for setting and toggling the value.
/// </summary>

/// <remarks>
/// 09/04/2025 by Damian Dalinger: Script Creation.
/// </remarks>

using UnityEngine;

namespace ProjectCeros
{
    [CreateAssetMenu(menuName = "Variables/Bool Variable")]
    public class BoolVariable : ScriptableObject
    {
#if UNITY_EDITOR
        [Multiline]
        public string DeveloperDescription = "";
#endif
        public bool Value;

        #region Public Methods
        #region SetValue
        public void SetValue(bool value)
        {
            Value = value;
        }

        public void SetValue(BoolVariable value)
        {
            Value = value.Value;
        }
        #endregion

        #region Toggle
        public void Toggle()
        {
        Value = !Value;
        }
        #endregion
        #endregion
    }
}