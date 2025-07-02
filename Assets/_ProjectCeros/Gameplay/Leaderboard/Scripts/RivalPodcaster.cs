using UnityEngine;

namespace ProjectCeros
{
    [CreateAssetMenu(menuName = "Other/RivalPodcaster")]
    public class RivalPodcaster : ScriptableObject
    {
        public int InitialListener;
        public int InitialRank;

        [Header("Identification")]
        public Sprite Portrait;
        public string DisplayName;
        public string PersonName;
        [TextArea] public string Bio;

        [Header("Genres")]
        public int[] LikedGenres;
        public int[] DislikedGenres;

        public int CurrentRank;
        public int PreviousRank;
        public int CurrentListener;
        public int RankChange;

        // Sets all runtime values to their initial states (for game reset or session start).
        public void InitializeRuntime()
        {
            CurrentListener = InitialListener;
            PreviousRank = InitialRank;
            CurrentRank = 0;
            RankChange = 0;
        }
    }
}