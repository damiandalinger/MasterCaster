/// <summary>
/// A ScriptableObject containing a list of LayoutBlocks, forming a full newspaper layout preset.
/// </summary>

/// <remarks>
/// 29/04/2025 by Damian Dalinger: Initial creation.
/// </remarks>

using System.Collections.Generic;
using UnityEngine;

namespace ProjectCeros
{

    [CreateAssetMenu(menuName = "News/LayoutPreset")]
    public class LayoutPreset : ScriptableObject
    {
        [Tooltip("List of blocks that make up the layout.")]
        public List<LayoutBlock> Blocks = new();
    }
}