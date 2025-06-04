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
            RuntimeValue += amount;
        }

        public void ApplyChange(IntVariable amount)
        {
            RuntimeValue += amount.RuntimeValue;
        }

        public void Clamp(int min, int max)
        {
            RuntimeValue = Mathf.Clamp(RuntimeValue, min, max);
        }

        public void Clamp(IntVariable min, int max)
        {
            RuntimeValue = Mathf.Clamp(RuntimeValue, min.RuntimeValue, max);
        }

        public void Clamp(int min, IntVariable max)
        {
            RuntimeValue = Mathf.Clamp(RuntimeValue, min, max.RuntimeValue);
        }

        public void Clamp(IntVariable min, IntVariable max)
        {
            RuntimeValue = Mathf.Clamp(RuntimeValue, min.RuntimeValue, max.RuntimeValue);
        }

        public void Divide(int divisor)
        {
            RuntimeValue /= divisor;
        }

        public void Divide(IntVariable divisor)
        {
            RuntimeValue /= divisor.RuntimeValue;
        }

        public void Multiply(int factor)
        {
            RuntimeValue *= factor;
        }

        public void Multiply(IntVariable factor)
        {
            RuntimeValue *= factor.RuntimeValue;
        }

        public void SetValue(int value)
        {
            RuntimeValue = value;
        }

        public void SetValue(IntVariable value)
        {
            RuntimeValue = value.RuntimeValue;
        }

        #endregion
    }
}
