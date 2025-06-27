/// <summary>
// Calculates a star rating (0–5) based on listener gain and other podcast performance factors.
/// </summary>

/// <remarks>
/// 24/06/2025 by Damian Dalinger: Initial creation.
/// </remarks>

using UnityEngine;

namespace ProjectCeros
{
    public class StarCalculator : MonoBehaviour
    {
        #region Fields

        [Tooltip("The result of the current podcast calculation.")]
        [SerializeField] private PodcastResult _podcastResult;

        [Tooltip("Number of listeners before the current episode.")]
        [SerializeField] private IntVariable _previousListeners;

        [Tooltip("Size modifier based on current listener count.")]
        [SerializeField] private FloatVariable _sizeMultiplier;

        [Header("Output")]
        [Tooltip("Calculated star rating output (0–5).")]
        [SerializeField] private FloatVariable _starRating;

        private bool _hasCalculated = false;
        private float _listenerScore = 0f;

        #endregion

        #region Lifecycle Methods

        private void OnEnable()
        {
            if (!_hasCalculated)
            {
                CalculateStars();
                _hasCalculated = true;
                DebugLogOutput();
            }
        }

        #endregion

        #region Private Methods

        // Calculates the final star rating from different weighted components.
        private void CalculateStars()
        {
            _listenerScore = CalculateListenerScore();

            // TODO: Add other components like guest/sponsor in future
            float guestScore = 0f;
            float sponsorScore = 0f;

            float totalScore = _listenerScore + guestScore + sponsorScore;
            float clampedStars = Mathf.Clamp(totalScore, 0f, 5f);

            _starRating.RuntimeValue = clampedStars;
        }

        // Calculates the listener-related contribution to the star rating.
        private float CalculateListenerScore()
        {
            int previous = Mathf.Max(1, _previousListeners.RuntimeValue);
            float size = _sizeMultiplier.RuntimeValue;
            int gain = _podcastResult.Gain;

            float ratio = (gain / size) / previous;
            float scaled = 6f * ratio;

            return Mathf.Clamp(scaled, 0f, 3f);
        }

        // Prints detailed star rating calculation info to the console.
        private void DebugLogOutput()
        {
            Debug.Log(
                "--- Star Calculation Debug ---\n" +
                $"PreviousListeners: {_previousListeners.RuntimeValue}\n" +
                $"SizeMod: {_sizeMultiplier.RuntimeValue:F3}\n" +
                $"ListenerGain: {_podcastResult.Gain}\n" +
                $"ListenerScore (0–3): {_listenerScore:F2}\n" +
                $"Final Star Rating: {_starRating.RuntimeValue:F2}"
            );
        }

        #endregion
    }
}
