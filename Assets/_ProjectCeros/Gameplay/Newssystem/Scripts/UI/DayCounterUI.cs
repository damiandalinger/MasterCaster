/// <summary>
/// Displays the current day number on the in-game newspaper UI.
/// </summary>

/// <remarks>
/// 09/05/2025 by Damian Dalinger: Script creation.
/// </remarks>

using UnityEngine;
using TMPro;

namespace ProjectCeros
{

    public class DayCounterUI : MonoBehaviour
    {
        [SerializeField, Tooltip("Reference to the IntVariable representing the current in-game day.")]
        private IntReference currentDay;

        [SerializeField, Tooltip("Text element where the current day number will be displayed.")]
        private TextMeshProUGUI dayText;

        // Updates the text field to show the current day.
        public void UpdateDayCounter()
        {
            dayText.text = $"Day {currentDay.Value}";
        }
    }
}
