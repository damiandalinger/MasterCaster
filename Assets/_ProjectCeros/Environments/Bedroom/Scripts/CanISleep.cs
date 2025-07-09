/// <summary>
/// Handles conditional display of a TextMeshPro element based on a BoolVariable.
/// If the condition is false, shows the text temporarily. If true, raises a GameEvent.
/// </summary>

/// <remarks>
/// 09/07/2025 by Damian Dalinger: Initial implementation.
/// </remarks>

using UnityEngine;
using System.Collections;
using TMPro;

namespace ProjectCeros
{

    public class CanISleep : MonoBehaviour
    {
        #region Fields

        [Header("Conditions")]
        [Tooltip("Condition that determines whether sleep is allowed. If true, the GameEvent is raised.")]
        [SerializeField] private BoolReference _condition;

        [Tooltip("GameEvent to raise if the condition is true.")]
        [SerializeField] private GameEvent _onConditionTrue;

        [Header("Text Target")]
        [Tooltip("The TextMeshPro component to fade in and out.")]
        [SerializeField] private TMP_Text _text;

        [Tooltip("Duration in seconds for the fade in and fade out transitions.")]
        [SerializeField] private float _fadeDuration = 0.5f;

        [Tooltip("How long (in seconds) the text should remain visible before fading out.")]
        [SerializeField] private float _visibleDuration = 10f;

        private Coroutine currentRoutine;

        #endregion

        #region Lifecycle Methods

        private void Awake()
        {
            SetAlpha(0f);
            _text.gameObject.SetActive(false);
        }

        #endregion

        #region Public Methods

        // Evaluates the condition. If true, raises the GameEvent. If false, shows the text temporarily.
        public void CheckCondition()
        {
            if (_condition.Value)
            {
                _onConditionTrue?.Raise();
            }
            else
            {
                if (currentRoutine != null)
                    StopCoroutine(currentRoutine);

                currentRoutine = StartCoroutine(FadeInAndOut());
            }
        }

        #endregion

        #region Private Methods

        // Handles the full sequence of showing, waiting, and hiding the text with fade transitions.
        private IEnumerator FadeInAndOut()
        {
            _text.gameObject.SetActive(true);
            yield return FadeAlpha(0f, 1f, _fadeDuration);

            yield return new WaitForSeconds(_visibleDuration);

            yield return FadeAlpha(1f, 0f, _fadeDuration);
            _text.gameObject.SetActive(false);
            currentRoutine = null;
        }

        // Gradually fades the text's alpha from one value to another over time.
        private IEnumerator FadeAlpha(float from, float to, float duration)
        {
            float time = 0f;

            while (time < duration)
            {
                time += Time.deltaTime;
                float alpha = Mathf.Lerp(from, to, time / duration);
                SetAlpha(alpha);
                yield return null;
            }

            SetAlpha(to);
        }

        // Sets the alpha channel of the text's color.
        private void SetAlpha(float alpha)
        {
            Color c = _text.color;
            c.a = alpha;
            _text.color = c;
        }

        #endregion
    }
}
