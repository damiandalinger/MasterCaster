/// <summary>
/// Invokes a UnityEvent once during OnEnable(). Useful for inspector-driven wiring.
/// </summary>

/// <remarks>
/// 11/08/2025 by Damian Dalinger: Script creation.
/// </remarks>

using UnityEngine;
using UnityEngine.Events;

namespace ProjectCeros
{
    public class InvokeUnityEventOnEnable : MonoBehaviour
    {
        [Tooltip("Invoked once during OnEnable(). Configure actions in the Inspector.")]
        [SerializeField] private UnityEvent _onEnable = new UnityEvent();

        private void OnEnable()
        {
            _onEnable?.Invoke();
        }
    }
}