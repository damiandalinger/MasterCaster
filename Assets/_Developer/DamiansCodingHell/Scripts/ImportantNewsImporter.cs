using UnityEngine;
using System.Collections.Generic;

namespace ProjectCeros
{
    /// <summary>
    /// Imports articles from multiple JSON sources into their respective databases at runtime.
    /// Automatically classifies articles by size and logs any issues.
    /// </summary>
    /// <remarks>
    /// 30/04/2025 by ChatGPT: Refactored for dynamic JSON-DB pairing.
    /// </remarks>
    public class ImportantNewsImporter : MonoBehaviour
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

        #region Unity Lifecycle

        private void Awake()
        {
            ImportAll();
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Processes all import sources and fills their respective target databases.
        /// </summary>
        public void ImportAll()
        {
            foreach (var source in _importSources)
            {
                if (source.json == null || source.targetDatabase == null)
                {
                    Debug.LogWarning("[NewsImporter] Skipped source with missing JSON or database.");
                    continue;
                }

                List<Article> articles = JsonUtilityWrapper.FromJsonArray<Article>(source.json.text);
                ClassifyArticles(articles);
                AddArticlesToDatabase(source.targetDatabase, articles);

                Debug.Log($"[NewsImporter] ✅ Imported {articles.Count} articles into {source.targetDatabase.name}.");
            }
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Classifies a list of articles based on their description lengths.
        /// </summary>
        private void ClassifyArticles(List<Article> articles)
        {
            foreach (var article in articles)
            {
                int length = article.Description.Length;

                if (length > _longMaxLength.Value)
                {
                    int excess = length - _longMaxLength.Value;
                    Debug.LogWarning($"[NewsImporter] ⚠️ \"{article.Headline}\" exceeds max length by {excess} characters.");
                }

                article.SizeCategory = (length <= _shortMaxLength.Value)
                    ? _shortCategoryValue.Value
                    : (length <= _mediumMaxLength.Value)
                        ? _mediumCategoryValue.Value
                        : _longCategoryValue.Value;
            }
        }

        /// <summary>
        /// Adds all articles to the specified article database.
        /// </summary>
        private void AddArticlesToDatabase(ArticleDatabase db, List<Article> articles)
        {
            db.Items.Clear(); // Optional: clear old data
            foreach (var article in articles)
            {
                db.Add(article);
            }
        }

        #endregion
    }
}
