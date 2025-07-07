/// <summary>
/// Component that listens to a specific GameEvent and invokes a UnityEvent when triggered.
/// Attach this to objects that should react to events in the scene.
/// </summary>

/// <remarks>
/// 11/04/2025 by Damian Dalinger: Script creation.
/// </remarks>

using UnityEngine;
using UnityEngine.Events;

namespace ProjectCeros
{
    public class GameEventListener : MonoBehaviour
    {
        [Tooltip("Event to register with.")]
        public GameEvent Event;

        [Tooltip("Response to invoke when Event is raised.")]
        public UnityEvent Response;

        private void OnEnable()
        {
            Event.RegisterListener(this);
        }

        private void OnDisable()
        {
            Event.UnregisterListener(this);
        }

        public void OnEventRaised()
        {
            Response.Invoke();
        }
    }
}