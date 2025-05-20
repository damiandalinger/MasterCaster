/// <summary>
/// Listens for the current day value and triggers a UnityEvent once when a new day is detected.
/// </summary>

/// <remarks>
/// 20/05/2025 by Damian Dalinger: Initial creation.
/// </remarks>

using UnityEngine;
using UnityEngine.Events;

namespace ProjectCeros
{
    public class OnNewDayTrigger : MonoBehaviour
    {
        #region Fields

        [Tooltip("The current day as tracked by the DayManager.")]
        [SerializeField] private IntReference _currentDay;

        [Tooltip("UnityEvent invoked when a new day is detected.")]
        [SerializeField] private UnityEvent _onNewDay;

        private int _lastInitializedDay = -1;

        #endregion

        #region LifeCycle Methods

        private void Start()
        {
            TryTriggerNewDayEvent();
        }

        #endregion

        #region Private Methods

        // Triggers the new day event if the current day has changed since the last known day.
        private void TryTriggerNewDayEvent()
        {
            if (_currentDay == null || _currentDay.Value <= 0)
                return;

            if (_lastInitializedDay != _currentDay.Value)
            {
                _lastInitializedDay = _currentDay.Value;
                _onNewDay?.Invoke();
            }
        }

        #endregion
    }
}