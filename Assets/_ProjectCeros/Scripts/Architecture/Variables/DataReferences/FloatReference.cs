/// <summary>
/// A serializable reference wrapper for a float value, allowing selection between a constant value or a FloatVariable.
/// Enables flexibility when designing systems in the Unity Inspector.
/// </summary>

/// <remarks>
/// 09/04/2025 by Damian Dalinger: Script Creation.
/// </remarks>

using System;

namespace ProjectCeros
{
    [Serializable]
    public class FloatReference : BaseReference<float, FloatVariable>
    {
        public FloatReference() : base() { }

        public FloatReference(float value) : base(value) { }

        protected override float GetValue()
        {
            return UseConstant ? ConstantValue : Variable.RuntimeValue;
        }

        public static implicit operator float(FloatReference reference)
        {
            return reference.Value;
        }
    }
}
