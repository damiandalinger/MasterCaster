/// <summary>
/// Renders a newspaper layout using LayoutPresets and a list of block assignments.
/// Matches layout blocks by prefab name and instantiates them into the grid.
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
        #region Fields
        [Header("Grid Settings")]
        [Tooltip("Size of each grid cell in units.")]
        [SerializeField] private IntReference _cellSize;

        [Tooltip("Spacing between grid cells.")]
        [SerializeField] private IntReference _spacing;

        [Tooltip("Parent transform for all instantiated layout blocks.")]
        [SerializeField] private Transform _gridParent;
        #endregion

        #region Public Methods
        // Clears the current grid and renders the new layout based on block assignments.
        public void Render(List<BlockAssignment> assignments)
        {
            ClearGrid();

            foreach (var assignment in assignments)
            {
                if (assignment.Prefab == null)
                {
                    Debug.LogWarning("[NewsRenderer] Missing prefab in assignment.");
                    continue;
                }

                var instance = Instantiate(assignment.Prefab, _gridParent);
                SetBlockPosition(instance, assignment.Position);
                SetBlockTexts(instance, assignment.ArticleHeadline, assignment.ArticleDescription);
            }
        }
        #endregion

        #region Private Methods
        // Clears all children of the grid parent.
        private void ClearGrid()
        {
            foreach (Transform child in _gridParent)
            {
                Destroy(child.gameObject);
            }
        }

        // Sets the instantiated block's local position based on grid coordinates.
        private void SetBlockPosition(GameObject block, Vector2Int gridPosition)
        {
            float posX = gridPosition.x * (_cellSize + _spacing);
            float posY = gridPosition.y * (_cellSize + _spacing);
            block.transform.localPosition = new Vector3(posX, posY, 0);
        }

        // Sets the headline and description texts inside the block prefab.
        private void SetBlockTexts(GameObject block, string headlineTitle, string headlineDescription)
        {
            var titleText = block.transform.Find("Headline")?.GetComponent<TMPro.TextMeshProUGUI>();
            var descriptionText = block.transform.Find("Description")?.GetComponent<TMPro.TextMeshProUGUI>();

            if (titleText != null) titleText.text = headlineTitle;
            if (descriptionText != null) descriptionText.text = headlineDescription;
        }
        #endregion
    }
}