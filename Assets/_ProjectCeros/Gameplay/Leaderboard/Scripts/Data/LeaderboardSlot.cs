/// <summary>
/// Represents a single slot in the leaderboard UI, displaying name and portrait of a podcaster.
/// </summary>

/// <remarks>
/// 02/07/2025 by Damian Dalinger: Script Creation.
/// </remarks>

using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ProjectCeros
{

    [System.Serializable]
    public class LeaderboardSlot
    {
        [Tooltip("Parent object of the leaderboard slot.")]
        public GameObject Slot;

        [Tooltip("Text element showing the podcaster's name.")]
        public TMP_Text Name;

        [Tooltip("Image element showing the podcaster's portrait.")]
        public Image Portrait;
    }
}