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

namespace ProjectCeros.UI
{
    public class ScreenFader : MonoBehaviour
    {
        #region Fields

        [Tooltip("Image component used for the fade (black fullscreen image).")]
        [SerializeField] private Image _fadeImage;

        [Header("Fade Timing")]
        [Tooltip("Time it takes to fade from clear to black.")]
        [SerializeField] private FloatReference _fadeInDuration;

        [Tooltip("How long the screen remains fully black.")]
        [SerializeField] private FloatReference _holdDuration;

        [Tooltip("Time it takes to fade from black to clear.")]
        [SerializeField] private FloatReference _fadeOutDuration;

        [Header("Events")]
        [Tooltip("Events to invoke while screen is fully black.")]
        [SerializeField] private UnityEvent _onFadeMidpoint;

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
            _fadeImage.gameObject.SetActive(true);

            yield return FadeAlpha(0f, 1f, _fadeInDuration);

            _onFadeMidpoint?.Invoke();
            yield return new WaitForSeconds(_holdDuration);

            yield return FadeAlpha(1f, 0f, _fadeOutDuration);

            _fadeImage.gameObject.SetActive(false);
        }

        // Gradually interpolates the alpha value of the fade image from a starting value to a target value over time.
        private IEnumerator FadeAlpha(float from, float to, float duration)
        {
            float time = 0f;
            Color color = _fadeImage.color;

            while (time < duration)
            {
                time += Time.deltaTime;
                float t = time / duration;
                color.a = Mathf.Lerp(from, to, t);
                _fadeImage.color = color;
                yield return null;
            }

            color.a = to;
            _fadeImage.color = color;
        }

        #endregion
    }
}
