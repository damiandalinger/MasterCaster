/// <summary>
/// ScriptableObject wrapper for FMOD event references.
/// Supports 2D one-shot playback and managed loops with parameters.
/// </summary>

/// <remarks>
/// 17/04/2025 by Damian Dalinger: Script Creation.
/// </remarks>

using UnityEngine;
using FMODUnity;

namespace ProjectCeros
{
    [CreateAssetMenu(menuName = "Audio/FMOD Sound Event")]
    public class FMODSoundEvent : ScriptableObject
    {
#if UNITY_EDITOR
        [Multiline] public string DeveloperDescription = "";
#endif
        [SerializeField] private EventReference _eventReference;
        public EventReference EventReference => _eventReference;

        public void Play()
        {
            RuntimeManager.PlayOneShot(_eventReference);
        }

        // Starts a managed loop on a target GameObject.
        public void PlayLoop(Transform target)
        {
            var handler = GetOrAddHandler(target);
            handler.PlayLoop(this);
        }

        // Stops the looped instance on a target GameObject.
        public void StopLoop(Transform target)
        {
            var handler = target.GetComponent<FMODSoundAutoHandler>();
            handler?.StopLoop(this);
        }

        // Sets a parameter value on a looped instance on the target.
        public void SetParameter(Transform target, string parameterName, float parameterValue)
        {
            var handler = target.GetComponent<FMODSoundAutoHandler>();
            handler?.SetParameter(this, parameterName, parameterValue);
        }

        private FMODSoundAutoHandler GetOrAddHandler(Transform target)
        {
            var handler = target.GetComponent<FMODSoundAutoHandler>();
            if (handler == null)
                handler = target.gameObject.AddComponent<FMODSoundAutoHandler>();
            return handler;
        }
    }
}