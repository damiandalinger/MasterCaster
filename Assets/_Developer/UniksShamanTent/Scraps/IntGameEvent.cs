// <summary>
/// A ScriptableObject-based event that can notify all registered listeners when raised.
/// Enables decoupled communication between game systems.
/// This version hands over an int value along raising an event.
/// </summary>

/// <remarks>
/// 18/06/2025 by Unik Kelmendi: Initial creation.
/// </remarks>

using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;

namespace ProjectCeros
{
    [CreateAssetMenu(menuName = "Events/Float Game Event")]
    public class IntGameEvent : ScriptableObject
    {
        // The list of listeners that this event will notify if it is raised.
        private readonly List<IntGameEventListener> listeners = new();

        public void Raise(int value)
        {
            // Looping backwards, in case the listeners respond includes removing it from the list.
            for (int i = listeners.Count - 1; i >= 0; i--)
            {
                listeners[i].OnEventRaised(value);
            }

            Debug.Log("Raise value " + value);

        }

        public void RegisterListener(IntGameEventListener listener)
        {
            if (!listeners.Contains(listener))
                listeners.Add(listener);
        }

        public void UnregisterListener(IntGameEventListener listener)
        {
            listeners.Remove(listener);
        }
    }
}