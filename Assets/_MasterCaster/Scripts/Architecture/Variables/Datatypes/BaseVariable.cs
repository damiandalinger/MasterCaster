/// <summary>
/// A generic ScriptableObject container for runtime variables of any type.
/// Provides save/load functionality via ISaveable.
/// </summary>

/// <remarks>
/// 28/05/2025 by Damian Dalinger: Script Creation.
/// </remarks>

using System;
using UnityEngine;

namespace ProjectCeros
{
    public abstract class BaseVariable<T> : ScriptableObject, ISaveable
    {
        #region Fields

#if UNITY_EDITOR
        [Multiline]
        [Tooltip("Editor-only description of what this variable is used for.")]
        public string DeveloperDescription = "";
#endif

        [Tooltip("The current value of the variable.")]
        public T Value;

        #endregion

        #region ISaveable

        // The unique key for saving and loading this variable.
        // Defaults to the asset's name.
        public virtual string SaveKey => name;

        // Captures the current value for saving.
        public virtual object CaptureState() => Value;

        // Restores the saved value from persisted data.
        public virtual void RestoreState(object state)
        {
            Value = (T)Convert.ChangeType(state, typeof(T));
        }

        #endregion
    }
}