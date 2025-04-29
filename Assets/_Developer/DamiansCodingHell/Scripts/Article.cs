/// <summary>
/// Represents a full article including headline, description, and size category.
/// Used for creating newspaper layouts.
/// </summary>

/// <remarks>
/// 29/04/2025 by Damian Dalinger: Initial creation.
/// </remarks>

namespace ProjectCeros
{
    [System.Serializable]
    public class Article
    {
        public string Headline;
        public string Description;
        public int SizeCategory;
    }
}