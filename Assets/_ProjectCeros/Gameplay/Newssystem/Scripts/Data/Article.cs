/// <summary>
/// Represents a full article.
/// Used for creating newspaper layouts.
/// </summary>

/// <remarks>
/// 29/04/2025 by Damian Dalinger: Initial creation.
/// 05/05/2025 by Damian Dalinger: Added the story variables.
/// </remarks>

namespace ProjectCeros
{
    [System.Serializable]
    public class Article
    {
        public int PairID;
        public string Headline;
        public string Description;
        public int AgencyID;
        public string Subgenre;
        public float ValuePositive;
        public float ValueNegative;
        public int SizeCategory;
        public int StoryID = 0;
        public int StoryPart = 0;     
        public string BackgroundName;
    }
}