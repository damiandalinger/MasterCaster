/// <summary>
/// Abstract base class for ScriptableObjects that support saving and loading.
/// Implements ISaveable and provides shared interface for all saveable variables.
/// </summary>

/// <remarks>
/// 04/06/2025 by Damian Dalinger: Script creation.
/// </remarks>

using UnityEngine;

namespace ProjectCeros
{

    public abstract class SaveableVariableBase : ScriptableObject, ISaveable
    {
        [Tooltip("Determines if this variable should be included in the save process.")]
        [SerializeField] protected bool _isSaveable = false;

        // Indicates whether the variable should be saved.
        public bool IsSaveable => _isSaveable;

        public abstract string SaveKey { get; }
        public abstract object CaptureState();
        public abstract void RestoreState(object state);
        public abstract void ResetToDefault();
    }
}
