/// <summary>
// Calculates a star rating (0–5) based on listener gain and other podcast performance factors.
/// </summary>

/// <remarks>
/// 24/06/2025 by Damian Dalinger: Initial creation.
/// 10/07/2025 by Damian Dalinger: Added the guestscore.
/// </remarks>

using System.Collections.Generic;
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

        [Header("Guest Scoring")]
        [Tooltip("The host's own star rating used for guest comparison.")]
        [SerializeField] private IntVariable _globalStarRating;

        [Tooltip("All guests that participated in the episode.")]
        [SerializeField] private List<GuestSO> _guests;

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

        // Checks if a guest is active and calculates the score there is.
        private float CalculateGuestScore()
        {
            int hostRating = _globalStarRating.RuntimeValue;
            int guestTotal = 0;
            int guestCount = 0;

            foreach (var guest in _guests)
            {
                if (guest == null || !guest.hasAccepted)
                    continue;

                guestTotal += guest.Rating;
                guestCount++;
            }

            if (guestCount == 0)
                return 0f;

            float guestAverage = (float)guestTotal / guestCount;

            if (guestAverage < hostRating)
                return 0.5f;
            if (Mathf.Approximately(guestAverage, hostRating))
                return 1.0f;

            return 1.5f;
        }

        // Calculates the final star rating from different weighted components.
        private void CalculateStars()
        {
            _listenerScore = CalculateListenerScore();
            float guestScore = CalculateGuestScore();
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

            return Mathf.Clamp(scaled, 0f, 4f);
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
