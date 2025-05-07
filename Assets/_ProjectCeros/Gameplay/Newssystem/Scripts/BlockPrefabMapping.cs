/// <summary>
/// Maps available prefab blocks to specific size, agency, and importance configurations.
/// Used to instantiate the correct layout element for each newspaper article.
/// </summary>

/// <remarks>
/// 06/05/2025 by Damian Dalinger: Script creation.
/// </remarks>

using System.Collections.Generic;
using UnityEngine;

namespace ProjectCeros
{

    [CreateAssetMenu(menuName = "News/Block Prefab Mapping")]
    public class BlockPrefabMapping : ScriptableObject
    {
        [Tooltip("All available block prefab entries used in the newspaper layout.")]
        [SerializeField] private List<BlockPrefabEntry> _prefabEntries = new();

        // Returns a matching prefab based on layout size, agency affiliation, and importance.
        public GameObject GetPrefab(Vector2Int size, int agencyID, bool isImportant)
        {
            foreach (var entry in _prefabEntries)
            {
                if (entry == null || entry.Prefab == null) continue;

                if (entry.AgencyID == agencyID &&
                    entry.IsImportant == isImportant &&
                    entry.GetSize() == size)
                {
                    return entry.Prefab;
                }
            }

            Debug.LogWarning($"[BlockPrefabMapping] No match for size {size}, agency {agencyID}, important: {isImportant}");
            return null;
        }
    }
}