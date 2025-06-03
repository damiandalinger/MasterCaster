/// <summary>
/// Manages building the newspaper layout by coordinating the NewsSelector and NewsGridRenderer.
/// Chooses a LayoutPreset based on the number of important headlines selected.
/// </summary>

/// <remarks>
/// 25/04/2025 by Damian Dalinger: Script creation.
/// 29/04/2025 by Damian Dalinger: Major refactor, cleanup, and optimizations.
/// 03/06/2025 by Damian Dalinger: Added ArticleBackgroundName and UseCustomBackground.
/// </remarks>

using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace ProjectCeros
{
    public class LayoutManager : MonoBehaviour
    {
        #region Fields

        [Tooltip("Available layout presets.")]
        [SerializeField] private List<LayoutPreset> _layoutPresets;

        [Header("Block Size Settings")]
        [Tooltip("Maximum area (width x height) a block can have to be considered 'short'.")]
        [SerializeField] private IntReference _shortBlockAreaLimit;

        [Tooltip("Maximum area (width x height) a block can have to be considered 'medium'.")]
        [SerializeField] private IntReference _mediumBlockAreaLimit;

        [Header("Category Value Settings")]
        [Tooltip("Int value assigned to short descriptions.")]
        [SerializeField] private IntReference _shortCategoryValue;

        [Tooltip("Int value assigned to medium descriptions.")]
        [SerializeField] private IntReference _mediumCategoryValue;

        [Tooltip("Int value assigned to long descriptions.")]
        [SerializeField] private IntReference _longCategoryValue;

        [Tooltip("Mapping of all block prefabs used for layout generation.")]
        [SerializeField] private BlockPrefabMapping _prefabMapping;

        [Header("Runtime Data")]
        [Tooltip("Database that holds selected important articles.")]
        [SerializeField] private ArticleDatabase _selectedImportantArticles;

        [Tooltip("Selected article for fruit of the day.")]
        [SerializeField] private ArticleDatabase _selectedFruitArticle;

        [Tooltip("Random articles database.")]
        [SerializeField] private ArticleDatabase _randomArticlesDatabase;

        public List<BlockAssignment> CurrentAssignments { get; private set; }

        private NewsSelector _selector;
        private NewsDatabaseReshuffler _reshuffler;

        #endregion

        #region Lifecycle Methods

        private void Awake()
        {
            _selector = GetComponent<NewsSelector>();
            _reshuffler = GetComponentInChildren<NewsDatabaseReshuffler>();
        }

        #endregion

        #region Public Methods

        // Builds a complete newspaper layout by selecting articles and rendering them.
        public void BuildLayout()
        {
            _selector.SelectImportantArticles();

            var importantArticles = _selectedImportantArticles.Items;
            var randomArticles = _randomArticlesDatabase.Items;
            var fruitArticle = _selectedFruitArticle.Items.FirstOrDefault();

            var chosenPreset = FindSuitablePreset(importantArticles);

            if (chosenPreset == null)
            {
                int requiredShort = importantArticles.Count(h => h.SizeCategory == _shortCategoryValue.Value);
                int requiredMedium = importantArticles.Count(h => h.SizeCategory == _mediumCategoryValue.Value);
                int requiredLong = importantArticles.Count(h => h.SizeCategory == _longCategoryValue.Value);

                string log = "[LayoutManager] No suitable preset found!\n" +
                             $"Needed Important Blocks:\n" +
                             $"- Short: {requiredShort}\n" +
                             $"- Medium: {requiredMedium}\n" +
                             $"- Long: {requiredLong}\n" +
                             $"Total Important Articles: {importantArticles.Count}";

                Debug.LogError(log);
                return;
            }

            var assignments = AssignBlocks(chosenPreset, importantArticles, randomArticles, fruitArticle);
            CurrentAssignments = assignments;
        }

        #endregion

        #region Private Methods

        // Finds a preset where the number of important blocks exactly matches the number of important articles.
        private LayoutPreset FindSuitablePreset(List<Article> importantArticles)
        {
            int requiredShort = importantArticles.Count(h => h.SizeCategory == _shortCategoryValue.Value);
            int requiredMedium = importantArticles.Count(h => h.SizeCategory == _mediumCategoryValue.Value);
            int requiredLong = importantArticles.Count(h => h.SizeCategory == _longCategoryValue.Value);

            var shuffledPresets = _layoutPresets.OrderBy(x => Random.value).ToList();

            foreach (var preset in shuffledPresets)
            {
                var importantBlocks = preset.Blocks.Where(b => b.IsImportantNews).ToList();

                int availableShort = importantBlocks.Count(b => GetBlockCategory(b) == _shortCategoryValue.Value);
                int availableMedium = importantBlocks.Count(b => GetBlockCategory(b) == _mediumCategoryValue.Value);
                int availableLong = importantBlocks.Count(b => GetBlockCategory(b) == _longCategoryValue.Value);

                if (availableShort == requiredShort &&
                    availableMedium == requiredMedium &&
                    availableLong == requiredLong)
                {
                    return preset;
                }
            }

            return null;
        }

        private List<BlockAssignment> AssignBlocks(LayoutPreset preset, List<Article> important, List<Article> random, Article fruit)
        {
            var result = new List<BlockAssignment>();

            // 1. Sortiere wichtige Artikel nach Größe
            var longImportant = important.Where(a => a.SizeCategory == _longCategoryValue.Value).ToList();
            var mediumImportant = important.Where(a => a.SizeCategory == _mediumCategoryValue.Value).ToList();

            // 2. Kopie der Blöcke, da wir entfernen
            var remainingBlocks = new List<LayoutBlock>(preset.Blocks);

            // 3. Wichtige Artikel platzieren
            foreach (var article in important)
            {
                var category = article.SizeCategory;
                LayoutBlock matchedBlock = null;

                if (category == _longCategoryValue.Value)
                {
                    matchedBlock = remainingBlocks.FirstOrDefault(b => b.IsImportantNews && GetBlockCategory(b) == _longCategoryValue.Value);
                }
                else if (category == _mediumCategoryValue.Value)
                {
                    matchedBlock = remainingBlocks.FirstOrDefault(b => b.IsImportantNews && GetBlockCategory(b) == _mediumCategoryValue.Value);
                }

                if (matchedBlock != null)
                {
                    var size = matchedBlock.GetSize();
                    bool is2x2 = size.x == 2 && size.y == 2;

                    result.Add(new BlockAssignment
                    {
                        Prefab = _prefabMapping.GetPrefab(matchedBlock.GetSize(), article.AgencyID, true),
                        Position = matchedBlock.Position,
                        ArticleHeadline = article.Headline,
                        ArticleDescription = article.Description,
                        ArticleBackgroundName = is2x2 ? article.BackgroundName : null,
                        UseCustomBackground = is2x2
                    });

                    remainingBlocks.Remove(matchedBlock);
                }
                else
                {
                    Debug.LogError($"[LayoutManager] No matching block found for important article: {article.Headline}");
                }
            }

            // 4. Fruit of the Week in freien Short-Block (nicht wichtig)
            if (fruit != null)
            {
                var fruitBlock = remainingBlocks
                    .FirstOrDefault(b => !b.IsImportantNews && GetBlockCategory(b) == _shortCategoryValue.Value);

                if (fruitBlock != null)
                {
                    result.Add(new BlockAssignment
                    {
                        Prefab = _prefabMapping.GetPrefab(fruitBlock.GetSize(), fruit.AgencyID, false),
                        Position = fruitBlock.Position,
                        ArticleHeadline = fruit.Headline,
                        ArticleDescription = fruit.Description,
                        ArticleBackgroundName = fruit.BackgroundName,
                        UseCustomBackground = true
                    });

                    remainingBlocks.Remove(fruitBlock);
                }
                else
                {
                    Debug.LogWarning("[LayoutManager] No suitable block found for Fruit of the Week.");
                }
            }

            // 5. Randomartikel füllen die restlichen nicht-wichtigen Blöcke (direkt aus Original-Pool)
            foreach (var block in remainingBlocks.Where(b => !b.IsImportantNews))
            {
                _reshuffler.ReshufflePoolsIfNeeded();
                int blockCategory = GetBlockCategory(block);

                var article = TryGetFittingRandomArticle(blockCategory, random);
                if (article != null)
                {
                    var size = block.GetSize();
                    bool is2x2 = size.x == 2 && size.y == 2;

                    result.Add(new BlockAssignment
                    {
                        Prefab = _prefabMapping.GetPrefab(block.GetSize(), article.AgencyID, false),
                        Position = block.Position,
                        ArticleHeadline = article.Headline,
                        ArticleDescription = article.Description,
                        ArticleBackgroundName = is2x2 ? article.BackgroundName : null,
                        UseCustomBackground = is2x2
                    });
                }
            }
            return result;
        }

        // Calculates the size category (short, medium, long) of a block based on its area.
        private int GetBlockCategory(LayoutBlock block)
        {
            var size = block.GetSize();
            int area = size.x * size.y;

            if (area <= _shortBlockAreaLimit.Value) return _shortCategoryValue.Value;
            if (area <= _mediumBlockAreaLimit.Value) return _mediumCategoryValue.Value;
            return _longCategoryValue.Value;
        }

        #endregion

        private Article TryGetFittingRandomArticle(int blockCategory, List<Article> pool)
        {
            // 1. Perfekter Match
            var match = pool.FirstOrDefault(a => a.SizeCategory == blockCategory);
            if (match != null)
            {
                pool.Remove(match);
                return match;
            }

            // 2. Fallbacks – kleinere Artikel akzeptieren
            if (blockCategory == _mediumCategoryValue.Value)
            {
                match = pool.FirstOrDefault(a => a.SizeCategory == _shortCategoryValue.Value);
            }
            else if (blockCategory == _longCategoryValue.Value)
            {
                match = pool.FirstOrDefault(a =>
                    a.SizeCategory == _mediumCategoryValue.Value || a.SizeCategory == _shortCategoryValue.Value);
            }

            if (match != null)
            {
                pool.Remove(match);
                return match;
            }

            return null;
        }
    }
}
