/// <summary>
/// Represents an assignment of a specific article into a specific layout block.
/// </summary>

/// <remarks>
/// 25/04/2025 by Damian Dalinger: Initial creation.
/// </remarks>

namespace ProjectCeros
{
    [System.Serializable]
    public class BlockAssignment
    {
        #region Fields
        public LayoutBlock Block;
        public string ArticleHeadline;
        public string ArticleDescription;
        public bool IsImportant;
        #endregion
    }
}