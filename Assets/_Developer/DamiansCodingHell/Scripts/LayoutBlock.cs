/// <summary>
/// Represents a single block inside a LayoutPreset, including position, size and category importance.
/// </summary>

/// <remarks>
/// 25/04/2025 by Damian Dalinger: Initial creation.
/// </remarks>

using UnityEngine;

namespace ProjectCeros
{
    [System.Serializable]
    public class LayoutBlock
    {
        #region Fields
        [Tooltip("Position of the block in the grid (bottom-left origin).")]
        public Vector2Int Position;

        [Tooltip("Size format (e.g., '2x1', '1x1'). Must match prefab names.")]
        public string SizeInput = "1x1";

        [Tooltip("Whether this block is reserved for an important article.")]
        public bool IsImportantNews;
        #endregion

        #region Public Methods
        // Gets the size of the block as a Vector2Int.
        public Vector2Int GetSize()
        {
            var split = SizeInput.Split('x');
            int x = int.Parse(split[0]);
            int y = int.Parse(split[1]);
            return new Vector2Int(x, y);
        }
        #endregion
    }
}