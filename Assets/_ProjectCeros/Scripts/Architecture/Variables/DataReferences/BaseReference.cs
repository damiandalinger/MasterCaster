/// <summary>
/// A generic base class for variable references that can either use a constant or a ScriptableObject variable.
/// </summary>

/// <remarks>
/// 11/04/2025 by Damian Dalinger: Base reference system created.
/// </remarks>

using UnityEngine;
using System;

namespace ProjectCeros
{
    [Serializable]
    public class BaseReference<TValue, TVariable> where TVariable : ScriptableObject
    {
        public bool UseConstant = true;
        public TValue ConstantValue;
        public TVariable Variable;

        public BaseReference() { }

        public BaseReference(TValue value)
        {
            UseConstant = true;
            ConstantValue = value;
        }

        public TValue Value => UseConstant ? ConstantValue : GetValue();

        protected virtual TValue GetValue()
        {
            return default;
        }
    }
}
