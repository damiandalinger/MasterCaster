using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace ProjectCeros
{

    public class StoryProgressTracker
    {
        private class StoryState
        {
            public int CurrentPart = 0;
            public int EligibleSince = 0;
            public bool StoryComplete = false;
        }

        private Dictionary<int, StoryState> storyStates = new();

        public void MarkStoryPartPlayed(int storyID, int part)
        {
            if (!storyStates.ContainsKey(storyID))
                storyStates[storyID] = new StoryState();

            var state = storyStates[storyID];
            state.CurrentPart = part;
            state.EligibleSince = 0;

            // Beispiel-Ende bei Part 3 – du kannst das auch dynamisch machen
            if (part >= 3)
                state.StoryComplete = true;
        }

        public void TickGenreAppearance(string genre, List<Article> articles)
        {
            // Gruppiere nach StoryID, damit du EligibleSince nur 1x pro Story hochzählst
            var grouped = articles
                .Where(a => a.StoryID != 0)
                .GroupBy(a => a.StoryID);

            foreach (var group in grouped)
            {
                int storyID = group.Key;
                if (!storyStates.TryGetValue(storyID, out var state)) continue;

                bool hasNextPart = group.Any(a => a.StoryPart == state.CurrentPart + 1);

                if (hasNextPart)
                {
                    state.EligibleSince++;
                    if (Debug.isDebugBuild)
                        Debug.Log($"[StoryTracker] Story {storyID} seen again, EligibleSince = {state.EligibleSince}");
                }
            }
        }

        public bool ShouldIncludeArticle(Article article, float minChance, float maxChance)
        {
            if (article.StoryID == 0 || article.StoryPart <= 1) return true;

            if (!storyStates.TryGetValue(article.StoryID, out var state))
            {
                Debug.Log($"[StoryTracker] Rejecting StoryID {article.StoryID} → No state found.");
                return false;
            }

            if (article.StoryPart != state.CurrentPart + 1)
            {
                Debug.Log($"[StoryTracker] Rejecting StoryID {article.StoryID} → Wrong part: {article.StoryPart}, expected: {state.CurrentPart + 1}");
                return false;
            }

            if (state.EligibleSince <= 0)
            {
                Debug.Log($"[StoryTracker] Rejecting StoryID {article.StoryID} → EligibleSince = 0");
                return false;
            }

            if (state.EligibleSince >= 2)
            {
                Debug.Log($"[StoryTracker] Accepting StoryID {article.StoryID} → Guaranteed");
                return true;
            }

            float chance = Mathf.Lerp(minChance, maxChance, state.EligibleSince / 2f);
            bool result = Random.value < (chance / 100f);

            Debug.Log($"[StoryTracker] StoryID {article.StoryID} → EligibleSince: {state.EligibleSince}, Chance: {chance}%, Result: {result}");

            return result;
        }

        public bool IsBlockedByRunningStory(Article article)
        {
            if (article.StoryID == 0) return false;

            if (!storyStates.TryGetValue(article.StoryID, out var state))
                return false;

            // Story läuft noch → alle Teile (auch Part 1) blockieren
            if (!state.StoryComplete)
                return true;

            return false;
        }

        public void DebugStoryStates()
        {
            foreach (var kvp in storyStates)
            {
                int storyID = kvp.Key;
                var state = kvp.Value;

                Debug.Log($"[StoryTracker] StoryID {storyID} → CurrentPart: {state.CurrentPart}, EligibleSince: {state.EligibleSince}, Complete: {state.StoryComplete}");
            }
        }
    }
}
