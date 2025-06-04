using UnityEngine;
using System.Collections.Generic;

namespace ProjectCeros
{
    [CreateAssetMenu(fileName = "GameEventT", menuName = "Scriptable Objects/GameEventT")]
    public abstract class GameEventT<T> : ScriptableObject
    {
        private readonly List<GameEventTListener<T>> listeners = new();

        public void Raise(T value)
        {
            for (int i = listeners.Count - 1; i >= 0; i--)
                listeners[i].OnEventRaised(value);
        }

        public void RegisterListener(GameEventTListener<T> listener)
        {
            if (!listeners.Contains(listener))
                listeners.Add(listener);
        }

        public void UnregisterListener(GameEventTListener<T> listener)
        {
            if (listeners.Contains(listener))
                listeners.Remove(listener);
        }
    }
}
