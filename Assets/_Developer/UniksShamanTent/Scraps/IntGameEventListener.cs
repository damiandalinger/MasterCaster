/// <summary>
/// Component that listens to a specific GameEvent and invokes a UnityEvent when triggered.
/// Attach this to objects that should react to events in the scene.
/// Works for the int based game events.
/// </summary>

/// <remarks>
/// 18/06/2025 by Unik Kelmendi: Initial creation.
/// </remarks>


using UnityEngine;
using UnityEngine.Events;

namespace ProjectCeros
{
    [System.Serializable]
    public class UnityIntEvent : UnityEvent<int> { }

    public class IntGameEventListener : MonoBehaviour
    {
        [Tooltip("Event to register with.")]
        public IntGameEvent GameEvent;

        [Tooltip("Response to invoke when Event is raised.")]
        public UnityIntEvent response;

        private void OnEnable()
        {
            if (GameEvent != null)
                GameEvent.RegisterListener(this);
        }

        private void OnDisable()
        {
            if (GameEvent != null)
                GameEvent.UnregisterListener(this);
        }

        public void OnEventRaised(int value)
        {
            response.Invoke(value);

            // Debug.Log("Invoke with value " + value);
        }
    }
}