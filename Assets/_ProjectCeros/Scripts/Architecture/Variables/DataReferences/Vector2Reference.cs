/// <summary>
/// A serializable reference wrapper for a vector2 value, allowing selection between a constant value or a Vector2Variable.
/// Enables flexibility when designing systems in the Unity Inspector.
/// </summary>

/// <remarks>
/// 09/04/2025 by Damian Dalinger: Script Creation.
/// </remarks>

using UnityEngine;
using System;

namespace ProjectCeros
{
   [Serializable]
    public class Vector2Reference : BaseReference<Vector2, Vector2Variable>
    {
        public Vector2Reference() : base() { }

        public Vector2Reference(Vector2 value) : base(value) { }

        protected override Vector2 GetValue()
        {
            return UseConstant ? ConstantValue : Variable.RuntimeValue;
        }

        public static implicit operator Vector2(Vector2Reference reference)
        {
            return reference.Value;
        }
    }
}
