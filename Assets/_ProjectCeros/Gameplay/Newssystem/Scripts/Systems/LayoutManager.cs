/// <summary>
/// Manages building the newspaper layout by coordinating the NewsSelector and NewsGridRenderer.
/// Chooses a LayoutPreset based on the number of important headlines selected.
/// </summary>

/// <remarks>
/// 25/04/2025 by Damian Dalinger: Script creation.
/// 29/04/2025 by Damian Dalinger: Major refactor, cleanup, and optimizations.
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

        public List<BlockAssignment> CurrentAssignments { get; private set; }

        private NewsSelector _selector;

        #endregion

        #region Lifecycle Methods

        private void Awake()
        {
            _selector = GetComponent<NewsSelector>();
        }

        #endregion

        #region Public Methods

        // Builds a complete newspaper layout by selecting articles and rendering them.
        public void BuildLayout()
        {
            _selector.SelectImportantArticles();

            var importantArticles = _selector.SelectedImportantArticles;
            var randomArticles = _selector.SelectedRandomArticles;
            var fruitArticle = _selector.SelectedFruitArticle;

            var chosenPreset = FindSuitablePreset(importantArticles);

            if (chosenPreset == null)
            {
                Debug.LogError("[LayoutManager] No suitable preset found!");
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

        // Assigns articles to available blocks based on their size category.
        private List<BlockAssignment> AssignBlocks(LayoutPreset preset, List<Article> important, List<Article> random, Article fruit)
        {
            var result = new List<BlockAssignment>();

            var longImportant = important.Where(a => a.SizeCategory == _longCategoryValue.Value).ToList();
            var mediumImportant = important.Where(a => a.SizeCategory == _mediumCategoryValue.Value).ToList();
            var shortImportant = important.Where(a => a.SizeCategory == _shortCategoryValue.Value).ToList();

            var longRandom = random.Where(a => a.SizeCategory == _longCategoryValue.Value).ToList();
            var mediumRandom = random.Where(a => a.SizeCategory == _mediumCategoryValue.Value).ToList();
            var shortRandom = random.Where(a => a.SizeCategory == _shortCategoryValue.Value).ToList();

            foreach (var block in preset.Blocks)
            {
                int blockCategory = GetBlockCategory(block);

                Article article = null;
                bool isImportant = block.IsImportantNews;

                if (isImportant)
                {
                    article = GetMatchingArticle(blockCategory, longImportant, mediumImportant, shortImportant);
                }
                else
                {
                    article = GetMatchingArticle(blockCategory, longRandom, mediumRandom, shortRandom) ?? fruit;
                    if (article == fruit) fruit = null; 
                }

                if (article != null)
                {
                    result.Add(new BlockAssignment
                    {
                        Prefab = _prefabMapping.GetPrefab(block.GetSize(), article.AgencyID, isImportant),
                        Position = block.Position,
                        ArticleHeadline = article.Headline,
                        ArticleDescription = article.Description,
                        ArticleSubgenre = article.Subgenre
                    });
                }
            }

            return result;
        }

        // Retrieves a matching article from the lists based on block category.
        private Article GetMatchingArticle(int blockCategory, List<Article> longList, List<Article> mediumList, List<Article> shortList)
        {
            if (blockCategory == _longCategoryValue.Value && longList.Count > 0)
                return PopFirst(longList);

            if (blockCategory == _mediumCategoryValue.Value && mediumList.Count > 0)
                return PopFirst(mediumList);

            if (blockCategory == _shortCategoryValue.Value && shortList.Count > 0)
                return PopFirst(shortList);

            return null;
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

        // Pops the first article from the list.
        private Article PopFirst(List<Article> list)
        {
            var article = list[0];
            list.RemoveAt(0);
            return article;
        }

        #endregion
    }
}
