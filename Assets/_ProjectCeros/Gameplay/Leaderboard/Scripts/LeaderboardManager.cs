using UnityEngine;
using System.Collections.Generic;
using System.Linq;

namespace ProjectCeros
{
    public class LeaderboardManager : MonoBehaviour
    {
        [Header("Rival Podcasters")]
        public List<RivalPodcaster> rivalPodcasters;

        [Header("Player as Podcaster")]
        public RivalPodcaster playerPodcaster; // Neuer: Spieler als SO

        [Header("Player Listener Data")]
        public IntVariable playerListeners; // Wird in CurrentListener geschrieben

        [Header("News Data")]
        public ArticleDatabase activeArticles;

        private const float INCREASE_MULTIPLIER = 1.02f;
        private const float DECREASE_MULTIPLIER = 0.98f;

        public int PlayerRank { get; private set; }

        public void InitializeAllPodcasters()
        {
            // Initialisiere Rivalen & Spieler
            foreach (var rival in rivalPodcasters)
                rival.InitializeRuntime();

            if (playerPodcaster != null)
            {
                playerPodcaster.InitializeRuntime();
                playerPodcaster.CurrentListener = playerListeners.RuntimeValue;
            }

            PlayerRank = -1;

            Debug.Log("Leaderboard initialized.");
        }

        public void UpdateLeaderboard()
        {
            var articles = activeArticles.Items;

            // 1. Alte Ränge sichern
            var allPodcasters = new List<RivalPodcaster>(rivalPodcasters);
            if (playerPodcaster != null)
                allPodcasters.Add(playerPodcaster);

            foreach (var rival in allPodcasters)
            {
                if (rival.CurrentRank > 0)
                    rival.PreviousRank = rival.CurrentRank;
            }

            // 2. Listenerzahlen für Rivalen aktualisieren
            foreach (var rival in rivalPodcasters)
            {
                bool hasPositive = rival.LikedGenres.Any(g =>
                    articles.Any(a => a.Subgenre == g && a.ValuePositive > 0));
                bool hasNegative = rival.DislikedGenres.Any(g =>
                    articles.Any(a => a.Subgenre == g && a.ValueNegative > 0));

                float multiplier = 1f;
                if (hasPositive && !hasNegative) multiplier = INCREASE_MULTIPLIER;
                else if (!hasPositive && hasNegative) multiplier = DECREASE_MULTIPLIER;

                rival.CurrentListener = Mathf.RoundToInt(rival.CurrentListener * multiplier);
            }

            // 3. Spieler-Listener auf aktuellen Wert setzen
            if (playerPodcaster != null)
            {
                playerPodcaster.CurrentListener = playerListeners.RuntimeValue;
            }

            // 4. Sortieren nach Hörerzahl
            var sorted = allPodcasters
                .OrderByDescending(r => r.CurrentListener)
                .ToList();

            // 5. Ränge zuweisen & RankChange berechnen
            for (int i = 0; i < sorted.Count; i++)
            {
                var rival = sorted[i];
                rival.CurrentRank = i + 1;

                rival.RankChange = rival.PreviousRank <= 0
                    ? 2
                    : rival.PreviousRank > rival.CurrentRank ? 1
                    : rival.PreviousRank < rival.CurrentRank ? 3
                    : 2;

                if (rival == playerPodcaster)
                    PlayerRank = rival.CurrentRank;
            }

            Debug.Log("Neuer Spieler-Rang: " + PlayerRank);
        }
    }
}
