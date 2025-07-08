/// <summary>
/// Represents a podcaster within the game world.
/// Used for the leaderboard.
/// </summary>

/// <remarks>
/// 02/07/2025 by Damian Dalinger: Script Creation.
/// </remarks>

using UnityEngine;

namespace ProjectCeros
{
    [CreateAssetMenu(menuName = "Other/Leaderboard/Podcaster")]
    public class Podcaster : ScriptableObject
    {
        #region Fields

        [Header("Initial Values")]
        [Tooltip("The starting number of listeners this podcaster has.")]
        public int InitialListener;

        [Tooltip("The starting rank of this podcaster.")]
        public int InitialRank;

        [Header("Identification")]
        [Tooltip("The portrait shown in UI.")]
        public Sprite Portrait;

        [Tooltip("Name of the podcast.")]
        public string DisplayName;

        [Tooltip("Name of the person.")]
        public string PersonName;

        [TextArea, Tooltip("Short description text shown in UI.")]
        public string Description;

        [Header("Genres")]
        [Tooltip("List of genre IDs this podcaster prefers.")]
        public int[] LikedGenres;

        [Tooltip("List of genre IDs this podcaster dislikes.")]
        public int[] DislikedGenres;

        public int CurrentRank;
        public int PreviousRank;
        public int CurrentListener;
        public int RankChange;

        #endregion

        #region Public Methods

        // Resets runtime values to initial state. Called on new game start or game reset.
        public void InitializeRuntime()
        {
            CurrentListener = InitialListener;
            PreviousRank = InitialRank;
            CurrentRank = 0;
            RankChange = 0;
        }

        #endregion
    }
}