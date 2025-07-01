using UnityEngine;

namespace ProjectCeros
{
    [CreateAssetMenu(menuName = "Other/RivalPodcaster")]
    public class RivalPodcaster : ScriptableObject
    {
        public int CurrentListeners;

        [Header("Identification")]
        public Sprite Portrait;
        public string DisplayName;
        [TextArea] public string Bio;

        [Header("Genres")]
        public int[] LikedGenres;
        public int[] DislikedGenres;



        [HideInInspector]
        public int PreviousListeners;
    }
}