/// <summary>
/// Imports and classifies article data from JSON sources into in-game databases.
/// Automatically assigns category values based on description length thresholds.
/// </summary>

/// <remarks>
/// 28/04/2025 by Damian Dalinger: Initial creation.
/// 07/05/2025 by Damian Dalinger: Tech-Bible cleanup.
/// </remarks>

using UnityEngine;
using System.Collections.Generic;

namespace ProjectCeros
{

    public class NewsImporter : MonoBehaviour
    {
        #region Fields

        [Header("Import Sources")]
        [Tooltip("List of JSON/database pairs for importing various article pools.")]
        [SerializeField] private List<JsonImportSource> _importSources;

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

        // Should eventually be replaced by explicit game initialization.
        private void Awake()
        {
            ImportAll();
        }

        #endregion

        #region Public Methods

        // Imports article data from all defined JSON sources and classifies them based on description length.
        // Fills the corresponding target databases with the processed articles.
        private void ImportAll()
        {
            foreach (var source in _importSources)
            {
                List<Article> articles = JsonUtilityWrapper.FromJsonArray<Article>(source.Json.text);
                ClassifyArticles(articles);
                AddArticlesToDatabase(source.TargetDatabase, articles);

                Debug.Log($"[NewsImporter] Imported {articles.Count} articles into {source.TargetDatabase.name}.");
            }
        }

        #endregion

        #region Private Methods

        // Clears the current contents of the given article database and adds the new articles to it.
        private void AddArticlesToDatabase(ArticleDatabase db, List<Article> articles)
        {
            db.Items.Clear();
            foreach (var article in articles)
            {
                db.Add(article);
            }
        }

        // Classifies each article based on its description length.
        private void ClassifyArticles(List<Article> articles)
        {
            var shortLimit = _shortMaxLength.Value;
            var mediumLimit = _mediumMaxLength.Value;
            var longLimit = _longMaxLength.Value;

            foreach (var article in articles)
            {
                var length = article.Description.Length;

                if (length > longLimit)
                {
                    var excess = length - longLimit;
                    Debug.LogWarning($"[NewsImporter] \"{article.Headline}\" exceeds max length by {excess} characters.");
                }

                article.SizeCategory = GetSizeCategory(length, shortLimit, mediumLimit);
            }
        }

        // Determines the size category value for an article based on its description length.
        private int GetSizeCategory(int length, int shortLimit, int mediumLimit)
        {
            if (length <= shortLimit)
            {
                return _shortCategoryValue.Value;
            }

            if (length <= mediumLimit)
            {
                return _mediumCategoryValue.Value;
            }

            return _longCategoryValue.Value;
        }

        #endregion
    }
}
