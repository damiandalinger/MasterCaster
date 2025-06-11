/// <summary>
/// Manages game pause and resume states using a configurable key and game events.
/// </summary>

/// <remarks>
/// 27/05/2025 by Damian Dalinger: Initial creation.
/// 28/05/2025 by Damian Dalinger: Updated to the new Input System.
/// </remarks>

using UnityEngine;
using UnityEngine.InputSystem;

namespace ProjectCeros
{
    public class PauseMenuManager : MonoBehaviour
    {
        #region Fields

        [Tooltip("The key used to toggle pause/resume.")]
        [SerializeField] private InputActionReference _pauseAction;

        [Tooltip("Raised when the game is paused.")]
        [SerializeField] private GameEvent _onGamePaused;

        [Tooltip("Raised when the game is resumed.")]
        [SerializeField] private GameEvent _onGameResumed;

        private bool _isPaused;

        #endregion

        #region Lifecycle Methods

        // Unregisters the pause input callback and disables the input action when the object is deactivated.
        private void OnDisable()
        {
            _pauseAction.action.performed -= OnPauseInput;
            _pauseAction.action.Disable();
        }

        // Registers the pause input callback and enables the input action when the object becomes active.
        private void OnEnable()
        {
            _pauseAction.action.performed += OnPauseInput;
            _pauseAction.action.Enable();
        }

        #endregion

        #region Public Methods

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

        // Handles the performed input event for the pause action and toggles game pause state accordingly.
        private void OnPauseInput(InputAction.CallbackContext context)
        {
            if (!_isPaused)
                TriggerPause();
            else
                TriggerResume();
        }

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

        #endregion
    }
}
