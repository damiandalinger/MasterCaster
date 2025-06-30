/// <summary>
/// Toggles a list of GameObjects on or off simultaneously.
/// </summary>

/// <remarks>
/// 20/05/2025 by Damian Dalinger: Initial creation.
/// 30/06/2025 by Damian Dalinger: Added the activate and deactivate methods.
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

        // Toggles the active state of all target GameObjects.
        public void Toggle()
        {
            _isActive = !_isActive;
            SetActiveState(_isActive);
        }

        // Activates all target GameObjects.
        public void Activate()
        {
            _isActive = true;
            SetActiveState(true);
        }

        // Deactivates all target GameObjects.
        public void Deactivate()
        {
            _isActive = false;
            SetActiveState(false);
        }

        #endregion

        #region Private Methods

        // Sets the active state of each target GameObject.
        private void SetActiveState(bool state)
        {
            foreach (var obj in _targets)
            {
                if (obj != null)
                    obj.SetActive(state);
            }
        }

        #endregion
    }
}