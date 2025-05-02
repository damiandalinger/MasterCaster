using UnityEngine;

namespace ProjectCeros
{
    [System.Serializable]
    public class JsonImportSource
    {
        [Tooltip("The JSON file to import.")]
        public TextAsset json;

        [Tooltip("The database to fill with parsed and classified articles.")]
        public ArticleDatabase targetDatabase;
    }
}