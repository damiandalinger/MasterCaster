using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace ProjectCeros
{
   [CreateAssetMenu(menuName = "News/Block Prefab Mapping")]
public class BlockPrefabMapping : ScriptableObject
{
    public List<BlockPrefabEntry> entries;

    public GameObject GetPrefab(string sizeLabel)
    {
        return entries.FirstOrDefault(e => e.sizeLabel == sizeLabel)?.prefab;
    }
}
}
