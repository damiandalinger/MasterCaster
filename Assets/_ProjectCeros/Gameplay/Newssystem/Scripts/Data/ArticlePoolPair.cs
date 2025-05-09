/// <summary>
/// Represents a pair of article pools for a specific genre: all available articles and those currently eligible.
/// </summary>

/// <remarks>
/// 05/05/2025 by Damian Dalinger: Script creation.
/// </remarks>

using UnityEngine;

namespace ProjectCeros
{
    [System.Serializable]
    public class ArticlePoolPair
    {
        #region Fields

        [Tooltip("The database containing all articles.")]
        public ArticleDatabase AllArticles;

        [Tooltip("The database containing only the currently eligible (unlocked) articles.")]
        public ArticleDatabase EligibleArticles;

        [Tooltip("Determines whether this database uses pairs.")]
        public bool UsesPairs = true;

        #endregion
    }
}
