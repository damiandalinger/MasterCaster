/// <summary>
/// Manages ambient FMOD parameters based on game state (items, PC status, weather).
/// </summary>

/// <remarks>
/// 10/07/2025 by Damian Dalinger: Initial implementation.
/// </remarks>

using UnityEngine;

namespace ProjectCeros
{
    public class AmbienceManager : MonoBehaviour
    {
        [Header("FMOD")]
        [Tooltip("Ambient FMOD event.")]
        [SerializeField] private FMODSoundEvent _ambienceEvent;

        [Tooltip("Target object where the loop is running.")]
        [SerializeField] private Transform _target;

        [Header("Item Tracking")]
        [Tooltip("Runtime set of currently active items.")]
        [SerializeField] private IntRuntimeSet _activeItems;

        [Tooltip("Item ID that triggers the parameter 'Items' to 'ItemBought'.")]
        [SerializeField] private int _specialItemID = 17;

        [Tooltip("Bool variable indicating whether the PC is on.")]
        [SerializeField] private BoolVariable _isPCOn;

        [Tooltip("Chance for rain when a day ends (0 to 1).")]
        [SerializeField] private FloatReference _rainChance;

        #region Lifecycle Events

        private void Update()
        {
            ApplyItemParameter();
            ApplyPCParameter();
        }

        #endregion

        #region Public Methods

        // Should be called at the end of a day to determine and apply weather.
        public void OnDayEnded()
        {
            string weather = Random.value < _rainChance ? "Rain" : "NoRain";
            _ambienceEvent.SetParameterLabel(_target, "Weather", weather);
        }

        // Updates the 'Items' parameter based on whether the item is in the active set.
        public void ApplyItemParameter()
        {
            string value = _activeItems.Items.Contains(_specialItemID) ? "ItemBought" : "ItemNotBought";
            _ambienceEvent.SetParameterLabel(_target, "Items", value);
        }

        // Updates the 'PC' parameter based on a bool variable.
        public void ApplyPCParameter()
        {
            string value = _isPCOn.RuntimeValue ? "PCon" : "PCoff";
            _ambienceEvent.SetParameterLabel(_target, "PC", value);
        }

        #endregion
    }
}
