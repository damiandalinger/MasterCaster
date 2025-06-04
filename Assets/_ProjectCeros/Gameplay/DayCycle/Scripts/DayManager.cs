/// <summary>
/// Manages day progression in the game by incrementing the current day
/// and raising a GameEvent to notify other systems of a new day start.
/// </summary>

/// <remarks>
/// 09/05/2025 by Damian Dalinger: Script creation.
/// </remarks>

using UnityEngine;

namespace ProjectCeros
{
    public class DayManager : MonoBehaviour
    {
        #region Fields

        [SerializeField, Tooltip("Event that is raised when a new day begins. Used to notify other systems.")]
        private GameEvent _onNewDayStarted;

        [SerializeField, Tooltip("Reference to the current in-game day variable. Increments when progressing to the next day.")]
        private IntReference _currentDay;

        #endregion

        #region Public Methods

        // Advances the game by one day.
        // Increments the current day and notifies listeners.
        public void NextDay()
        {
            _currentDay.Variable.ApplyChange(1);

            SaveManager.Instance.Save();
            
            _onNewDayStarted.Raise();
        }

        #endregion
    }
}
