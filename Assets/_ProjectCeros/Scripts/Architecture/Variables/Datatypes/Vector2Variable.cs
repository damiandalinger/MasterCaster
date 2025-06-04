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
            RuntimeValue += amount;
        }

        public void ApplyChange(Vector2Variable amount)
        {
            RuntimeValue += amount.RuntimeValue;
        }

        public void Normalize()
        {
            RuntimeValue = RuntimeValue.normalized;
        }

        public void SetValue(Vector2 value)
        {
            RuntimeValue = value;
        }

        public void SetValue(Vector2Variable value)
        {
            RuntimeValue = value.RuntimeValue;
        }

        #endregion
    }
}