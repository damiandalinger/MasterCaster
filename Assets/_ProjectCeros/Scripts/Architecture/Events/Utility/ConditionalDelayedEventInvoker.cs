/// <summary>
/// Invokes a UnityEvent only after BOTH a minimum delay has elapsed and an external condition is met.
/// Designed for save screens: start the timer when showing the screen, and call OnConditionMet() from a GameEvent once saving completes.
/// </summary>

/// <remarks>
/// 12/08/2025 by Damian Dalinger: Script creation.
/// </remarks>

using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace ProjectCeros
{
    public class ConditionalDelayedEventInvoker : DelayedEventInvoker
    {
        #region Fields

        [Tooltip("Event invoked once delay elapsed AND the external condition was met.")]
        [SerializeField] private UnityEvent _onDelayAndConditionComplete;

        private bool _timeElapsed;
        private bool _conditionMet;

        #endregion

        #region Lifecycle Methods

        private void OnDisable()
        {
            // Clean up.
            if (_activeRoutine != null)
            {
                StopCoroutine(_activeRoutine);
                _activeRoutine = null;
            }
            _timeElapsed = false;
            _conditionMet = false;
        }

        #endregion

        #region Public Methods

        // Mark the external condition as satisfied (e.g., called from a GameEvent listener).
        public void OnConditionMet()
        {
            _conditionMet = true;
            TryInvoke();
        }

        #endregion

        #region Protected Methods

        protected override void HandleDelayElapsed()
        {
            _timeElapsed = true;
            TryInvoke();
        }

        #endregion

        #region Private Methods

        private void TryInvoke()
        {
            if (!_timeElapsed || !_conditionMet)
                return;

            _onDelayAndConditionComplete?.Invoke();
            
            _timeElapsed = false;
            _conditionMet = false;
        }

        #endregion
    }
}
