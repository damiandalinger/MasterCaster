/// <summary>
/// A generic ScriptableObject that holds a dynamic list of runtime objects of type T.
/// Useful for managing instances without direct references.
/// </summary>

/// <remarks>
/// 14/04/2025 by Damian Dalinger: Script creation.
/// 30/05/2025 by Damian Dalinger: Implemented the save methods.
/// </remarks>

using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;

namespace ProjectCeros
{
    public abstract class RuntimeSet<T> : ScriptableObject, ISaveable
    {
        #region Fields

        [Tooltip("List of runtime instances currently tracked.")]
        public List<T> Items = new List<T>();

        #endregion

        #region Properties

        public virtual string SaveKey => name;

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

        // Captures the current list of items for saving.
        public virtual object CaptureState()
        {
            if (typeof(T).IsSerializable)
            {
                return Items;
            }
            else
            {
#if UNITY_EDITOR
                Debug.LogWarning($"{name}: Type {typeof(T)} is not serializable. RuntimeSet will not be saved.");
#endif
                return null;
            }
        }


        // Restores the list of items from saved state.
        public virtual void RestoreState(object state)
        {
            if (!typeof(T).IsSerializable || state == null)
                return;

            try
            {
                if (state is List<T> typedList)
                {
                    Items = new List<T>(typedList);
                }
                else
                {
                    Items = JsonConvert.DeserializeObject<List<T>>(state.ToString());
                }
            }
            catch (Exception ex)
            {
                Debug.LogError($"{name}: Failed to restore RuntimeSet state for {typeof(T)}. Error: {ex.Message}");
            }
        }

        #endregion
    }
}