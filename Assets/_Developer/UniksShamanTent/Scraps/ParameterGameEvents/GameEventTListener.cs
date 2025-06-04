using ProjectCeros;
using UnityEngine;
using UnityEngine.Events;

public abstract class GameEventTListener<T> : MonoBehaviour
{
    public GameEventT<T> Event;
    public UnityEvent<T> Response;

    private void OnEnable()
    {
        Event.RegisterListener(this);
    }

    private void OnDisable()
    {
        Event.UnregisterListener(this);
    }

    public void OnEventRaised(T item)
    {
        Response.Invoke(item);
    }
}