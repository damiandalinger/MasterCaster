using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using ProjectCeros;

namespace ProjectCeros
{
    public class LeaderboardManager : MonoBehaviour
    {
        [Header("Rival Podcasters")]
        public List<RivalPodcaster> rivalPodcasters;

        [Header("Player Data")]
        public IntVariable playerListeners;
        public string playerName;
        public Sprite playerPortrait;

        [Header("News Data")]
        public ArticleDatabase activeArticles; // <- Das ist jetzt dein RuntimeSet

        private const float INCREASE_MULTIPLIER = 1.02f;
        private const float DECREASE_MULTIPLIER = 0.98f;

        public struct LeaderboardEntry
        {
            public string name;
            public Sprite portrait;
            public int listeners;
            public bool isPlayer;
        }

        public List<LeaderboardEntry> CurrentLeaderboard { get; private set; } = new();
        void Start()
        {
            
        }
        public void UpdateLeaderboard()
        {
            var articles = activeArticles.Items; // Zugriff auf RuntimeSet-Inhalte

            // 1. Rival Listener-Berechnung
            foreach (var rival in rivalPodcasters)
            {
                rival.PreviousListeners = rival.CurrentListeners;

                bool hasPositive = rival.LikedGenres.Any(g =>
                    articles.Any(a => a.Subgenre == g && a.ValuePositive > 0));

                bool hasNegative = rival.DislikedGenres.Any(g =>
                    articles.Any(a => a.Subgenre == g && a.ValueNegative > 0));

                float multiplier = 1f;
                if (hasPositive && !hasNegative) multiplier = INCREASE_MULTIPLIER;
                else if (!hasPositive && hasNegative) multiplier = DECREASE_MULTIPLIER;

                rival.CurrentListeners = Mathf.RoundToInt(rival.CurrentListeners * multiplier);
            }

            // 2. Liste für Sortierung zusammenstellen
            var allEntries = new List<LeaderboardEntry>();

            // Spieler einfügen
            allEntries.Add(new LeaderboardEntry
            {
                name = playerName,
                portrait = playerPortrait,
                listeners = playerListeners.RuntimeValue,
                isPlayer = true
            });

            // Rivalen einfügen
            foreach (var rival in rivalPodcasters)
            {
                allEntries.Add(new LeaderboardEntry
                {
                    name = rival.DisplayName,
                    portrait = rival.Portrait,
                    listeners = rival.CurrentListeners,
                    isPlayer = false
                });
            }

            // 3. Sortieren
            CurrentLeaderboard = allEntries
                .OrderByDescending(e => e.listeners)
                .ToList();

            // 4. Spieler-Rang debuggen
            int playerRank = CurrentLeaderboard.FindIndex(e => e.isPlayer) + 1;
            Debug.Log("Neuer Spieler-Rang: " + playerRank);
        }

        [ContextMenu("Debug Leaderboard Order")]
        public void DebugLeaderboardOrder()
        {
            if (CurrentLeaderboard == null || CurrentLeaderboard.Count == 0)
            {
                Debug.LogWarning("Leaderboard is empty. Updating first...");
                UpdateLeaderboard();
            }

            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.AppendLine("==== Current Leaderboard ====");

            for (int i = 0; i < CurrentLeaderboard.Count; i++)
            {
                var entry = CurrentLeaderboard[i];
                string marker = entry.isPlayer ? "[PLAYER] " : "";
                sb.AppendLine($"{i + 1}. {marker}{entry.name} – {entry.listeners:N0} listeners");
            }

            sb.AppendLine("=============================");

            Debug.Log(sb.ToString());
        }
    }
}
