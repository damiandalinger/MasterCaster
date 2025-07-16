/// <summary>
/// Provides a central, decoupled way to trigger a game pause via the PauseMenuManager, without requiring direct scene references or dependencies.
/// </summary>

/// <remarks>
/// 16/07/2025 by Damian Dalinger: Initial implementation.
/// </remarks>

using UnityEngine;

namespace ProjectCeros
{
    public class PauseInvoker : MonoBehaviour
    {
        #region Public Methods

        // Triggers the global pause logic by locating the PauseMenuManager and calling TriggerPause().
        public void PauseGame()
        {
            var manager = FindFirstObjectByType<PauseMenuManager>();
            manager?.TriggerPause();
        }

        #endregion
    }
}
