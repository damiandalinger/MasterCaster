/// <summary>
/// A ScriptableObject-based event that can notify all registered listeners when raised.
/// Enables decoupled communication between game systems.
/// </summary>

/// <remarks>
/// 11/04/2025 by Damian Dalinger: Script creation.
/// </remarks>

using System.Collections.Generic;
using UnityEngine;

namespace ProjectCeros
{
    [CreateAssetMenu]
    public class GameEvent : ScriptableObject
    {

        // The list of listeners that this event will notify if it is raised.
        public readonly List<GameEventListener> eventListeners =
            new List<GameEventListener>();

        public void Raise()
        {
#if UNITY_EDITOR
            // Log the event to the editor-only Event Log window (excluded from builds).
            EventLogWindow.LogEvent(name, this);
#endif
            // Looping backwards, in case the listeners respond includes removing it from the list.
            for (int i = eventListeners.Count - 1; i >= 0; i--)
                eventListeners[i].OnEventRaised();
        }

        public void RegisterListener(GameEventListener listener)
        {
            if (!eventListeners.Contains(listener))
                eventListeners.Add(listener);
        }

        public void UnregisterListener(GameEventListener listener)
        {
            if (eventListeners.Contains(listener))
                eventListeners.Remove(listener);
        }
    }
}