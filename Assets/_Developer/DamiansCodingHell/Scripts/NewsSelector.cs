/// <summary>
/// Selects pools of important and random articles from available databases.
/// These pools are used to build the newspaper layout.
/// </summary>

/// <remarks>
/// 25/04/2025 by Damian Dalinger: Script Creation.
/// </remarks>

using UnityEngine;
using System.Collections.Generic;
using System.Linq;

namespace ProjectCeros
{
    public class NewsSelector : MonoBehaviour
    {
        #region Fields
        [Header("Article Databases")]
        [Tooltip("Database containing all important articles.")]
        [SerializeField] private ArticleDatabase _importantArticlesDatabase;

        [Tooltip("Database containing all random filler articles.")]
        [SerializeField] private ArticleDatabase _randomArticlesDatabase;

        [HideInInspector] public List<Article> ImportantArticlesPool;
        [HideInInspector] public List<Article> RandomArticlesPool;
        #endregion

        #region Public Methods
        // Randomly generates important and random article pools for today's newspaper.
        public void GenerateArticlePools()
        {
            ImportantArticlesPool = _importantArticlesDatabase.Items
                .OrderBy(x => Random.value)
                .Take(5)
                .ToList();

            RandomArticlesPool = _randomArticlesDatabase.Items
                .OrderBy(x => Random.value)
                .ToList();

            Debug.Log($"[NewsSelector] Generated {ImportantArticlesPool.Count} important and {RandomArticlesPool.Count} random articles.");
        }
        #endregion
    }
}