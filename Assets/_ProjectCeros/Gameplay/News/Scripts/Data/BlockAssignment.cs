/// <summary>
/// Represents an assignment of a specific article into a specific layout block.
/// </summary>

/// <remarks>
/// 25/04/2025 by Damian Dalinger: Initial creation.
/// 03/06/2025 by Damian Dalinger: Added ArticleBackgroundName and UseCustomBackground.
/// </remarks>

using UnityEngine;

namespace ProjectCeros
{
    [System.Serializable]
    public class BlockAssignment
    {
        public GameObject Prefab;
        public Vector2Int Position;
        public string ArticleHeadline;
        public string ArticleDescription;
        public string ArticleBackgroundName;
        public bool UseCustomBackground;
    }
}