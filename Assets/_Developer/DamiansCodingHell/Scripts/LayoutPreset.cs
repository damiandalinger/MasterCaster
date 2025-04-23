using System.Collections.Generic;
using UnityEngine;

namespace ProjectCeros
{

    [CreateAssetMenu(menuName = "News/LayoutPreset")]
    public class LayoutPreset : ScriptableObject
    {
        public List<LayoutBlock> blocks; // A layout is a collection of blocks
    }
}