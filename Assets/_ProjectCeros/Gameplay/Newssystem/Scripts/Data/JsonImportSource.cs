/// <summary>
/// Defines a source for importing article data, consisting of a JSON file and a target article database.
/// </summary>

/// <remarks>
/// 28/04/2025 by Damian Dalinger: Script creation.
/// </remarks>

using UnityEngine;

namespace ProjectCeros
{
    [System.Serializable]
    public class JsonImportSource
    {
        #region Fields

        [Tooltip("The JSON file containing serialized article data.")]
        public TextAsset Json;

        [Tooltip("The article database that will be filled with the parsed and classified articles.")]
        public ArticleDatabase TargetDatabase;

        #endregion
    }
}