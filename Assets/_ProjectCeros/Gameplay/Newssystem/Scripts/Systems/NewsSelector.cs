/// <summary>
/// Handles the selection of news articles for the newspaper.
/// Pulls from eligible genre pools and ensures correct agency logic.
/// </summary>

/// <remarks>
/// 25/04/2025 by Damian Dalinger: Script creation.
/// 01/05/2025 by Damian Dalinger: Overhaul for the final selection logic.
/// 07/05/2025 by Damian Dalinger: Tech Bible compliance.
/// 14/05/2025 by Damian Dalinger: Weighted genre selection.
/// </remarks>

using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace ProjectCeros
{
    public class NewsSelector : MonoBehaviour
    {
        #region Fields

        [Header("Important Article Databases")]
        [Tooltip("Databases that hold eligible important articles.")]
        [SerializeField] private List<ArticleDatabase> _eligibleImportantArticles;

        [Tooltip("Eligible articles for fruit of the day.")]
        [SerializeField] private ArticleDatabase _eligibleFruitArticles;

        [Header("Selected Articles")]
        [Tooltip("Database that holds selected important articles.")]
        [SerializeField] private ArticleDatabase _selectedImportantArticles;

        [Tooltip("Selected article for fruit of the day.")]
        [SerializeField] private ArticleDatabase _selectedFruitArticle;

        [Tooltip("Last selected genres string runtime set.")]
        [SerializeField] private StringRuntimeSet _lastSelectedGenreNames;

        [Header("Selection Settings")]
        [Tooltip("How many genres should be selected per newspaper.")]
        [SerializeField] private IntReference _genreCount;

        [Tooltip("Penalty multiplier for genres that were selected in the previous selection. Range: 0.0 (strong penalty) to 1.0 (no penalty)")]
        [SerializeField] private FloatReference _repeatPenaltyFactor;

        [Tooltip("Enable a possible hype slot 4.")]
        [SerializeField] private BoolReference _enableHypeSlot;

        [Tooltip("Enable the optional fifth article slot.")]
        [SerializeField] private BoolReference _enableFifthSlot;

        [Tooltip("Chance (in %) to add a fifth important article.")]
        [SerializeField] private FloatReference _fifthSlotChance;

        [Tooltip("Number of random news articles to add at the end.")]
        [SerializeField] private IntReference _randomArticleCount;

        [Header("Hype Chance Settings")]
        [Tooltip("Max % chance for the hype article.")]
        [SerializeField] private FloatReference _maxHypeChance;

        [Tooltip("Min % chance for the hype article.")]
        [SerializeField] private FloatReference _minHypeChance;

        [Header("Debug")]
        [Tooltip("Enable debug logging for selection results and hype process.")]
        [SerializeField] private bool _isDebugLogging = false;

        private NewsDatabaseReshuffler _reshuffler;
        private Dictionary<int, List<Article>> _pairedByGenre = new();

        #endregion

        #region Lifecycle Methods

        private void Awake()
        {
            _reshuffler = GetComponentInChildren<NewsDatabaseReshuffler>();
        }

        #endregion

        #region Public Methods

        // Main selection method that builds a complete set of newspaper content.
        // Pulls from article databases and adds optional random and hype articles.
        public void SelectImportantArticles()
        {
            _reshuffler.ReshufflePoolsIfNeeded();
            _pairedByGenre.Clear();
            _selectedImportantArticles.Clear();
            _selectedFruitArticle.Clear();


            // 1. Select a genre.
            var selectedGenres = SelectGenres();

            // 2. Select the primary importatnt articles from each genre.
            foreach (var genre in selectedGenres)
            {
                var items = genre.Items;
                if (items.Count == 0)
                    continue;

                var pick = Random.value < 0.5f ? items[0] : items[1];

                _selectedImportantArticles.Add(pick);
                _pairedByGenre[pick.PairID] = new List<Article> { items[0], items[1] };

                items.RemoveRange(0, 2);
            }

            // 3. Select a hype article.
            if (_enableHypeSlot)
            {
                AddOptionalPairArticle();
            }

            // 4. Select a random or important article.
            if (_enableFifthSlot)
            {
                bool useImportant = Random.value < (_fifthSlotChance.Value / 100f);
                if (useImportant)
                {
                    AddOptionalPairArticle();
                    Log($"5th Slot → Important Story");
                }
                else
                {
                    //SelectRandomArticles(1);
                    Log($"5th Slot → Random News");
                }
            }

            // 5. Select fruit of the day.
            _reshuffler.ReshufflePoolsIfNeeded();
            SelectFruitOfTheDay();

            LogSelectedArticles();
        }

        #endregion

        #region Private Methods

        // Attempts to add a follow-up story article based on weighted selection.
        private void AddOptionalPairArticle()
        {
            var alreadyUsedOpposites = _selectedImportantArticles.Items
                .Select(a => a.PairID)
                .GroupBy(id => id)
                .Where(g => g.Count() > 1)
                .Select(g => g.Key)
                .ToHashSet();

            var weighted = new List<(Article primary, Article opposite, float weight)>();

            foreach (var a in _selectedImportantArticles.Items)
            {
                if (!_pairedByGenre.ContainsKey(a.PairID) || alreadyUsedOpposites.Contains(a.PairID))
                    continue;

                var opposite = _pairedByGenre[a.PairID].First(x => x.AgencyID != a.AgencyID);
                float value = Mathf.Max(a.ValuePositive, a.ValueNegative);
                weighted.Add((a, opposite, value));
            }

            float total = weighted.Sum(w => w.weight);
            if (total <= 0f) return;

            var normalized = weighted.Select(w =>
            {
                float norm = w.weight / total;
                float clamped = Mathf.Clamp(norm, _minHypeChance.Value / 100f, _maxHypeChance.Value / 100f);
                return (w.primary, w.opposite, clamped);
            }).ToList();

            var chosen = normalized.OrderBy(x => Random.value * (1f / x.clamped)).FirstOrDefault();

            Log($"Continuation candidates:");
            foreach (var entry in normalized)
            {
                float v = Mathf.Max(entry.primary.ValuePositive, entry.primary.ValueNegative);
                Log($"→ PairID: {entry.primary.PairID}, Value: {v:0.00}, Chance: {entry.clamped * 100f:0.0}%");
            }

            if (chosen.opposite != null)
            {
                _selectedImportantArticles.Add(chosen.opposite);
            }
        }

        // DEBUG: Logs a message to the console if debug logging is enabled.
        private void Log(string message)
        {
            if (_isDebugLogging)
                Debug.Log("[NewsSelector] " + message);
        }

        // DEBUG: Logs all selected important articles.
        private void LogSelectedArticles()
        {
            Log($"Selected Important Articles:");
            foreach (var a in _selectedImportantArticles.Items)
            {
                Log($"→ {a.Headline} | Agency: {a.AgencyID}, Pair: {a.PairID}, Value+: {a.ValuePositive}, Value-: {a.ValueNegative}");
            }
        }

        // Selects a set of genres using weighted randomness.
        // Genres selected in the previous round have a reduced chance of being picked again.
        public List<ArticleDatabase> SelectGenres()
        {
            // Step 1: Create weighted list
            var weightedGenres = new List<(ArticleDatabase db, float weight)>();

            foreach (var db in _eligibleImportantArticles)
            {
                bool wasPreviouslySelected = _lastSelectedGenreNames.Items.Contains(db.name);
                float baseWeight = Random.value;

                float finalWeight = wasPreviouslySelected ? baseWeight * _repeatPenaltyFactor : baseWeight;
                weightedGenres.Add((db, finalWeight));
            }

            // Step 2: Sort by weight descending (more likely = higher finalWeight)
            var selectedGenres = weightedGenres
                .OrderByDescending(entry => entry.weight)
                .Take(_genreCount.Value)
                .Select(entry => entry.db)
                .ToList();

            // Step 3: Store current selection as last
            _lastSelectedGenreNames.Clear();
            foreach (var genre in selectedGenres)
            {
                _lastSelectedGenreNames.Add(genre.name);
            }

            return selectedGenres;
        }

        // Selects the first fruit article.
        private void SelectFruitOfTheDay()
        {
            var items = _eligibleFruitArticles.Items;
            _selectedFruitArticle.Add(items[0]);
            items.RemoveAt(0);
        }

        #endregion
    }
}