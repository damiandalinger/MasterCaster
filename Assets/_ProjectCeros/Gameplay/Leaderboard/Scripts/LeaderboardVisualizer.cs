using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;

namespace ProjectCeros
{
    public class LeaderboardVisualizer : MonoBehaviour
    {
        [Header("Manager Reference")]
        public LeaderboardManager leaderboardManager;

        [Header("UI Slots (from top to bottom)")]
        public GameObject[] entrySlots; // Nur die GameObjects (z. B. UI Panels)
        private void OnEnable()
        {
            leaderboardManager.UpdateLeaderboard();
            UpdateUI();
        }
        public void UpdateUI()
        {
            var entries = leaderboardManager.CurrentLeaderboard;

            for (int i = 0; i < entrySlots.Length; i++)
            {
                GameObject slot = entrySlots[i];

                // Find components
                Image portraitImage = slot.GetComponentsInChildren<Image>()
    .FirstOrDefault(img => img.gameObject != slot);

                TMP_Text nameText = slot.GetComponentsInChildren<TMP_Text>()
                    .FirstOrDefault(txt => txt.gameObject != slot);

                // Validate
                if (portraitImage == null || nameText == null)
                {
                    Debug.LogWarning($"Slot {i + 1} is missing Image or TMP_Text component.");
                    continue;
                }

                if (i < entries.Count)
                {
                    var entry = entries[i];
                    portraitImage.sprite = entry.portrait;
                    nameText.text = entry.isPlayer ? $"[YOU] {entry.name}" : entry.name;
                }
                else
                {
                    portraitImage.sprite = null;
                    nameText.text = "---";
                }
            }
        }
    }
}
