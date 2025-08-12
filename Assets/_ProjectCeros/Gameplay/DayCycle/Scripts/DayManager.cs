/// <summary>
/// Manages day progression in the game by incrementing the current day.
/// </summary>

/// <remarks>
/// 09/05/2025 by Damian Dalinger: Script creation.
/// 16/07/2025 by Damian Dalinger: Removed the newday event. 
/// </remarks>

using UnityEngine;

namespace ProjectCeros
{
    public class DayManager : MonoBehaviour
    {
        #region Fields

        [SerializeField, Tooltip("Reference to the current in-game day variable. Increments when progressing to the next day.")]
        private IntReference _currentDay;

        #endregion

        #region Public Methods

        // Advances the game by one day.
        // Increments the current day and notifies listeners.
        public void NextDay()
        {
            _currentDay.Variable.ApplyChange(1);

            StartCoroutine(SaveManager.Instance.Save());
        }

        #endregion
    }
}
