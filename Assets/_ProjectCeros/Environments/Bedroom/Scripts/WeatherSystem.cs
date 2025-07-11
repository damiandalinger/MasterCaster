/// <summary>
/// Determines weather conditions on day change and updates visual + logical state.
/// </summary>

/// <remarks>
/// 10/07/2025 by Damian Dalinger: Initial implementation.
/// </remarks>

using UnityEngine;

namespace ProjectCeros
{
    public class WeatherSystem : MonoBehaviour
    {
        [Header("Config")]
        [Tooltip("Chance of rain (0 to 1).")]
        [SerializeField, Range(0f, 1f)] private float _rainChance;

        [Tooltip("Bool variable updated based on whether it's raining.")]
        [SerializeField] private BoolVariable _isRaining;

        [Header("Visuals")]
        [Tooltip("Sprite shown when it's raining.")]
        [SerializeField] private GameObject _rainVisual;

        // Call this at the end of the day to determine the weather.
        public void DetermineWeather()
        {
            bool isRain = Random.value < _rainChance;

            _isRaining.RuntimeValue = isRain;

            if (_rainVisual != null)
                _rainVisual.SetActive(isRain);
        }
    }
}
