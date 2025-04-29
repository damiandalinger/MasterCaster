/// <summary>
/// Imports articles from JSON files into ArticleDatabases at runtime.
/// Automatically classifies each article based on the description length.
/// </summary>

/// <remarks>
/// 28/04/2025 by Damian Dalinger: Initial creation.
/// 29/04/2025 by Damian Dalinger: Refactoring and updating to tech bible standarts.
/// </remarks>

using UnityEngine;
using System.Collections.Generic;

namespace ProjectCeros
{
    public class NewsImporter : MonoBehaviour
    {
        #region Fields
        [Header("JSON Files")]
        [Tooltip("TextAsset containing important articles in JSON array format.")]
        [SerializeField] private TextAsset _importantArticlesJson;

        [Tooltip("TextAsset containing random filler articles in JSON array format.")]
        [SerializeField] private TextAsset _randomArticlesJson;

        [Header("Article Databases")]
        [Tooltip("Runtime database for important articles.")]
        [SerializeField] private ArticleDatabase _importantArticlesDatabase;

        [Tooltip("Runtime database for random filler articles.")]
        [SerializeField] private ArticleDatabase _randomArticlesDatabase;

        [Header("Article Size Settings")]
        [Tooltip("Maximum character length for a short description.")]
        [SerializeField] private IntReference _shortMaxLength;

        [Tooltip("Maximum character length for a medium description.")]
        [SerializeField] private IntReference _mediumMaxLength;

        [Tooltip("Maximum allowed character length for any description.")]
        [SerializeField] private IntReference _longMaxLength;

        [Header("Category Value Settings")]
        [Tooltip("Int value assigned to short descriptions.")]
        [SerializeField] private IntReference _shortCategoryValue;

        [Tooltip("Int value assigned to medium descriptions.")]
        [SerializeField] private IntReference _mediumCategoryValue;

        [Tooltip("Int value assigned to long descriptions.")]
        [SerializeField] private IntReference _longCategoryValue;
        #endregion

        #region Lifecycle Methods
        private void Awake()
        {
            ImportArticles();
        }
        #endregion

        #region Public Methods
        // Imports, classifies, and stores important and random articles from JSON files.
        public void ImportArticles()
        {
            ClearDatabases();

            List<Article> importantArticles = JsonUtilityWrapper.FromJsonArray<Article>(_importantArticlesJson.text);
            List<Article> randomArticles = JsonUtilityWrapper.FromJsonArray<Article>(_randomArticlesJson.text);

            ClassifyArticles(importantArticles);
            ClassifyArticles(randomArticles);

            AddArticlesToDatabase(_importantArticlesDatabase, importantArticles);
            AddArticlesToDatabase(_randomArticlesDatabase, randomArticles);

            Debug.Log($"[NewsImporter] Imported {importantArticles.Count} important and {randomArticles.Count} random headlines.");
        }
        #endregion

        #region Private Methods
        // Clears all items from both article databases.
        private void ClearDatabases()
        {
            _importantArticlesDatabase.Items.Clear();
            _randomArticlesDatabase.Items.Clear();
        }

        // Classifies a list of articles based on their description lengths.
        private void ClassifyArticles(List<Article> articles)
        {
            foreach (var article in articles)
            {
                int length = article.Description.Length;

                if (length > _longMaxLength.Value)
                {
                    int exceededBy = length - _longMaxLength.Value;
                    Debug.LogWarning($"[NewsImporter] Article \"{article.Headline}\" description is {length} characters long, exceeding the maximum allowed limit({_longMaxLength.Value}) by {exceededBy} characters.");
                }

                if (length <= _shortMaxLength.Value)
                {
                    article.SizeCategory = _shortCategoryValue.Value;
                }
                else if (length <= _mediumMaxLength.Value)
                {
                    article.SizeCategory = _mediumCategoryValue.Value;
                }
                else
                {
                    article.SizeCategory = _longCategoryValue.Value;
                }
            }
        }

        // Adds a list of articles into the specified article database.
        private void AddArticlesToDatabase(ArticleDatabase database, List<Article> articles)
        {
            foreach (var article in articles)
            {
                database.Add(article);
            }
        }
        #endregion
    }
}