using UnityEngine;
using UnityEngine.Events;

namespace ProjectCeros
{
    [System.Serializable]
    public class UnityIntEvent : UnityEvent<int> { }

    public class IntGameEventListener : MonoBehaviour
    {
        public IntGameEvent gameEvent;
        public UnityIntEvent response;

        private void OnEnable()
        {
            if (gameEvent != null)
                gameEvent.RegisterListener(this);
        }

        private void OnDisable()
        {
            if (gameEvent != null)
                gameEvent.UnregisterListener(this);
        }

        public void OnEventRaised(int value)
        {
            response.Invoke(value);

            // Debug.Log("Invoke with value " + value);
        }
    }
}