/// <summary>
/// Manages reshuffling of article pools for different genres, building dynamic story queues based on article metadata.
/// </summary>

/// <remarks>
/// 05/05/2025 by Damian Dalinger: Script creation.
/// </remarks>

using UnityEngine;
using System.Collections.Generic;
using System.Linq;

namespace ProjectCeros
{
    public class NewsDatabaseReshuffler : MonoBehaviour
    {
        #region Fields

        [Header("Story Offsets")]
        [Tooltip("Probability that the next part of a story appears directly after the current one.")]
        [SerializeField] private FloatReference _storyChanceOffset1;

        [Tooltip("Probability that the next part of a story appears two slots after the current one. The third slot has a 100% chance.")]
        [SerializeField] private FloatReference _storyChanceOffset2;

        [Header("Category Value Settings")]
        [Tooltip("Int value assigned to short descriptions.")]
        [SerializeField] private IntReference _shortCategoryValue;

        [Tooltip("Int value assigned to medium descriptions.")]
        [SerializeField] private IntReference _mediumCategoryValue;

        [Tooltip("Int value assigned to long descriptions.")]
        [SerializeField] private IntReference _longCategoryValue;

        [Header("Article Pool Pairs")]
        [Tooltip("List of article pool pairs linking all articles with their eligible counterparts.")]
        [SerializeField] private List<ArticlePoolPair> _articlePoolPairs;

        [Header("Debugging")]
        [Tooltip("Enables debug logging for internal processing steps.")]
        [SerializeField] private bool _isDebugLogging = false;

        #endregion

        #region Public Methods

        // Reshuffles all article pools from scratch.
        // Called on start and available via context menu for debugging.
        [ContextMenu("DEBUG: Reshuffle All Pools")]
        public void RefreshAllPools()
        {
            foreach (var pair in _articlePoolPairs)
            {
                BuildEligibleQueue(pair);

                Log($" Full reshuffle for genre: {GetGenreName(pair)}");
            }
            PrintEligibleQueues();
        }

        // Reshuffles only those article pools whose eligible queues are currently empty.
        public void ReshufflePoolsIfNeeded()
        {
            foreach (var pair in _articlePoolPairs)
            {
                var items = pair.EligibleArticles.Items;

                if (pair.DisableAutoReshuffle)
                {
                    if (items.Count == 0)
                    {
                        BuildEligibleQueue(pair);
                        Log($"Reshuffled empty genre: {GetGenreName(pair)}");
                    }
                    continue;
                }

                // Logic for random articles: if a size category is empty it is reshuffled.
                if (!pair.UsesPairs)
                {
                    int shortCount = items.Count(a => a.SizeCategory == _shortCategoryValue.Value);
                    int mediumCount = items.Count(a => a.SizeCategory == _mediumCategoryValue.Value);
                    int longCount = items.Count(a => a.SizeCategory == _longCategoryValue.Value);

                    if (shortCount == 0 || mediumCount == 0 || longCount == 0)
                    {
                        BuildEligibleQueue(pair);
                        Log($"[RandomPool] Reshuffled due to missing category: S={shortCount}, M={mediumCount}, L={longCount}");
                        continue;
                    }
                }

                if (items.Count == 0)
                {
                    BuildEligibleQueue(pair);
                    Log($"Reshuffled empty genre: {GetGenreName(pair)}");
                }
            }
        }

        #endregion

        #region Private Methods

        // Converts all articles into logical blocks (either single articles or pairs).
        private List<ArticleBlock> BuildArticleBlocks(List<Article> articles)
        {
            var blocks = new List<ArticleBlock>();

            // Group articles by PairID to build pairs.
            blocks.AddRange(articles
                .Where(a => a.PairID != 0)
                .GroupBy(a => a.PairID)
                .Select(g => new ArticleBlock(g.ToList())));

            // Add single articles directly.
            blocks.AddRange(articles
                .Where(a => a.PairID == 0)
                .Select(a => new ArticleBlock(new List<Article> { a })));

            return blocks;
        }

        // Builds and reshuffles the eligible article queue for a given genre.
        // Story continuations are inserted with probabilistic offsets.
        private void BuildEligibleQueue(ArticlePoolPair pair)
        {
            var blocks = BuildArticleBlocks(pair.AllArticles.Items);
            var baseBlocks = GetBaseBlocks(blocks);

            if (pair.UsesPairs)
            {
                var storyLookup = blocks
                    .Where(b => b.StoryID != 0)
                    .GroupBy(b => b.StoryID)
                    .Where(g => g.Select(b => b.StoryPart).Distinct().Count() > 1)
                    .ToDictionary(
                        g => g.Key,
                        g => g.ToDictionary(b => b.StoryPart, b => b)
                    );

                InsertPairedStoryContinuations(baseBlocks, storyLookup);
            }

            pair.EligibleArticles.Items.Clear();
            pair.EligibleArticles.Items.AddRange(baseBlocks.SelectMany(b => b.Articles));
        }

        // Filters and randomizes blocks that can serve as story starters (Part 1 or non-story).
        private List<ArticleBlock> GetBaseBlocks(List<ArticleBlock> blocks)
        {
            return blocks
                .Where(b => b.StoryID == 0 || b.StoryPart == 1)
                .OrderBy(_ => Random.value)
                .ToList();
        }

        // DEBUG: Returns the display name of a genre.
        private string GetGenreName(ArticlePoolPair pair)
        {
            return pair.AllArticles != null ? pair.AllArticles.name : "(Unnamed Genre)";
        }

        // Inserts continuation blocks (StoryParts >= 2) into the base queue using probabilistic offset positions.
        private void InsertPairedStoryContinuations(List<ArticleBlock> baseBlocks, Dictionary<int, Dictionary<int, ArticleBlock>> storyLookup)
        {
            foreach (var story in storyLookup.Values)
            {
                if (!story.TryGetValue(1, out var part1Block)) continue;

                int idx = baseBlocks.IndexOf(part1Block);
                if (idx < 0) continue;

                for (int partNum = 2; story.ContainsKey(partNum); partNum++)
                {
                    var block = story[partNum];

                    float rnd = Random.value;
                    int offset = rnd < _storyChanceOffset1 ? 1 : (rnd < _storyChanceOffset2 ? 2 : 3);
                    int insertPos = Mathf.Clamp(idx + offset, 0, baseBlocks.Count);

                    baseBlocks.Insert(insertPos, block);
                    idx = insertPos;
                }
            }
        }

        // DEBUG: Logs a message to the console if debug logging is enabled.
        private void Log(string message)
        {
            if (_isDebugLogging)
                Debug.Log("[NewsPoolReshuffler] " + message);
        }

        // DEBUG: Prints all eligible article queues for each genre to the console.
        private void PrintEligibleQueues()
        {
            foreach (var pair in _articlePoolPairs)
            {
                Log($"--- Eligible Queue for genre: {GetGenreName(pair)} ---");
                var items = pair.EligibleArticles.Items;
                for (int i = 0; i < items.Count; i++)
                {
                    var a = items[i];
                    Log($"{i}: {a.Headline} (StoryID={a.StoryID}, Part={a.StoryPart}, PairID={a.PairID})");
                }
            }
        }

        #endregion

        #region Nested Types

        // A logical container for one or more articles, representing either a single article or a pair.
        private class ArticleBlock
        {
            public List<Article> Articles { get; }
            public int StoryID => Articles[0].StoryID;
            public int StoryPart => Articles[0].StoryPart;

            public ArticleBlock(List<Article> articles)
            {
                Articles = articles;
            }
        }

        #endregion
    }
}