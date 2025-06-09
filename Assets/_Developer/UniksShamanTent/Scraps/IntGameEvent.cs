using UnityEngine;
using UnityEngine.Events;

namespace ProjectCeros
{
    [CreateAssetMenu(menuName = "Events/Float Game Event")]
    public class IntGameEvent : ScriptableObject
    {
        private readonly System.Collections.Generic.List<IntGameEventListener> listeners = new();

        public void Raise(int value)
        {
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