using UnityEngine;
using UnityEngine.Events;

namespace ProjectCeros
{
    public class PauseMenuHandler : MonoBehaviour
    {
        [Header("Pause Config")]
        [SerializeField] private KeyCode _pauseKey = KeyCode.Escape;

        [Header("Events")]
        [Tooltip("Called when the pause menu should be shown.")]
        [SerializeField] private UnityEvent _onPauseTriggered;

        [Tooltip("Called when the pause menu should be hidden.")]
        [SerializeField] private UnityEvent _onResumeTriggered;

        private bool _isPaused;

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

        public void TriggerPause()
        {
            _isPaused = true;
            _onPauseTriggered?.Invoke();
        }

        public void TriggerResume()
        {
            _isPaused = false;
            _onResumeTriggered?.Invoke();
        }
    }
}
