/// <summary>
/// A serializable reference wrapper for a vector3 value, allowing selection between a constant value or a Vector3Variable.
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
    public class Vector3Reference : BaseReference<Vector3, Vector3Variable>
    {
        public Vector3Reference() : base() { }

        public Vector3Reference(Vector3 value) : base(value) { }

        protected override Vector3 GetValue()
        {
            return UseConstant ? ConstantValue : Variable.RuntimeValue;
        }

        public static implicit operator Vector3(Vector3Reference reference)
        {
            return reference.Value;
        }
    }
}
