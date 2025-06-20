using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using ProjectCeros;

namespace ProjectCeros
{
    public class StarRatingVisualizer : MonoBehaviour
    {
        [Header("Star Fill Images (type = Filled, horizontal)")]
        [SerializeField] private List<Image> _filledStars;

        [Header("Rating Source")]
        [SerializeField] private FloatReference _targetRating;

        [Header("Animation Settings")]
        [SerializeField] private float _animationDuration = 1.5f;
        [SerializeField] private float _delayBeforeStart = 0.5f;
        [SerializeField] private AnimationCurve _fillCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);
        private Coroutine _animationCoroutine;
        private bool _hasAnimated = false;

        private List<bool> _wasFilled = new List<bool>();

        private void Awake()
        {
            // Init bei Start (alle false)
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
                _animationCoroutine = StartCoroutine(AnimateStarsCoroutine());
            }
        }

        private void OnDisable()
        {
            SkipAnimation(); // Wichtig: Animation abbrechen, wenn man wegklickt
        }

        private IEnumerator AnimateStarsCoroutine()
        {
            yield return new WaitForSeconds(_delayBeforeStart);

            float elapsed = 0f;
            float currentRating = 0f;

            while (elapsed < _animationDuration)
            {
                elapsed += Time.deltaTime;
                float t = Mathf.Clamp01(elapsed / _animationDuration);
                float eased = _fillCurve.Evaluate(t);
                currentRating = Mathf.Lerp(0f, _targetRating, eased);

                for (int i = 0; i < _filledStars.Count; i++)
                {
                    float fill = Mathf.Clamp01(currentRating - i);
                    _filledStars[i].fillAmount = fill;

                    // Check für Pop, wenn Stern JETZT voll ist und es vorher nicht war
                    if (!_wasFilled[i] && fill >= 1f)
                    {
                        _wasFilled[i] = true;
                        StartCoroutine(DoPopEffect(_filledStars[i].transform));
                    }
                    else if (fill < 1f)
                    {
                        _wasFilled[i] = false; // falls Animation neu startet
                    }
                }

                yield return null;
            }
            _hasAnimated = true;
            _animationCoroutine = null;
        }

        private IEnumerator DoPopEffect(Transform star)
        {
            Vector3 original = star.localScale;
            Vector3 target = original * 1.3f;
            float t = 0f;
            float duration = 0.15f;

            while (t < duration)
            {
                t += Time.deltaTime;
                float p = t / duration;
                star.localScale = Vector3.Lerp(original, target, p);
                yield return null;
            }

            t = 0f;
            while (t < duration)
            {
                t += Time.deltaTime;
                float p = t / duration;
                star.localScale = Vector3.Lerp(target, original, p);
                yield return null;
            }

            star.localScale = original;
        }

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

        public void SkipAnimation()
        {
            if (_animationCoroutine != null)
            {
                StopCoroutine(_animationCoroutine);
                _animationCoroutine = null;
            }

            SetRating(_targetRating.Value);
            _hasAnimated = true;
        }

        public void ResetAnimation()
        {
            _hasAnimated = false;
            SetRating(0f);
        }
    }
}
