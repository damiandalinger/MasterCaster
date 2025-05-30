/// <summary>
/// A ScriptableObject-based container for a int value that allows runtime value modification.
/// Includes utility methods for setting, modifying, multiplying, dividing, and clamping the value.
/// </summary>

/// <remarks>
/// 09/04/2025 by Damian Dalinger: Script Creation.
/// 28/05/2025 by Damian Dalinger: Changed to BaseVariable for saving.
/// </remarks>

using UnityEngine;

namespace ProjectCeros
{
    [CreateAssetMenu(menuName = "Variables/Int Variable")]
    public class IntVariable : BaseVariable<int>
    {
        #region Public Methods

        public void ApplyChange(int amount)
        {
            Value += amount;
        }

        public void ApplyChange(IntVariable amount)
        {
            Value += amount.Value;
        }

        public void Clamp(int min, int max)
        {
            Value = Mathf.Clamp(Value, min, max);
        }

        public void Clamp(IntVariable min, int max)
        {
            Value = Mathf.Clamp(Value, min.Value, max);
        }

        public void Clamp(int min, IntVariable max)
        {
            Value = Mathf.Clamp(Value, min, max.Value);
        }

        public void Clamp(IntVariable min, IntVariable max)
        {
            Value = Mathf.Clamp(Value, min.Value, max.Value);
        }

        public void Divide(int divisor)
        {
            Value /= divisor;
        }

        public void Divide(IntVariable divisor)
        {
            Value /= divisor.Value;
        }

        public void Multiply(int factor)
        {
            Value *= factor;
        }

        public void Multiply(IntVariable factor)
        {
            Value *= factor.Value;
        }

        public void SetValue(int value)
        {
            Value = value;
        }

        public void SetValue(IntVariable value)
        {
            Value = value.Value;
        }

        #endregion
    }
}
