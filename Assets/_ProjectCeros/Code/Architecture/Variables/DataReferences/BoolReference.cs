/// <summary>
/// A serializable reference wrapper for a bool value, allowing selection between a constant value or a BoolVariable.
/// Enables flexibility when designing systems in the Unity Inspector.
/// </summary>

/// <remarks>
/// 09/04/2025 by Damian Dalinger: Script Creation.
/// </remarks>

using System;

namespace ProjectCeros
{
     [Serializable]
    public class BoolReference : BaseReference<bool, BoolVariable>
    {
        public BoolReference() : base() { }

        public BoolReference(bool value) : base(value) { }

        protected override bool GetValue()
        {
            return UseConstant ? ConstantValue : Variable.Value;
        }

        public static implicit operator bool(BoolReference reference)
        {
            return reference.Value;
        }
    }
}
