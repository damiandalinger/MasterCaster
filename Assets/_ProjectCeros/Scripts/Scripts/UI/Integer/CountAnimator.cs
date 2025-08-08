/// <summary>
/// Utility class for animating number counters in TMP_Text fields.
/// Supports both count-up and count-down using a unified method.
/// </summary>

/// <remarks>
/// 27/06/2025 by Damian Dalinger: Script creation.
/// </remarks>

using System.Collections;
using TMPro;
using UnityEngine;
using System.Globalization;

namespace ProjectCeros
{
    public static class CountAnimator
    {
        // Animates a TMP_Text from one integer to another.
        // Automatically handles counting up or down.
        public static IEnumerator Count(TMP_Text target, int from, int to, float duration, bool showSign = false)
        {
            float elapsed = 0f;
            while (elapsed < duration)
            {
                elapsed += Time.deltaTime;
                float t = Mathf.Clamp01(elapsed / duration);
                float eased = t * t * (3f - 2f * t); // Smoothstep easing
                int value = Mathf.RoundToInt(Mathf.Lerp(from, to, eased));

                string sign = showSign && value > 0 ? "+" : "";
                target.text = sign + value.ToString("N0", CultureInfo.InvariantCulture);
                yield return null;
            }

            string finalSign = showSign && to > 0 ? "+" : "";
            target.text = finalSign + to.ToString("N0", CultureInfo.InvariantCulture);
        }
    }
}
