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

        [Header("Layout Settings")]
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

        private NewsSelector _selector;
        private NewsGridRenderer _renderer;
        #endregion

        #region Lifecycle Methods

        private void Awake()
        {
            _selector = GetComponent<NewsSelector>();
            _renderer = GetComponent<NewsGridRenderer>();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                BuildLayout();
            }
        }
        #endregion

        #region Public Methods
        // Builds a complete newspaper layout by selecting articles and rendering them.
        public void BuildLayout()
        {
            _selector.GenerateArticlePools();

            var importantArticles = _selector.ImportantArticlesPool;
            var randomArticles = _selector.RandomArticlesPool;

            var chosenPreset = FindSuitablePreset(importantArticles);

            if (chosenPreset == null)
            {
                Debug.LogError("[LayoutManager] No suitable preset found!");
                return;
            }

            Debug.Log($"[LayoutManager] Preset {chosenPreset.name} selected.");

            var assignments = AssignBlocks(chosenPreset, importantArticles, randomArticles);
            _renderer.Render(assignments);
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
        private List<BlockAssignment> AssignBlocks(LayoutPreset preset, List<Article> important, List<Article> random)
        {
            var result = new List<BlockAssignment>();

            var longImportant = important.Where(h => h.SizeCategory == _longCategoryValue.Value).ToList();
            var mediumImportant = important.Where(h => h.SizeCategory == _mediumCategoryValue.Value).ToList();
            var shortImportant = important.Where(h => h.SizeCategory == _shortCategoryValue.Value).ToList();

            var longRandom = random.Where(h => h.SizeCategory == _longCategoryValue.Value).ToList();
            var mediumRandom = random.Where(h => h.SizeCategory == _mediumCategoryValue.Value).ToList();
            var shortRandom = random.Where(h => h.SizeCategory == _shortCategoryValue.Value).ToList();

            var blocks = preset.Blocks.OrderByDescending(b => GetBlockCategory(b)).ToList();

            foreach (var block in blocks)
            {
                int blockCategory = GetBlockCategory(block);

                if (block.IsImportantNews)
                {
                    var article = GetMatchingArticle(blockCategory, longImportant, mediumImportant, shortImportant);

                    if (article != null)
                    {
                        result.Add(new BlockAssignment
                        {
                            Block = block,
                            ArticleHeadline = article.Headline,
                            ArticleDescription = article.Description,
                            IsImportant = true
                        });
                    }
                }
                else
                {
                    var filler = GetMatchingArticle(blockCategory, longRandom, mediumRandom, shortRandom);

                    if (filler != null)
                    {
                        result.Add(new BlockAssignment
                        {
                            Block = block,
                            ArticleHeadline = filler.Headline,
                            ArticleDescription = filler.Description,
                            IsImportant = false
                        });
                    }
                    else
                    {
                        result.Add(new BlockAssignment
                        {
                            Block = block,
                            ArticleHeadline = "Error",
                            ArticleDescription = "",
                            IsImportant = false
                        });
                    }
                }
            }

            return result;
        }

        // Retrieves a matching article from the lists based on block category.
        private Article GetMatchingArticle(int blockCategory, List<Article> longList, List<Article> mediumList, List<Article> shortList)
        {
            if (blockCategory == _longCategoryValue.Value && longList.Count > 0)
            {
                var article = longList[0];
                longList.RemoveAt(0);
                return article;
            }
            if (blockCategory == _mediumCategoryValue.Value && mediumList.Count > 0)
            {
                var article = mediumList[0];
                mediumList.RemoveAt(0);
                return article;
            }
            if (blockCategory == _shortCategoryValue.Value && shortList.Count > 0)
            {
                var article = shortList[0];
                shortList.RemoveAt(0);
                return article;
            }

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
        #endregion
    }
}
