using UnityEngine;

namespace ProjectCeros
{
    public class StarCalculator : MonoBehaviour
    {
        [Header("Listener Factors")]
        [SerializeField] private PodcastResult _podcastResult;
        [SerializeField] private IntVariable _previousListeners;
        [SerializeField] private FloatVariable _sizeMod;

        [Header("Output")]
        [SerializeField] private FloatVariable _starRating; // 0–5 stars total
        private bool hasCalculated = false;
        private float Score;

        private void OnEnable()
        {
            if (!hasCalculated)
            {
                CalculateStars();
                hasCalculated = true;
                DebugLog();
            }
        }

        public void CalculateStars()
        {
            float listenerScore = CalculateListenerPart();
            Score = listenerScore;
            // TODO: Add guest and sponsor parts here later
            float guestScore = 0f;
            float sponsorScore = 0f;

            float totalStars = listenerScore + guestScore + sponsorScore;
            totalStars = Mathf.Clamp(totalStars, 0f, 5f);

            _starRating.RuntimeValue = totalStars;
        }

        private float CalculateListenerPart()
        {
            int previous = _previousListeners.RuntimeValue;
            float sizeMod = _sizeMod.RuntimeValue;
            int gain = _podcastResult.Gain;

            if (previous <= 0f)
                previous = 1;

            float ratio = (gain / sizeMod) / previous;
            float scaled = 6f * ratio;

            return Mathf.Clamp(scaled, 0f, 3f);
        }

        private void DebugLog()
        {
            Debug.Log(
                "--- Star Calculation Debug ---\n" +
                $"PreviousListeners: {_previousListeners.RuntimeValue})\n" +
                $"SizeMod: {_sizeMod.RuntimeValue}\n" +
                $"NewListeners: {_podcastResult.Gain}\n" +
                $"ListenerScore: {Score}"
            );
        }
    }
}
