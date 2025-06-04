/// <summary>
/// A serializable reference wrapper for a int value, allowing selection between a constant value or a IntVariable.
/// Enables flexibility when designing systems in the Unity Inspector.
/// </summary>

/// <remarks>
/// 09/04/2025 by Damian Dalinger: Script Creation.
/// </remarks>

using System;

namespace ProjectCeros
{
    [Serializable]
    public class IntReference : BaseReference<int, IntVariable>
    {
        public IntReference() : base() { }

        public IntReference(int value) : base(value) { }

        protected override int GetValue()
        {
            return UseConstant ? ConstantValue : Variable.RuntimeValue;
        }

        public static implicit operator int(IntReference reference)
        {
            return reference.Value;
        }
    }
}
