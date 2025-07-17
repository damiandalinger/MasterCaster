/// <summary>
/// Delays an event by a specified number of seconds after being triggered.
/// Can be used to sequence effects or time-based reactions.
/// </summary>

/// <remarks>
/// 17/07/2025 by Damian Dalinger: Initial implementation.
/// </remarks>

using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace ProjectCeros
{
    public class DelayedEventInvoker : MonoBehaviour
    {
        #region Fields

        [Tooltip("Time to wait (in seconds) before triggering the delayed event.")]
        [SerializeField] private FloatReference _delayInSeconds;

        [Tooltip("Event to call after the delay.")]
        [SerializeField] private UnityEvent _onDelayComplete;

        private Coroutine _activeRoutine;

        #endregion

        #region Public Methods

        // Starts the delay countdown and triggers the event afterward.
        public void Trigger()
        {
            if (_activeRoutine != null)
                StopCoroutine(_activeRoutine);

            _activeRoutine = StartCoroutine(DelayCoroutine());
        }

        #endregion

        #region Private Methods

        private IEnumerator DelayCoroutine()
        {
            yield return new WaitForSeconds(_delayInSeconds);
            _onDelayComplete?.Invoke();
            _activeRoutine = null;
        }

        #endregion
    }
}
