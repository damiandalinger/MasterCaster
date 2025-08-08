/// <summary>
/// Defines metadata for a layout block prefab, including size, agency, and importance.
/// </summary>

/// <remarks>
/// 06/05/2025 by Damian Dalinger: Script creation.
/// 03/06/2025 by Damian Dalinger: Changed from GameObject to a list.
/// </remarks>

using System.Collections.Generic;
using UnityEngine;

namespace ProjectCeros
{

    [System.Serializable]
    public class BlockPrefabEntry
    {
        #region Fields

        [Tooltip("All prefabs for this configuration (e.g. visual variations for 2x1 blocks).")]
        public List<GameObject> Prefabs = new();

        [Tooltip("Which agency this prefab belongs to (0 = Agency A, 1 = Agency B, 2 = Fruit of the day, 3 = Random).")]
        public int AgencyID;

        [Tooltip("Whether this block is for important news.")]
        public bool IsImportant;

        #endregion

        // Parses the prefab name to determine its layout size (e.g. '2x1').
        public Vector2Int GetSize()
        {
            if (Prefabs == null || Prefabs.Count == 0 || Prefabs[0] == null)
                return Vector2Int.zero;

            string[] parts = Prefabs[0].name.Split('x');
            if (parts.Length == 2 &&
                int.TryParse(parts[0], out int x) &&
                int.TryParse(parts[1], out int y))
            {
                return new Vector2Int(x, y);
            }
            return Vector2Int.zero;
        }
    }
}