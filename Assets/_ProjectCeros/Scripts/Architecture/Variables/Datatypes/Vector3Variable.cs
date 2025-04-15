/// <summary>
/// A ScriptableObject-based container for a Vector3 value that allows runtime value modification.
/// Includes utility methods for setting, modifying and normalizing the value.
/// </summary>

/// <remarks>
/// 09/04/2025 by Damian Dalinger: Script Creation.
/// </remarks>

using UnityEngine;


namespace ProjectCeros
{
    [CreateAssetMenu(menuName = "Variables/Vector3Variable")]
    public class Vector3Variable : ScriptableObject
    {
#if UNITY_EDITOR
        [Multiline]
        public string DeveloperDescription = "";
#endif
        public Vector3 Value;

        #region Public Methods
        #region ApplyChange
        public void ApplyChange(Vector3 amount)
        {
            Value += amount;
        }

        public void ApplyChange(Vector3Variable amount)
        {
            Value += amount.Value;
        }
        #endregion

        #region Normalize
        public void Normalize()
        {
            Value = Value.normalized;
        }
        #endregion

        #region SetValue
        public void SetValue(Vector3 value)
        {
            Value = value;
        }

        public void SetValue(Vector3Variable value)
        {
            Value = value.Value;
        }
        #endregion
        #endregion
    }
}