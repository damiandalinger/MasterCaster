using UnityEngine;

namespace ProjectCeros
{
    [System.Serializable]
    public class GenrePoolPair
    {

        [Tooltip("Datenbank mit allen Artikeln dieses Genres.")]
        public ArticleDatabase AllArticles;

        [Tooltip("Datenbank mit aktuell freigeschalteten Artikeln.")]
        public ArticleDatabase EligibleArticles;

        public bool UsesPairs = true;
    }
}
