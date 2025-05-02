using UnityEngine;
using System.Collections.Generic;
using System.Linq;

namespace ProjectCeros
{
    /// <summary>
    /// Handles the reshuffling of eligible article pools based on their corresponding all article pools.
    /// </summary>
    public class NewsPoolReshuffler : MonoBehaviour
    {
        [Header("Genre Pool Pairs")]
        [Tooltip("List of genre pool pairs linking all articles with their eligible counterparts.")]
        [SerializeField] private List<GenrePoolPair> genrePoolPairs;

        [Header("Debugging")]
        [Tooltip("Enable debug logging to see when pools are reshuffled.")]
        [SerializeField] private bool debugLogging = false;

        private void Start()
        {
            InitializeEligiblePools();
        }

        private void Update()
        {
            ReshufflePoolsIfNeeded();
        }

        /// <summary>
        /// Fills all eligible pools with a fresh copy of their full article pool.
        /// Called once at startup.
        /// </summary>
        private void InitializeEligiblePools()
        {
            foreach (var pair in genrePoolPairs)
            {
                RefreshPool(pair);

                if (debugLogging)
                {
                    Debug.Log($"[NewsPoolReshuffler] ✅ Initialized eligible pool for genre: {GetGenreName(pair)}");
                }
            }
        }

        /// <summary>
        /// Checks each pool once per frame and reshuffles any with one or fewer valid article pairs.
        /// </summary>
        private void ReshufflePoolsIfNeeded()
        {
            foreach (var pair in genrePoolPairs)
            {
                if (pair.AllArticles == null || pair.EligibleArticles == null) continue;

                bool shouldReshuffle = false;

                if (pair.UsesPairs)
                {
                    int pairCount = CountValidPairs(pair.EligibleArticles);
                    shouldReshuffle = pairCount <= 1;
                }
                else
                {
                    // Für Random News: Wenn zu wenige Artikel vorhanden sind, reshufflen
                    shouldReshuffle = pair.EligibleArticles.Items.Count <= 1;
                }

                if (shouldReshuffle)
                {
                    RefreshPool(pair);

                    if (debugLogging)
                    {
                        Debug.Log($"[NewsPoolReshuffler] 🔄 Reshuffled eligible pool for genre: {GetGenreName(pair)}");
                    }
                }
            }
        }

        /// <summary>
        /// Clears and refills a single eligible pool from its full pool.
        /// </summary>
        private void RefreshPool(GenrePoolPair pair)
        {
            pair.EligibleArticles.Items.Clear();
            pair.EligibleArticles.Items.AddRange(pair.AllArticles.Items);
        }

        /// <summary>
        /// Counts the number of valid article pairs (based on matching PairIDs).
        /// </summary>
        private int CountValidPairs(ArticleDatabase db)
        {
            return db.Items
                .Where(a => a != null && a.PairID != 0)
                .GroupBy(a => a.PairID)
                .Count(g => g.Count() == 2);
        }

        /// <summary>
        /// Tries to use the name of the full article pool as the genre name for logging.
        /// </summary>
        private string GetGenreName(GenrePoolPair pair)
        {
            return pair.AllArticles != null ? pair.AllArticles.name : "(Unnamed Genre)";
        }
    }
}