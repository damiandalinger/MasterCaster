/// <summary>
/// Renders a newspaper layout using LayoutPresets and a list of block assignments.
/// Matches layout blocks by prefab name and instantiates them into the grid.
/// The system positions prefabs relative to the grid parent, starting from bottom left (0,0).
/// </summary>

/// <remarks>
/// 25/04/2025 by Damian Dalinger: Script Creation.
/// 07/05/2025 by Damian Dalinger: Refactoring.
/// 03/06/2025 by Damian Dalinger: Implemented the background sprite function.
/// </remarks>

using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

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

        [Header("Background Sprites")]
        [Tooltip("The sprites must have the exact same name as they have in the JSONs.")]
        [SerializeField] private List<Sprite> _backgroundSprites = new();
        private Dictionary<string, Sprite> _backgroundLookup;

        #endregion

        #region Lifecycle Methods

        // Builds the sprite lookup table based on sprite names.
        private void Awake()
        {
            _backgroundLookup = _backgroundSprites
                .Where(sprite => sprite != null)
                .GroupBy(sprite => sprite.name.ToLower())
                .ToDictionary(group => group.Key, group => group.First());
        }

        // Render the Newspaper as soon as the scene is loaded.
        private void Start()
        {
            RenderFromLayout();
        }

        #endregion

        #region Public Methods

        // Clears the current grid and renders the new layout based on block assignments.
        private void Render(List<BlockAssignment> assignments)
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
                if (assignment.UseCustomBackground)
                    SetBlockBackground(instance, assignment.ArticleBackgroundName);
            }
        }

        // Renders content using the current layout assignments from the active LayoutManager in the scene.
        [ContextMenu("DEBUG: Render Now")]
        public void RenderFromLayout()
        {
            var layoutManager = FindFirstObjectByType<LayoutManager>();
            if (layoutManager == null)
                return;

            var assignments = layoutManager.CurrentAssignments;

            if (assignments == null || assignments.Count == 0)
                return;

            Render(assignments);
        }

        #endregion

        #region Private Methods

        // Removes all children from the grid parent.
        private void ClearGrid()
        {
            foreach (Transform child in _gridParent)
            {
                Destroy(child.gameObject);
            }
        }

        // Sets the background of a layout block based on the article's BackgroundName.
        private void SetBlockBackground(GameObject block, string backgroundName)
        {
            if (string.IsNullOrEmpty(backgroundName)) return;

            var background = block.transform.Find("Background")?.GetComponent<Image>();
            if (background == null) return;

            string key = backgroundName.ToLower();
            if (_backgroundLookup.TryGetValue(key, out Sprite sprite))
            {
                background.sprite = sprite;
                background.enabled = true;
            }
        }

        // Sets the instantiated block's local position based on grid coordinates.
        private void SetBlockPosition(GameObject block, Vector2Int gridPosition)
        {
            float posX = gridPosition.x * (_cellSize + _spacing);
            float posY = gridPosition.y * (_cellSize + _spacing);
            block.transform.localPosition = new Vector3(posX, posY, 0f);
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