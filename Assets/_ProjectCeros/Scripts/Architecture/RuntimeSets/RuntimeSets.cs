/// <summary>
/// A generic ScriptableObject that holds a dynamic list of runtime objects of type T.
/// Useful for managing instances without direct references.
/// </summary>

/// <remarks>
/// 14/04/2025 by Damian Dalinger: Script creation.
/// 30/05/2025 by Damian Dalinger: Implemented the save methods.
/// 04/06/2025 by Damian Dalinger: Refactored to inherit SaveableVariableBase and auto-register.
/// </remarks>

using System.Collections.Generic;
using UnityEngine;

namespace ProjectCeros
{
    public abstract class RuntimeSet<T> : SaveableVariableBase
    {
        #region Fields

        [Tooltip("List of runtime instances currently tracked.")]
        public List<T> Items = new List<T>();

        #endregion

        #region Properties

        // The key used to save and load this RuntimeSet.
        public override string SaveKey => name;

        // The number of items currently tracked.
        public int Count => Items.Count;

        #endregion

        #region Public Methods

        // Adds an object to the set if it is not already present.
        public void Add(T thing)
        {
            if (!Items.Contains(thing))
                Items.Add(thing);
        }

        // Removes an object from the set if it exists.
        public void Remove(T thing)
        {
            if (Items.Contains(thing))
                Items.Remove(thing);
        }

        // Clears the entire runtime set.
        public void Clear()
        {
            Items.Clear();
        }

        #endregion

        #region ISaveable

        // Captures the current list of items for saving.
        public override object CaptureState()
        {
            return Items;
        }

        // Restores the list of items from saved state.
        public override void RestoreState(object state)
        {
            if (state is List<T> list)
                Items = new List<T>(list);
        }

        // Resets the runtime state to its default (empty) state.
        public override void ResetToDefault()
        {
            Clear();
        }

        #endregion
    }
}