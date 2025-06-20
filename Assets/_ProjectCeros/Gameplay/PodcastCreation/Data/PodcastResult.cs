using UnityEngine;

namespace ProjectCeros
{
    [CreateAssetMenu(menuName = "ProjectCeros/Podcast Result")]
    public class PodcastResult : ScriptableObject
    {
        [Header("Calculated Listener Stats")]
        public int TotalListeners;
        public int Gain;
        public int GainAfterBonus;

        [Header("Bonus Breakdown")]
        public float GuestBonus;
        public float EquipmentBonus;
        public float SponsorBonus;
        public float DarkWebBonus;
        public float SubgenreBonus;
        public float OtherBonus;

        [Header("Multipliers")]
        public float BonusMultiplier;
        public float TopicMultiplier;

        public void OverwriteWith(
            int totalListeners,
            int gain,
            int gainAfterBonus,
            float guest,
            float equip,
            float sponsor,
            float dark,
            float sub,
            float other,
            float bonusMult,
            float topicMult
        )
        {
            TotalListeners = totalListeners;
            Gain = gain;
            GainAfterBonus = gainAfterBonus;
            GuestBonus = guest;
            EquipmentBonus = equip;
            SponsorBonus = sponsor;
            DarkWebBonus = dark;
            SubgenreBonus = sub;
            OtherBonus = other;
            BonusMultiplier = bonusMult;
            TopicMultiplier = topicMult;
        }
    }
}