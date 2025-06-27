/// <summary>
/// Displays an animated star rating from 0 to 5 using filled UI images.
/// Includes pop effects for newly filled stars and supports interruption.
/// </summary>

/// <remarks>
/// 23/06/2025 by Damian Dalinger: Script Creation.
/// 27/06/2025 by Damian Dalinger: Implemented ITabInterruptible.
/// </remarks>

using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

namespace ProjectCeros
{
    public class StarRatingVisualizer : MonoBehaviour, ITabInterruptible
    {
        #region Fields

        public bool IsBusy => _starAnimation != null;

        [Tooltip("UI star images to be filled (should use 'Filled' Image type).")]
        [SerializeField] private List<Image> _filledStars;

        [Tooltip("The rating value to visualize (expected range: 0.0 – 5.0).")]
        [SerializeField] private FloatReference _targetRating;

        [Header("Animation Settings")]
        [Tooltip("Total duration of the fill animation.")]
        [SerializeField] private FloatReference _animationDuration;

        [Tooltip("Delay before starting the animation.")]
        [SerializeField] private FloatReference _animationDelay;

        [Tooltip("Easing curve used for fill animation.")]
        [SerializeField] private AnimationCurve _fillCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);

        private Coroutine _starAnimation;
        private bool _hasAnimated = false;
        private List<bool> _wasFilled = new List<bool>();

        #endregion

        #region Lifecycle Methods

        private void Awake()
        {
            _wasFilled = new List<bool>(new bool[_filledStars.Count]);
        }

        private void OnEnable()
        {
            if (_hasAnimated)
            {
                SetRating(_targetRating.Value);
            }
            else
            {
                ResetAnimation();
                _starAnimation = StartCoroutine(AnimateStarsCoroutine());
            }
        }

        private void OnDisable()
        {
            SkipToEnd();
        }

        #endregion

        #region Public Methods

        // Skips the Animation and displays the finished state.
        public void SkipToEnd()
        {
            if (_starAnimation != null)
            {
                StopCoroutine(_starAnimation);
                _starAnimation = null;
            }

            SetRating(_targetRating.Value);
            _hasAnimated = true;
        }

        #endregion

        #region Private Methods

        // Coroutine that animates the gradual filling of stars with optional pop effect.
        private IEnumerator AnimateStarsCoroutine()
        {
            yield return new WaitForSeconds(_animationDelay);

            float elapsed = 0f;

            while (elapsed < _animationDuration)
            {
                elapsed += Time.deltaTime;
                float t = Mathf.Clamp01(elapsed / _animationDuration);
                float eased = _fillCurve.Evaluate(t);
                float currentRating = Mathf.Lerp(0f, _targetRating, eased);

                for (int i = 0; i < _filledStars.Count; i++)
                {
                    float fill = Mathf.Clamp01(currentRating - i);
                    _filledStars[i].fillAmount = fill;

                    if (!_wasFilled[i] && fill >= 1f)
                    {
                        _wasFilled[i] = true;
                        StartCoroutine(DoPopEffect(_filledStars[i].transform));
                    }
                    else if (fill < 1f)
                    {
                        _wasFilled[i] = false;
                    }
                }

                yield return null;
            }
            _hasAnimated = true;
            _starAnimation = null;
        }

        // Coroutine that creates a short scale "pop" animation for a star.
        private IEnumerator DoPopEffect(Transform star)
        {
            Vector3 original = star.localScale;
            Vector3 target = original * 1.3f;
            float duration = 0.15f;

            float t = 0f;
            while (t < duration)
            {
                t += Time.deltaTime;
                star.localScale = Vector3.Lerp(original, target, t / duration);
                yield return null;
            }

            t = 0f;
            while (t < duration)
            {
                t += Time.deltaTime;
                star.localScale = Vector3.Lerp(target, original, t / duration);
                yield return null;
            }

            star.localScale = original;
        }

        // Applies the final fill state to each star based on the provided rating.
        private void SetRating(float rating)
        {
            for (int i = 0; i < _filledStars.Count; i++)
            {
                if (rating >= i + 1)
                {
                    _filledStars[i].fillAmount = 1f;
                }
                else if (rating > i)
                {
                    _filledStars[i].fillAmount = rating - i;
                }
                else
                {
                    _filledStars[i].fillAmount = 0f;
                }
            }
        }

        // Resets the visual state and flags, preparing for a fresh animation.
        public void ResetAnimation()
        {
            _hasAnimated = false;
            SetRating(0f);
        }

        #endregion
    }
}
