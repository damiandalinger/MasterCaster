/// <summary>
/// A ScriptableObject-based container for a int value that allows runtime value modification.
/// Includes utility methods for setting, modifying, multiplying, dividing, and clamping the value.
/// </summary>

/// <remarks>
/// 09/04/2025 by Damian Dalinger: Script Creation.
/// </remarks>

using System;
using UnityEngine;

namespace ProjectCeros
{
    [CreateAssetMenu(menuName = "Variables/Int Variable")]
    public class IntVariable : ScriptableObject, ISaveable
    {
#if UNITY_EDITOR
        [Multiline]
        public string DeveloperDescription = "";
#endif

        public int Value;

        public string SaveKey => name;

        public object CaptureState() => Value;

        public void RestoreState(object state)
        {
            Value = Convert.ToInt32(state);
        }

        #region Public Methods
        #region ApplyChange
        public void ApplyChange(int amount)
        {
            Value += amount;
        }

        public void ApplyChange(IntVariable amount)
        {
            Value += amount.Value;
        }
        #endregion

        #region Clamp
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
        #endregion

        #region Divide
        public void Divide(int divisor)
        {
            Value /= divisor;
        }

        public void Divide(IntVariable divisor)
        {
            Value /= divisor.Value;
        }
        #endregion

        #region Multiply
        public void Multiply(int factor)
        {
            Value *= factor;
        }

        public void Multiply(IntVariable factor)
        {
            Value *= factor.Value;
        }
        #endregion

        #region SetValue
        public void SetValue(int value)
        {
            Value = value;
        }

        public void SetValue(IntVariable value)
        {
            Value = value.Value;
        }
        #endregion
        #endregion
    }
}
