/// <summary>
/// A ScriptableObject-based container for a Vector2 value that allows runtime value modification.
/// Includes utility methods for setting, modifying and normalizing the value.
/// </summary>

/// <remarks>
/// 09/04/2025 by Damian Dalinger: Script Creation.
/// 28/05/2025 by Damian Dalinger: Changed to BaseVariable for saving.
/// </remarks>

using UnityEngine;

namespace ProjectCeros
{
    [CreateAssetMenu(menuName = "Variables/Vector2 Variable")]
    public class Vector2Variable : BaseVariable<Vector2>
    {
        #region Public Methods

        public void ApplyChange(Vector2 amount)
        {
            Value += amount;
        }

        public void ApplyChange(Vector2Variable amount)
        {
            Value += amount.Value;
        }

        public void Normalize()
        {
            Value = Value.normalized;
        }

        public void SetValue(Vector2 value)
        {
            Value = value;
        }

        public void SetValue(Vector2Variable value)
        {
            Value = value.Value;
        }

        #endregion
    }
}