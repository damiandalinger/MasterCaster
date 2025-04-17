/// <summary>
/// Handles looped FMOD sound instances per GameObject.
/// Allows playing, stopping, and updating parameters on sound loops.
/// Automatically cleans up instances on destruction or disable.
/// </summary>

/// <remarks>
/// 17/04/2025 by Damian Dalinger: Script Creation.
/// </remarks>

using System.Collections.Generic;
using FMOD.Studio;
using FMODUnity;
using UnityEngine;

namespace ProjectCeros
{
    public class FMODSoundAutoHandler : MonoBehaviour
    {

        private Dictionary<FMODSoundEvent, EventInstance> _loopedInstances = new();

        // Starts a looped FMOD event instance if not already playing.
        public void PlayLoop(FMODSoundEvent soundEvent)
        {
            if (_loopedInstances.ContainsKey(soundEvent)) return;

            EventInstance instance = RuntimeManager.CreateInstance(soundEvent.EventReference);
            instance.start();

            _loopedInstances[soundEvent] = instance;
        }

        // Stops and releases a looped FMOD instance if active.
        public void StopLoop(FMODSoundEvent soundEvent)
        {
            if (!_loopedInstances.TryGetValue(soundEvent, out var instance)) return;

            instance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
            instance.release();

            _loopedInstances.Remove(soundEvent);
        }

        // Sets a parameter on a currently playing looped instance.
        public void SetParameter(FMODSoundEvent soundEvent, string parameterName, float parameterValue)
        {
            if (_loopedInstances.TryGetValue(soundEvent, out var instance))
            {
                instance.setParameterByName(parameterName, parameterValue);
            }
        }

        private void OnDisable()
        {
            Cleanup();
        }

        private void OnDestroy()
        {
            Cleanup();
        }

        private void Cleanup()
        {
            foreach (var instance in _loopedInstances.Values)
            {
                instance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
                instance.release();
            }

            _loopedInstances.Clear();
        }
    }
}