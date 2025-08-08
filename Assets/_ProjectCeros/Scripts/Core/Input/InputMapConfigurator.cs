/// <summary>
/// Configures which Input Action Maps to enable or disable when triggered.
/// </summary>

/// <remarks>
/// 11/06/2025 by Damian Dalinger: Script creation.
/// </remarks>

using UnityEngine;
using UnityEngine.InputSystem;

namespace ProjectCeros
{
    public class InputMapConfigurator : MonoBehaviour
    {
        #region Fields

        [Tooltip("The InputActionAsset to modify.")]
        [SerializeField] private InputActionAsset _inputActions;

        [Tooltip("Maps to enable on scene load.")]
        [SerializeField] private string[] _enableMaps;

        [Tooltip("Maps to disable on scene load.")]
        [SerializeField] private string[] _disableMaps;

        [Tooltip("If true, configuration is applied during Start().")]
        [SerializeField] private bool _applyOnStart = true;

        #endregion

        #region LifeCycle Methods

        private void Start()
        {
            if (_applyOnStart)
            {
                ApplyConfiguration();
            }
        }

        #endregion

        #region Public Methods

        // Applies the input map configuration by enabling and disabling specified maps.
        public void ApplyConfiguration()
        {
            foreach (var map in _enableMaps)
            {
                var actionMap = _inputActions.FindActionMap(map, true);
                if (actionMap != null)
                {
                    actionMap.Enable();
                }
            }

            foreach (var map in _disableMaps)
            {
                var actionMap = _inputActions.FindActionMap(map, true);
                if (actionMap != null)
                {
                    actionMap.Disable();
                }
            }
        }

        #endregion
    }
}
