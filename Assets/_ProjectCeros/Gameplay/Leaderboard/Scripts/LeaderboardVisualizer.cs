using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;

namespace ProjectCeros
{
    [System.Serializable]
    public class LeaderboardSlot
    {
        public GameObject slotObject;
        public TMP_Text nameText;
        public Image portraitImage;
    }

    public class LeaderboardVisualizer : MonoBehaviour
    {
        [Header("Podcasters (Includes Player)")]
        public RivalPodcaster[] allPodcasters;

        [Header("UI Slots (top to bottom)")]
        public LeaderboardSlot[] slots;

        [Header("Background Sprites")]
        public Sprite goldBackground;
        public Sprite silverBackground;
        public Sprite bronzeBackground;
        public Sprite standardBackground;

        [Header("Rise/Fall Icons")]
        public Sprite iconUp;
        public Sprite iconDown;
        public Sprite iconSame;

        [Header("Detail View")]
        public GameObject detailPanel;
        public Image detailRiseFallIcon;
        public Image detailBackgroundImage;
        public TMP_Text detailName;
        public TMP_Text detailPersonName;
        public TMP_Text detailListenerCount;
        public TMP_Text detailRank;
        public Image detailPortrait;
        public TMP_Text detailDescription;
        public TMP_Text detailLiked1;
        public TMP_Text detailLiked2;
        public TMP_Text detailDisliked1;
        public TMP_Text detailDisliked2;

        private RivalPodcaster[] sorted;

        private void OnEnable()
        {
            UpdateUI();
        }

        public void UpdateUI()
        {
            sorted = allPodcasters.OrderBy(p => p.CurrentRank).ToArray();

            for (int i = 0; i < slots.Length; i++)
            {
                var slot = slots[i];
                if (slot.slotObject == null || slot.nameText == null || slot.portraitImage == null)
                    continue;

                if (i < sorted.Length)
                {
                    var rival = sorted[i];

                    slot.nameText.text = rival.DisplayName;
                    slot.portraitImage.sprite = rival.Portrait;
            
                }
                else
                {
                    slot.nameText.text = "---";
                    slot.portraitImage.sprite = null;
                }
            }
        }

        public void ShowDetail(int index)
        {
            if (index < 0 || index >= sorted.Length)
                return;

            var rival = sorted[index]; 

            detailPanel.SetActive(true);
            detailName.text = rival.DisplayName;
            detailPersonName.text = rival.PersonName;
            detailListenerCount.text = $"{rival.CurrentListener:N0} listeners";
            detailRank.text = $"#{rival.CurrentRank}";
            detailPortrait.sprite = rival.Portrait;
            detailDescription.text = rival.Bio;

            detailRiseFallIcon.sprite = rival.RankChange switch
            {
                1 => iconUp,
                2 => iconSame,
                3 => iconDown,
                _ => iconSame
            };

            detailBackgroundImage.sprite = rival.CurrentRank switch
            {
                1 => goldBackground,
                2 => silverBackground,
                3 => bronzeBackground,
                _ => standardBackground
            };

            detailLiked1.text = rival.LikedGenres.Length > 0 ? GetSubgenreDisplayName(rival.LikedGenres[0]) : "-";
            detailLiked2.text = rival.LikedGenres.Length > 1 ? GetSubgenreDisplayName(rival.LikedGenres[1]) : "-";
            detailDisliked1.text = rival.DislikedGenres.Length > 0 ? GetSubgenreDisplayName(rival.DislikedGenres[0]) : "-";
            detailDisliked2.text = rival.DislikedGenres.Length > 1 ? GetSubgenreDisplayName(rival.DislikedGenres[1]) : "-";
        }

        public void HideDetail()
        {
            detailPanel.SetActive(false);
        }

        private string GetSubgenreDisplayName(int subgenreId)
        {
            return subgenreId switch
            {
                1 => "FPS",
                2 => "Hero Shooter",
                3 => "Loot Shooter",
                4 => "Fighting Game",
                5 => "Stealth Game",
                6 => "Hack & Slash",
                7 => "Souls Like",
                8 => "Open World",
                9 => "MMORPG",
                10 => "RTS",
                11 => "Grand Strategy",
                12 => "TBS",
                13 => "Sport",
                14 => "Living Simulation",
                15 => "Job Simulation",
                16 => "Farming Game",
                17 => "Side Scroller",
                18 => "Roguelike",
                _ => "Unknown"
            };
        }
    }
}
