/// <summary>
/// Manages game pause and resume states using a configurable key and game events.
/// </summary>

/// <remarks>
/// 27/05/2025 by Damian Dalinger: Initial creation.
/// </remarks>

using UnityEngine;

namespace ProjectCeros
{
    public class PauseMenuManager : MonoBehaviour
    {
        #region Fields

        [Tooltip("The key used to toggle pause/resume.")]
        [SerializeField] private KeyCode _pauseKey = KeyCode.Escape;

        [Tooltip("Raised when the game is paused.")]
        [SerializeField] private GameEvent _onGamePaused;

        [Tooltip("Raised when the game is resumed.")]
        [SerializeField] private GameEvent _onGameResumed;

        private bool _isPaused;

        #endregion

        #region Lifecycle Methods

        private void Update()
        {
            if (Input.GetKeyDown(_pauseKey))
            {
                if (!_isPaused)
                    TriggerPause();
                else
                    TriggerResume();
            }
        }

        #endregion

        #region Public Methods

        // Pauses the game and raises the pause event.
        public void TriggerPause()
        {
            if (_isPaused) return;
            _isPaused = true;
            Time.timeScale = 0;
            _onGamePaused?.Raise();
        }

        // Resumes the game and raises the resume event.
        public void TriggerResume()
        {
            if (!_isPaused) return;
            _isPaused = false;
            Time.timeScale = 1;
            _onGameResumed?.Raise();
        }

        // Called externally to pause the game.
        public void OnGamePausedExternally()
        {
            TriggerPause();
        }

        // Called externally to resume the game.
        public void OnGameResumedExternally()
        {
            TriggerResume();
        }

        #endregion
    }
}
