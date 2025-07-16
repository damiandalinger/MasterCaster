/// <summary>
/// Handles screen fade transitions with three phases: fade-in, hold (fully black), and fade-out.
/// Executes UnityEvents during the hold phase.
/// </summary>

/// <remarks>
/// 10/07/2025 by Damian Dalinger: Script creation.
/// </remarks>

using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

namespace ProjectCeros.UI
{
    public class ScreenFader : MonoBehaviour
    {
        #region Fields

        [Tooltip("Parent GameObject containing all fadeable UI elements.")]
        [SerializeField] private GameObject _fadeRoot;

        [Header("Fade Timing")]
        [Tooltip("Time it takes to fade from clear to black.")]
        [SerializeField] private FloatReference _fadeInDuration;

        [Tooltip("How long the screen remains fully black.")]
        [SerializeField] private FloatReference _holdDuration;

        [Tooltip("Time it takes to fade from black to clear.")]
        [SerializeField] private FloatReference _fadeOutDuration;

        [Header("Events")]
        [Tooltip("Events to invoke while screen is fully black.")]
        [SerializeField] private UnityEvent _onMidpointReached;

        [Tooltip("Events to invoke while screen is fully black.")]
        [SerializeField] private UnityEvent _onEndpointReached;

        private List<Graphic> _graphics = new();

        #endregion

        #region Unity Methods

        private void Awake()
        {
            if (_fadeRoot != null)
            {
                _graphics = new List<Graphic>(_fadeRoot.GetComponentsInChildren<Graphic>(includeInactive: true));
            }
        }

        #endregion

        #region Public Methods

        // Starts the fade sequence (fade in, hold, fade out) and fires events during the hold.
        public void FadeTransition()
        {
            StartCoroutine(FadeRoutine());
        }

        #endregion

        #region Private Methods

        private IEnumerator FadeRoutine()
        {
            _fadeRoot.gameObject.SetActive(true);

            yield return FadeAlpha(0f, 1f, _fadeInDuration);

            _onMidpointReached?.Invoke();
            yield return new WaitForSeconds(_holdDuration);

            yield return FadeAlpha(1f, 0f, _fadeOutDuration);

            _onEndpointReached?.Invoke();
            _fadeRoot.gameObject.SetActive(false);
        }

        // Gradually interpolates the alpha value of the fade image from a starting value to a target value over time.
        private IEnumerator FadeAlpha(float from, float to, float duration)
        {
            float time = 0f;

            while (time < duration)
            {
                float t = time / duration;
                float alpha = Mathf.Lerp(from, to, t);

                ApplyAlphaToGraphics(alpha);

                time += Time.deltaTime;
                yield return null;
            }

            ApplyAlphaToGraphics(to);
        }

        private void ApplyAlphaToGraphics(float alpha)
        {
            foreach (var graphic in _graphics)
            {
                if (graphic == null) continue;

                var color = graphic.color;
                color.a = alpha;
                graphic.color = color;
            }
        }

        #endregion
    }
}