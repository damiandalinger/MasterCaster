/// <summary>
/// Utility component to set FMOD loop parameters via UnityEvents.
/// Supports setting both numeric and labeled parameters.
/// </summary>

/// <remarks>
/// 10/07/2025 by Damian Dalinger: Script creation.
/// </remarks>

using UnityEngine;

namespace ProjectCeros
{
    public class FMODParameterSetter : MonoBehaviour
    {
        #region Fields

        [Header("Target")]
        [Tooltip("The FMOD sound event to affect.")]
        [SerializeField] private FMODSoundEvent _soundEvent;

        [Tooltip("Transform where the FMOD loop is running.")]
        [SerializeField] private Transform _target;

        [Header("Parameter")]
        [Tooltip("Name of the parameter to change.")]
        [SerializeField] private string _parameterName;

        [Tooltip("Use label (string) instead of float value.")]
        [SerializeField] private bool _useLabel = false;

        [Tooltip("String label value to apply (used if Use Label is true).")]
        [SerializeField] private string _parameterLabel;

        [Tooltip("Float value to apply (used if Use Label is false).")]
        [SerializeField] private float _parameterValue;

        #endregion

        #region Public Methods

        // Applies the parameter change to the target FMOD event.
        public void SetParameter()
        {
            if (_useLabel)
            {
                _soundEvent.SetParameterLabel(_target, _parameterName, _parameterLabel);
            }
            else
            {
                _soundEvent.SetParameter(_target, _parameterName, _parameterValue);
            }
        }

        #endregion
    }
}
