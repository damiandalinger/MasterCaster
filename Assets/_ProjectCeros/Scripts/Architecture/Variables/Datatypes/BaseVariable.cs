/// <summary>
/// A generic ScriptableObject container for runtime variables of any type.
/// Provides save/load functionality via ISaveable.
/// </summary>

/// <remarks>
/// 28/05/2025 by Damian Dalinger: Script Creation.
/// 04/06/2025 by Damian Dalinger: Refactored to inherit from SaveableVariableBase for unified save handling.
/// </remarks>

using System;
using UnityEngine;

namespace ProjectCeros
{
    public abstract class BaseVariable<T> : SaveableVariableBase
    {
        #region Fields

#if UNITY_EDITOR
        [Multiline]
        [Tooltip("Editor-only description of what this variable is used for.")]
        public string DeveloperDescription = "";
#endif

        [Tooltip("The default value used when resetting.")]
        public T InitialValue;

        [Tooltip("The current value of the variable.")]
        public T RuntimeValue;

        #endregion

        #region ISaveable

        // The unique key for saving and loading this variable.
        // Defaults to the asset's name.
        public override string SaveKey => name;

        // Captures the current value for saving.
        public override object CaptureState() => RuntimeValue;

        // Restores the saved value from persisted data.
        public override void RestoreState(object state)
        {
            RuntimeValue = (T)Convert.ChangeType(state, typeof(T));
        }

        // Resets the variable to its initial value.
        public override void ResetToDefault()
        {
            RuntimeValue = InitialValue;
        }

        #endregion
    }
}