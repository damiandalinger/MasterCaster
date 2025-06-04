/// <summary>
/// A serializable reference wrapper for a string value, allowing selection between a constant value or a StringVariable.
/// Enables flexibility when designing systems in the Unity Inspector.
/// </summary>

/// <remarks>
/// 09/04/2025 by Damian Dalinger: Script Creation.
/// </remarks>

using System;

namespace ProjectCeros
{
    [Serializable]
    public class StringReference : BaseReference<string, StringVariable>
    {
        public StringReference() : base() { }

        public StringReference(string value) : base(value) { }

        protected override string GetValue()
        {
            return UseConstant ? ConstantValue : Variable.RuntimeValue;
        }

        public static implicit operator string(StringReference reference)
        {
            return reference.Value;
        }
    }
}
