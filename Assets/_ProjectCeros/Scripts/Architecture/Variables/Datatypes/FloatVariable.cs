/// <summary>
/// A ScriptableObject-based container for a float value that allows runtime value modification.
/// Includes utility methods for setting, modifying, multiplying, dividing, and clamping the value.
/// </summary>

/// <remarks>
/// 09/04/2025 by Damian Dalinger: Script Creation.
/// 28/05/2025 by Damian Dalinger: Changed to BaseVariable for saving.
/// </remarks>

using UnityEngine;

namespace ProjectCeros
{
    [CreateAssetMenu(menuName = "Variables/Float Variable")]
    public class FloatVariable : BaseVariable<float>
    {
        #region Public Methods

        public void ApplyChange(float amount)
        {
            RuntimeValue += amount;
        }

        public void ApplyChange(FloatVariable amount)
        {
            RuntimeValue += amount.RuntimeValue;
        }

        public void Clamp(float min, float max)
        {
            RuntimeValue = Mathf.Clamp(RuntimeValue, min, max);
        }

        public void Clamp(FloatVariable min, float max)
        {
            RuntimeValue = Mathf.Clamp(RuntimeValue, min.RuntimeValue, max);
        }

        public void Clamp(float min, FloatVariable max)
        {
            RuntimeValue = Mathf.Clamp(RuntimeValue, min, max.RuntimeValue);
        }

        public void Clamp(FloatVariable min, FloatVariable max)
        {
            RuntimeValue = Mathf.Clamp(RuntimeValue, min.RuntimeValue, max.RuntimeValue);
        }

        public void Divide(float divisor)
        {
            RuntimeValue /= divisor;
        }

        public void Divide(FloatVariable divisor)
        {
            RuntimeValue /= divisor.RuntimeValue;
        }

        public void Multiply(float factor)
        {
            RuntimeValue *= factor;
        }

        public void Multiply(FloatVariable factor)
        {
            RuntimeValue *= factor.RuntimeValue;
        }

        public void SetValue(float value)
        {
            RuntimeValue = value;
        }

        public void SetValue(FloatVariable value)
        {
            RuntimeValue = value.RuntimeValue;
        }

        #endregion
    }
}
