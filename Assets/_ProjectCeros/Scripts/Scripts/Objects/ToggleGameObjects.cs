/// <summary>
/// Toggles a list of GameObjects on or off simultaneously.
/// </summary>

/// <remarks>
/// 20/05/2025 by Damian Dalinger: Initial creation.
/// </remarks>

using UnityEngine;

namespace ProjectCeros
{
    public class ToggleGameObjects : MonoBehaviour
    {
        #region Fields

        [Tooltip("The GameObjects to be toggled on or off.")]
        [SerializeField] private GameObject[] _targets;

        private bool _isActive = true;

        #endregion

        #region Public Methods

        // Toggles the active state of all assigned GameObjects.
        public void Toggle()
        {
            _isActive = !_isActive;

            foreach (var gameObject in _targets)
            {
                if (gameObject != null)
                {
                    gameObject.SetActive(_isActive);
                }
            }
        }

        #endregion
    }
}
