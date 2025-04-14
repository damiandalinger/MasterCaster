/// <summary>
/// A ScriptableObject-based container for a float value that allows runtime value modification.
/// Includes utility methods for setting, modifying, multiplying, dividing, and clamping the value.
/// </summary>

/// <remarks>
/// 09/04/2025 by Damian Dalinger: Script Creation.
/// </remarks>

using UnityEngine;


namespace ProjectCeros
{
    [CreateAssetMenu(menuName = "Variables/FloatVariable")]
    public class FloatVariable : ScriptableObject
    {
#if UNITY_EDITOR
        [Multiline]
        public string DeveloperDescription = "";
#endif

        public float Value;

        #region Public Methods
        #region ApplyChange
        public void ApplyChange(float amount)
        {
            Value += amount;
        }

        public void ApplyChange(FloatVariable amount)
        {
            Value += amount.Value;
        }
        #endregion

        #region Clamp
        public void Clamp(float min, float max)
        {
            Value = Mathf.Clamp(Value, min, max);
        }

        public void Clamp(FloatVariable min, float max)
        {
            Value = Mathf.Clamp(Value, min.Value, max);
        }

        public void Clamp(float min, FloatVariable max)
        {
            Value = Mathf.Clamp(Value, min, max.Value);
        }

        public void Clamp(FloatVariable min, FloatVariable max)
        {
            Value = Mathf.Clamp(Value, min.Value, max.Value);
        }
        #endregion

        #region Divide
        public void Divide(float divisor)
        {
            Value /= divisor;
        }

        public void Divide(FloatVariable divisor)
        {
            Value /= divisor.Value;
        }
        #endregion

        #region Multiply
        public void Multiply(float factor)
        {
            Value *= factor;
        }

        public void Multiply(FloatVariable factor)
        {
            Value *= factor.Value;
        }
        #endregion

        #region SetValue
        public void SetValue(float value)
        {
            Value = value;
        }

        public void SetValue(FloatVariable value)
        {
            Value = value.Value;
        }
        #endregion
        #endregion
    }
}
