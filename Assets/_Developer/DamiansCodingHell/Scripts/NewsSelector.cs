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
        [Header("Important News Pools")]
        [Tooltip("Databases that hold eligible important articles (by genre).")]
        [SerializeField] private List<ArticleDatabase> eligibleImportantGenrePools;

        [Header("Random News Pool")]
        [Tooltip("Single database that holds eligible random articles.")]
        [SerializeField] private ArticleDatabase eligibleRandomArticles;

        [Tooltip("Eligible articles for fruit of the day.")]
        [SerializeField] private ArticleDatabase eligibleFruitArticles;

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
        private NewsPoolReshuffler _reshuffler;

        [Header("Debug")]
        [SerializeField] private bool debugLogging = false;

        public List<Article> SelectedImportantArticles { get; private set; } = new();
        public List<Article> SelectedRandomArticles { get; private set; } = new();
        public Article SelectedFruitArticle { get; private set; }
        private Dictionary<int, List<Article>> pairedByGenre = new();

        void Start()
        {
            _reshuffler = GetComponentInChildren<NewsPoolReshuffler>();
        }
        public void SelectImportantArticles()
        {
            _reshuffler.ReshufflePoolsIfNeeded();
            SelectedImportantArticles.Clear();
            SelectedRandomArticles.Clear();
            SelectedFruitArticle = null;
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
                var items = genre.Items;
                if (items.Count == 0)
                    continue;

                // Prüfen, ob die beiden obersten Elemente ein Paar sind
                if (items[0].PairID != 0
                    && items.Count > 1
                    && items[1].PairID == items[0].PairID)
                {
                    // Zufällig eines der beiden aus den oberen 2 nehmen
                    var pick = Random.value < 0.5f ? items[0] : items[1];

                    SelectedImportantArticles.Add(pick);
                    pairedByGenre[pick.PairID] = new List<Article> { items[0], items[1] };

                    // Entferne beide Varianten aus dem Pool
                    items.RemoveRange(0, 2);
                }
                else
                {
                    // Kein Paar, einfach den obersten Artikel nehmen
                    SelectedImportantArticles.Add(items[0]);
                    items.RemoveAt(0);
                }
            }

            // 3. Story-Fortsetzung (Slot 4)
            if (enableContinuationSlot)
                AddOptionalPairArticle(slotsNeeded: 1);

            // 4. Optionaler 5ter Artikel (Slot 5)
            if (enableFifthSlot)
            {
                bool useImportant = Random.value < (fifthSlotChance.Value / 100f);
                if (useImportant)
                {
                    if (debugLogging)
                        Debug.Log("[NewsSelector] 🎲 5th Slot → Important Story");

                    AddOptionalPairArticle(1);
                }
                else
                {
                    if (debugLogging)
                        Debug.Log("[NewsSelector] 🎲 5th Slot → Random News");

                    SelectRandomArticles(1);
                }
            }

            // 5. Slot 6 – immer eine Random News
            SelectRandomArticles(1);
            SelectFruitOfTheDay();

            if (debugLogging)
                LogSelectedArticles();
            LogSelectedRandoms();
        }

        private void SelectRandomArticles(int count)
        {
            var items = eligibleRandomArticles.Items;
            for (int i = 0; i < count && items.Count > 0; i++)
            {
                // Nimm immer das oberste Element
                var selected = items[0];
                SelectedRandomArticles.Add(selected);
                items.RemoveAt(0);
            }
        }

        private void SelectFruitOfTheDay()
        {
            var items = eligibleFruitArticles.Items;
            if (items.Count == 0)
                return;

            // Immer das oberste Element nehmen
            SelectedFruitArticle = items[0];
            items.RemoveAt(0);
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

        private void LogSelectedRandoms()
        {
            Debug.Log("[NewsSelector] Selected Random Articles:");
            foreach (var a in SelectedRandomArticles)
                Debug.Log($"→ {a.Headline}");
        }
    }
}