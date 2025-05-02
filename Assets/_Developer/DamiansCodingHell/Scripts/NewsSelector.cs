using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace ProjectCeros
{
    /// <summary>
    /// Handles the selection of important news articles for the newspaper.
    /// Pulls from eligible genre pools and ensures correct agency/story logic.
    /// </summary>
    /// 
    /// <remarks>
    /// 01/05/2025 by Damian Dalinger: Overhaul for genre-based story logic and modular pools.
    /// </remarks>
    public class NewsSelector : MonoBehaviour
    {
        [Header("Eligible Genre Pools")]
        [Tooltip("Databases that hold eligible important articles (by genre).")]
        [SerializeField] private List<ArticleDatabase> eligibleImportantGenrePools;

        [Header("Selection Settings")]
        [Tooltip("How many genres should be selected per newspaper.")]
        [SerializeField] private IntReference genreCount;

        [Tooltip("Enable a possible story continuation in slot 4.")]
        [SerializeField] private bool enableContinuationSlot = true;

        [Tooltip("Enable the optional fifth article slot.")]
        [SerializeField] private bool enableFifthSlot = true;

        [Tooltip("Chance (in %) to add a fifth important article.")]
        [SerializeField] private FloatReference fifthSlotChance;

        [Header("Continuation Chance Settings")]
        [Tooltip("Max % chance for a follow-up story article.")]
        [SerializeField] private FloatReference maxContinuationChance;

        [Tooltip("Min % chance for a follow-up story article.")]
        [SerializeField] private FloatReference minContinuationChance;

        [Header("Debug")]
        [SerializeField] private bool debugLogging = false;

        public List<Article> SelectedImportantArticles { get; private set; } = new();
        private Dictionary<int, List<Article>> pairedByGenre = new();

        public void SelectImportantArticles()
        {
            SelectedImportantArticles.Clear();
            pairedByGenre.Clear();

            // 1. Wähle Genres
            var selectedGenres = eligibleImportantGenrePools
                .Where(db => db.Items.Count >= 2)
                .OrderBy(_ => Random.value)
                .Take(genreCount.Value)
                .ToList();

            // 2. Aus jedem Genre ein Artikelpaar ziehen (eine Agentur)
            foreach (var genre in selectedGenres)
            {
                var pairs = genre.Items
                    .GroupBy(a => a.PairID)
                    .Where(g => g.Count() == 2)
                    .Select(g => g.ToList())
                    .ToList();

                if (pairs.Count == 0) continue;

                var selectedPair = pairs[Random.Range(0, pairs.Count)];
                var picked = selectedPair[Random.Range(0, 2)];

                SelectedImportantArticles.Add(picked);
                pairedByGenre[picked.PairID] = selectedPair;

                // Entferne beide aus Pool
                foreach (var article in selectedPair)
                    genre.Items.Remove(article);
            }

            // 3. Story-Fortsetzung (Slot 4)
            if (enableContinuationSlot)
                AddOptionalPairArticle(slotsNeeded: 1);

            // 4. Optionaler 5ter Artikel (Slot 5)
            if (enableFifthSlot && Random.value < fifthSlotChance.Value / 100f)
            {
                Debug.Log($"[NewsSelector] 🎲 Rolled fifth slot: continuing story.");
                AddOptionalPairArticle(1);
            }

            if (debugLogging)
                LogSelectedArticles();
        }

        private void AddOptionalPairArticle(int slotsNeeded)
        {
            var alreadyUsedOpposites = SelectedImportantArticles
                .Select(a => a.PairID)
                .GroupBy(id => id)
                .Where(g => g.Count() > 1)
                .Select(g => g.Key)
                .ToHashSet();

            var weighted = new List<(Article primary, Article opposite, float weight)>();

            foreach (var a in SelectedImportantArticles)
            {
                if (!pairedByGenre.ContainsKey(a.PairID) || alreadyUsedOpposites.Contains(a.PairID))
                    continue;

                var opposite = pairedByGenre[a.PairID].First(x => x.AgencyID != a.AgencyID);
                float value = Mathf.Max(a.ValuePositive, a.ValueNegative);
                weighted.Add((a, opposite, value));
            }

            float total = weighted.Sum(w => w.weight);
            if (total <= 0f) return;

            var normalized = weighted.Select(w =>
            {
                float norm = w.weight / total;
                float clamped = Mathf.Clamp(norm, minContinuationChance.Value / 100f, maxContinuationChance.Value / 100f);
                return (w.primary, w.opposite, clamped);
            }).ToList();

            for (int i = 0; i < slotsNeeded && normalized.Count > 0; i++)
            {
                var chosen = normalized.OrderBy(x => Random.value * (1f / x.clamped)).FirstOrDefault();

                if (debugLogging)
                {
                    Debug.Log($"[NewsSelector] 🎯 Continuation candidates:");
                    foreach (var entry in normalized)
                    {
                        float v = Mathf.Max(entry.primary.ValuePositive, entry.primary.ValueNegative);
                        Debug.Log($"→ PairID: {entry.primary.PairID}, Value: {v:0.00}, Chance: {entry.clamped * 100f:0.0}%");
                    }
                }

                if (chosen.opposite != null)
                    SelectedImportantArticles.Add(chosen.opposite);
            }
        }

        private void LogSelectedArticles()
        {
            Debug.Log("[NewsSelector] Selected Important Articles:");
            foreach (var a in SelectedImportantArticles)
            {
                Debug.Log($"→ {a.Headline} | Agency: {a.AgencyID}, Pair: {a.PairID}, Value+: {a.ValuePositive}, Value-: {a.ValueNegative}");
            }
        }
    }
}