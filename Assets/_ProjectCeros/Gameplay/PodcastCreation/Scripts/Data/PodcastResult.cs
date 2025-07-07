using UnityEngine;

namespace ProjectCeros
{
    [CreateAssetMenu(menuName = "Other/Podcast Result")]
    public class PodcastResult : ScriptableObject
    {
        [Header("Calculated Listener Stats")]
        public int TotalListeners;
        public int Gain;

        [Header("Bonus Breakdown")]
        public float BaseBonus;
        public float GuestBonus;
        public float EquipmentBonus;
        public float SponsorBonus;
        public float DarkWebBonus;
        public float SubgenreBonus;

        [Header("Multipliers")]
        public float BonusMultiplier;
        public float TopicMultiplier;

        public void OverwriteWith(
            int totalListeners,
            int gain,
            float baseBonus,
            float guest,
            float equip,
            float sponsor,
            float dark,
            float sub,
            float bonusMult,
            float topicMult
        )
        {
            TotalListeners = totalListeners;
            Gain = gain;
            BaseBonus = baseBonus;
            GuestBonus = guest;
            EquipmentBonus = equip;
            SponsorBonus = sponsor;
            DarkWebBonus = dark;
            SubgenreBonus = sub;
            BonusMultiplier = bonusMult;
            TopicMultiplier = topicMult;
        }
    }
}