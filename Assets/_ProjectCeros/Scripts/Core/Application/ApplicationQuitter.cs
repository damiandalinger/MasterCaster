/// <summary>
/// Provides a method to quit the application. Intended to be used by UI buttons.
/// </summary>

/// <remarks>
/// 10/07/2025 by Damian Dalinger: Initial implementation.
/// </remarks>

using UnityEngine;

namespace ProjectCeros
{
    public class ApplicationQuitter : MonoBehaviour
    {
        // Quits the application. Has no effect in the editor unless simulation is enabled.
        public void Quit()
        {
            Application.Quit();
        }
    }
}
