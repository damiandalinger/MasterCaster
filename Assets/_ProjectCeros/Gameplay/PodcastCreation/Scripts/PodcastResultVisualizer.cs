using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using System.Globalization;
using UnityEngine.Events;

namespace ProjectCeros
{
    [System.Serializable]
    public class MultiplierRowUI
    {
        public GameObject RootObject;
        public TMP_Text TextName;
        public TMP_Text TextValue;
        public UnityEngine.UI.Image Icon;
    }

    public class PodcastResultVisualizer : MonoBehaviour
    {
        [Header("UI References")]
        [SerializeField] private TMP_Text _gainText;
        [SerializeField] private TMP_Text _totalText;
        [SerializeField] private GameObject _totalRow;
        [SerializeField] private List<MultiplierRowUI> _multiplierRows;
        [SerializeField] private List<Sprite> _iconSprites;

        [Header("Animation Durations")]
        [SerializeField] private float _gainCountDuration = 2f;
        [SerializeField] private float _finalCountDuration = 2f;
        [SerializeField] private float _stepDelay = 0.8f;
        [SerializeField] private float _delayBeforeTotal = 1.5f;
        [SerializeField] private float _initialDelay = 1.0f;

        public UnityEvent OnAnimationFinished;

        private Coroutine _activeAnimation;
        private PodcastResult _lastResult;

        public void ShowResult(PodcastResult result)
        {
            if (_activeAnimation != null)
            {
                StopCoroutine(_activeAnimation);
                CompleteImmediately(_lastResult);
            }

            _lastResult = result;
            _activeAnimation = StartCoroutine(AnimateResult(result));
        }

        public void SkipAnimation()
        {
            if (_activeAnimation != null)
            {
                StopCoroutine(_activeAnimation);
                CompleteImmediately(_lastResult);
                _activeAnimation = null;
            }
        }

        private IEnumerator AnimateResult(PodcastResult result)
        {
            _gainText.text = "+0";
            _totalRow.SetActive(false);

            foreach (var row in _multiplierRows)
                row.RootObject.SetActive(false);

            yield return new WaitForSeconds(_initialDelay);

            int currentRow = 0;

            // Topic Multiplier
            SetRow(currentRow++, "Topic Multiplier", $"x{result.TopicMultiplier:F2}");

            yield return new WaitForSeconds(_stepDelay);

            // Bonus Multiplier
            SetRow(currentRow++, "Bonus Multiplier", $"x{result.BonusMultiplier:F2}");

            yield return new WaitForSeconds(_stepDelay);

            // Einzelboni
            var modifiers = new List<(string label, float value, int iconIndex)>
            {
                ("Guest Bonus", result.GuestBonus, 0),
                ("Equipment Bonus", result.EquipmentBonus, 1),
                ("Sponsor Bonus", result.SponsorBonus, 2),
                ("Dark Web Bonus", result.DarkWebBonus, 3),
                ("Subgenre Bonus", result.SubgenreBonus, 4),
                ("Other", result.OtherBonus, 5)
            };

            foreach (var (label, value, iconIndex) in modifiers)
            {
                if (Mathf.Abs(value) < 0.001f || currentRow >= _multiplierRows.Count)
                    continue;

                var row = _multiplierRows[currentRow++];
                row.RootObject.SetActive(true);
                row.TextName.text = label;
                row.TextValue.text = $"+{value:F2}";

                if (iconIndex >= 0 && iconIndex < _iconSprites.Count)
                    row.Icon.sprite = _iconSprites[iconIndex];

                yield return new WaitForSeconds(_stepDelay);
            }

            // Listener Gain Count
            yield return CountUp(_gainText, 0, result.Gain, _gainCountDuration);
            yield return new WaitForSeconds(_delayBeforeTotal);

            // Total Listeners Count
            _totalRow.SetActive(true);
            int previous = result.TotalListeners - result.Gain;
            yield return CountUp(_totalText, previous, result.TotalListeners, _finalCountDuration);

            _activeAnimation = null;
            OnAnimationFinished?.Invoke();
        }

        private void CompleteImmediately(PodcastResult result)
        {
            var culture = new CultureInfo("de-DE");

            _gainText.text = "+" + result.Gain.ToString("N0", culture);
            _totalRow.SetActive(true);
            _totalText.text = result.TotalListeners.ToString("N0", culture);

            foreach (var row in _multiplierRows)
                row.RootObject.SetActive(false);

            int rowIndex = 0;

            SetRow(rowIndex++, "Topic Multiplier", $"x{result.TopicMultiplier:F2}");
            SetRow(rowIndex++, "Bonus Multiplier", $"x{result.BonusMultiplier:F2}");

            var modifiers = new List<(string label, float value, int iconIndex)>
            {
                ("Guest Bonus", result.GuestBonus, 0),
                ("Equipment Bonus", result.EquipmentBonus, 1),
                ("Sponsor Bonus", result.SponsorBonus, 2),
                ("Dark Web Bonus", result.DarkWebBonus, 3),
                ("Subgenre Bonus", result.SubgenreBonus, 4),
                ("Other", result.OtherBonus, 5)
            };

            foreach (var (label, value, iconIndex) in modifiers)
            {
                if (Mathf.Abs(value) < 0.001f || rowIndex >= _multiplierRows.Count)
                    continue;

                var row = _multiplierRows[rowIndex++];
                row.RootObject.SetActive(true);
                row.TextName.text = label;
                row.TextValue.text = $"+{value:F2}";

                if (iconIndex >= 0 && iconIndex < _iconSprites.Count)
                    row.Icon.sprite = _iconSprites[iconIndex];
            }

            _activeAnimation = null;
            OnAnimationFinished?.Invoke();
        }

        private void SetRow(int index, string name, string value)
        {
            if (index >= _multiplierRows.Count)
                return;

            var row = _multiplierRows[index];
            row.RootObject.SetActive(true);
            row.TextName.text = name;
            row.TextValue.text = value;
        }

        private IEnumerator CountUp(TMP_Text textElement, int from, int to, float duration)
        {
            float elapsed = 0f;
            var culture = new CultureInfo("de-DE");

            while (elapsed < duration)
            {
                elapsed += Time.deltaTime;
                float t = Mathf.Clamp01(elapsed / duration);
                float eased = t * t * (3f - 2f * t);
                int value = Mathf.RoundToInt(Mathf.Lerp(from, to, eased));

                string formatted = value.ToString("N0", culture);
                if (textElement == _gainText)
                    formatted = (value >= 0 ? "+" : "") + formatted;

                textElement.text = formatted;
                yield return null;
            }

            string finalFormatted = to.ToString("N0", culture);
            if (textElement == _gainText)
                finalFormatted = (to >= 0 ? "+" : "") + finalFormatted;

            textElement.text = finalFormatted;
        }
    }
}
