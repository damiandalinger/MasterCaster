/// <summary>
/// A generic ScriptableObject that holds a dynamic list of runtime objects of type T.
/// Useful for managing instances without direct references.
/// </summary>

/// <remarks>
/// 14/04/2025 by Damian Dalinger: Script creation.
/// </remarks>

using System.Collections.Generic;
using UnityEngine;

namespace ProjectCeros
{
    public abstract class RuntimeSet<T> : ScriptableObject
    {
        public List<T> Items = new List<T>();

        public void Add(T thing)
        {
            if (!Items.Contains(thing))
                Items.Add(thing);
        }

        public void Remove(T thing)
        {
            if (Items.Contains(thing))
                Items.Remove(thing);
        }
    }
}