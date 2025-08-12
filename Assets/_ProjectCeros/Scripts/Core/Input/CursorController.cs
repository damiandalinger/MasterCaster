/// <summary>
/// Controls the visibility and lock state of the mouse cursor.
/// Can be called from UI buttons, events, or code to enable/disable the cursor.
/// </summary>

/// <remarks>
/// 12/08/2025 by Damian Dalinger: Initial implementation.
/// </remarks>

using UnityEngine;

namespace ProjectCeros
{
    public class CursorController : MonoBehaviour
    {

        #region Fields

        [Tooltip("If true, ShowCursor() will be called on enable.")]
        [SerializeField] private bool _showOnEnable = false;

        [Tooltip("If true, HideCursor() will be called on enable.")]
        [SerializeField] private bool _hideOnEnable = false;

        [Tooltip("If true, HideCursorConfined() will be called on enable.")]
        [SerializeField] private bool _hideConfinedOnEnable = false;

        #endregion

        #region Lifecycle Methods

        private void OnEnable()
        {
            if (_showOnEnable) ShowCursor();
            if (_hideOnEnable) HideCursor();
            if (_hideConfinedOnEnable) HideCursorConfined();
        }

        #endregion

        #region Public Methods

        // Makes the cursor visible and unlocks it.
        public void ShowCursor()
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }

        // Hides the cursor and locks it to the center of the screen.
        public void HideCursor()
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }

        // Hides the cursor but allows it to move inside the game window (not locked to center).
        public void HideCursorConfined()
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Confined;
        }

        #endregion
    }
}
