/// <summary>
/// Renders a newspaper layout based on a LayoutPreset and two lists of headlines (important and random).
/// LayoutBlocks must match the block size (e.g., "2x1", "1x1") exactly by name.
/// The system positions prefabs relative to the grid parent, starting from bottom left (0,0).
/// </summary>

/// <remarks>
/// 25/04/2025 by Damian Dalinger: Script Creation.
/// </remarks>

using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace ProjectCeros
{
    public class NewsGridRenderer : MonoBehaviour
    {
        [SerializeField] private IntReference _cellSize;
        [SerializeField] private IntReference _spacing;
        [SerializeField] private Transform _gridParent;
        [SerializeField] private List<GameObject> _layoutBlocks;

        private Dictionary<string, GameObject> _prefabLookup;

        private void Awake()
        {
            _prefabLookup = _layoutBlocks.ToDictionary(p => p.name);
        }

        public void Render(List<BlockAssignment> assignments)
        {
            foreach (Transform child in _gridParent)
            {
                Destroy(child.gameObject);
            }

            foreach (var assignment in assignments)
            {
                if (!_prefabLookup.TryGetValue(assignment.block.sizeInput, out GameObject prefab))
                {
                    Debug.LogWarning($"[NewsGridRenderer] ❌ Missing prefab for size {assignment.block.sizeInput}");
                    continue;
                }

                var instance = Instantiate(prefab, _gridParent);

                float posX = assignment.block.position.x * (_cellSize + _spacing);
                float posY = assignment.block.position.y * (_cellSize + _spacing);
                instance.transform.localPosition = new Vector3(posX, posY, 0);

                // SET BOTH TEXTS
                var titleText = instance.transform.Find("Headline").GetComponent<TMPro.TextMeshProUGUI>();
                var descriptionText = instance.transform.Find("Description").GetComponent<TMPro.TextMeshProUGUI>();

                if (titleText != null) titleText.text = assignment.headlineTitle;
                if (descriptionText != null) descriptionText.text = assignment.headlineDescription;
            }
        }
    }
}