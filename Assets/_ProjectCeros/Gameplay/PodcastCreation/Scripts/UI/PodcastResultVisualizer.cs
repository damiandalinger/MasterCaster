/// <summary>
/// Visualizes podcast result data (listener gain and total) with animated UI counters and multipliers.
/// Supports tab interruption via ITabInterruptible.
/// </summary>

/// <remarks>
/// 23/06/2025 by Damian Dalinger: Script Creation.
/// 27/06/2025 by Damian Dalinger: Implemented ITabInterruptible.
/// </remarks>

using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace ProjectCeros
{
    public class PodcastResultVisualizer : MonoBehaviour, ITabInterruptible
    {
        #region Fields

        public bool IsBusy => _multiplierAnimation != null;

        [Header("UI References")]
        [Tooltip("Text field showing listener gain (e.g., +1,200).")]
        [SerializeField] private TMP_Text _gainText;

        [Tooltip("Text field showing total listener count.")]
        [SerializeField] private TMP_Text _totalText;

        [Tooltip("Container row for total listener info.")]
        [SerializeField] private GameObject _totalRow;

        [Tooltip("UI rows for displaying individual multipliers.")]
        [SerializeField] private List<MultiplierRowUI> _multiplierRows;

        [Tooltip("Icons to associate with multiplier rows (ordered).")]
        [SerializeField] private List<Sprite> _iconSprites;

        [Header("Animation Durations")]
        [Tooltip("Duration used for count-up animations.")]
        [SerializeField] private FloatReference _countDuration;

        [Tooltip("Delay between each step of the multiplier reveal.")]
        [SerializeField] private FloatReference _delay;

        [Header("Data")]
        [Tooltip("The data source containing all calculated podcast result values.")]
        [SerializeField] private PodcastResult _result;

        private Coroutine _multiplierAnimation;
        private bool _hasAnimated = false;

        #endregion

        #region Lifecycle Methods

        private void OnEnable()
        {
            if (_hasAnimated)
            {
                CompleteImmediately(_result);
            }
            else
            {
                ResetUI();
                _multiplierAnimation = StartCoroutine(AnimateResult(_result));
            }
        }

        private void OnDisable()
        {
            SkipToEnd();
        }

        #endregion

        #region Public Methods

        // Immediately skips the result animation and shows final state.
        public void SkipToEnd()
        {
            if (_multiplierAnimation != null)
            {
                StopCoroutine(_multiplierAnimation);
                CompleteImmediately(_result);
                _multiplierAnimation = null;
            }
        }

        #endregion

        #region Private Methods

        // Coroutine that progressively displays multipliers and count-up animations.
        private IEnumerator AnimateResult(PodcastResult result)
        {
            yield return new WaitForSeconds(_delay);

            int rowIndex = 0;
            if (result.IsGuestEpisode)
                SetSimpleRow(rowIndex++, "Guest Episode", $"x{result.TopicMultiplier:F2}");
            else
                SetSimpleRow(rowIndex++, "Topic Multiplier", $"x{result.TopicMultiplier:F2}");
            yield return new WaitForSeconds(_delay);

            SetSimpleRow(rowIndex++, "Bonus Multiplier", $"x{result.BonusMultiplier:F2}");
            yield return new WaitForSeconds(_delay);

            foreach (var (label, value, iconIndex) in GetResultModifiers(result))
            {
                if (Mathf.Abs(value) < 0.001f)
                    continue;

                if (rowIndex >= _multiplierRows.Count)
                    break;

                SetFormattedRow(rowIndex, label, value, iconIndex);
                rowIndex++;
                yield return new WaitForSeconds(_delay);
            }

            yield return CountAnimator.Count(_gainText, 0, result.Gain, _countDuration, showSign: true);
            yield return new WaitForSeconds(_delay * 2);

            _totalRow.SetActive(true);
            int previous = result.TotalListeners - result.Gain;
            yield return CountAnimator.Count(_totalText, previous, result.TotalListeners, _countDuration);

            _hasAnimated = true;
            _multiplierAnimation = null;
        }

        // Directly applies the final visual state without animation.
        private void CompleteImmediately(PodcastResult result)
        {
            _gainText.text = "+" + result.Gain.ToString("N0");
            _totalText.text = result.TotalListeners.ToString("N0");
            _totalRow.SetActive(true);

            foreach (var row in _multiplierRows)
                row.RootObject.SetActive(false);

            int rowIndex = 0;
            if (result.IsGuestEpisode)
                SetSimpleRow(rowIndex++, "Guest Episode", $"x{result.TopicMultiplier:F2}");
            else
                SetSimpleRow(rowIndex++, "Topic Multiplier", $"x{result.TopicMultiplier:F2}");
            SetSimpleRow(rowIndex++, "Bonus Multiplier", $"x{result.BonusMultiplier:F2}");

            foreach (var (label, value, iconIndex) in GetResultModifiers(result))
            {
                if (Mathf.Abs(value) < 0.001f)
                    continue;

                if (rowIndex >= _multiplierRows.Count)
                    break;

                SetFormattedRow(rowIndex, label, value, iconIndex);
                rowIndex++;
            }

            _hasAnimated = true;
        }

        // Returns a list of labeled bonus values with icon indices for UI display.
        private List<(string label, float value, int iconIndex)> GetResultModifiers(PodcastResult result) => new()
        {
            ("Base Value", result.BaseBonus, 0),
            ("Guest Bonus", result.GuestBonus, 1),
            ("Equipment Bonus", result.EquipmentBonus, 2),
            ("Sponsor Bonus", result.SponsorBonus, 3),
            ("Dark Web Bonus", result.DarkWebBonus, 4),
            ("Subgenre Bonus", result.SubgenreBonus, 5),
        };

        // Populates a basic row with label and value.
        private void SetSimpleRow(int index, string name, string value)
        {
            if (index >= _multiplierRows.Count) return;

            var row = _multiplierRows[index];
            row.RootObject.SetActive(true);
            row.TextName.text = name;
            row.TextValue.text = value;
        }

        // Populates a formatted row with label, numeric bonus, and optional icon.
        private void SetFormattedRow(int index, string label, float value, int iconIndex)
        {
            if (index >= _multiplierRows.Count || Mathf.Abs(value) < 0.001f) return;

            var row = _multiplierRows[index];
            row.RootObject.SetActive(true);
            row.TextName.text = label;
            row.TextValue.text = $"+{value:F2}";

            if (iconIndex >= 0 && iconIndex < _iconSprites.Count)
                row.Icon.sprite = _iconSprites[iconIndex];
        }

        // Resets all result UI elements to their initial, empty state.
        private void ResetUI()
        {
            _gainText.text = "+0";
            _totalRow.SetActive(false);
            foreach (var row in _multiplierRows)
                row.RootObject.SetActive(false);
        }

        #endregion
    }
}