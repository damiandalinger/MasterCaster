using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace ProjectCeros
{
    [System.Serializable]
    public class BlockPrefabEntry
    {
        [Tooltip("Visual prefab, must be named like '2x1' or '1x3'.")]
        public GameObject Prefab;

        [Tooltip("Which agency this prefab belongs to (0 = Agency A, 1 = Agency B, 2 = Random).")]
        public int AgencyID;

        [Tooltip("Whether this block is for important news.")]
        public bool IsImportant;

        public Vector2Int GetSize()
        {
            if (Prefab == null)
                return Vector2Int.zero;

            string[] parts = Prefab.name.Split('x');
            if (parts.Length == 2 &&
                int.TryParse(parts[0], out int x) &&
                int.TryParse(parts[1], out int y))
            {
                return new Vector2Int(x, y);
            }

            Debug.LogWarning($"[BlockPrefabEntry] Could not parse size from prefab name '{Prefab.name}'. Expected format: '2x1', '1x2', etc.");
            return Vector2Int.zero;
        }
    }

    [CreateAssetMenu(menuName = "News/Block Prefab Library")]
    public class BlockPrefabMapping : ScriptableObject
    {
        public List<BlockPrefabEntry> _entries = new();

        public GameObject GetPrefab(Vector2Int size, int agencyID, bool isImportant)
        {
            foreach (var entry in _entries)
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