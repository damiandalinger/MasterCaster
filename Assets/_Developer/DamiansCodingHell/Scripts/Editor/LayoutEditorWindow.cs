/// <summary>
/// A custom editor window to visually create and save LayoutPresets.
/// </summary>

/// <remarks>
/// 28/04/2025 by Damian Dalinger: Script creation.
/// 29/04/2025 by Damian Dalinger: Refactoring based on Tech Bible standards and UI improvements.
/// </remarks>

/*using UnityEditor;
using UnityEngine;
using System.Collections.Generic;

namespace ProjectCeros
{
    public class LayoutEditorWindow : EditorWindow
    {
        #region Fields
        [Header("Grid Settings")]
        private int _gridWidth = 6;
        private int _gridHeight = 3;
        private int _cellSize = 40;
        private int _spacing = 4;

        [Header("Preset Settings")]
        private string _presetName = "NewLayoutPreset";

        private Vector2Int _selectedSize = new Vector2Int(1, 1);
        private List<LayoutBlock> _placedBlocks = new();
        #endregion

        [MenuItem("Tools/News Layout Creator")]
        public static void ShowWindow()
        {
            GetWindow<LayoutEditorWindow>("News Layout Creator");
        }

        private void OnGUI()
        {
            DrawSettings();
            DrawBlockSelection();
            DrawGrid();
            DrawSaveButton();
        }

        #region Drawing Methods
        // Draws the basic settings UI like preset name and grid dimensions.
        private void DrawSettings()
        {
            GUILayout.Label("Layout Preset Settings", EditorStyles.boldLabel);

            GUILayout.BeginHorizontal();
            _presetName = EditorGUILayout.TextField("Preset Name", _presetName);
            if (GUILayout.Button("Clear Blocks")) _placedBlocks.Clear();
            GUILayout.EndHorizontal();

            _gridWidth = EditorGUILayout.IntField("Grid Width", _gridWidth);
            _gridHeight = EditorGUILayout.IntField("Grid Height", _gridHeight);
            _cellSize = EditorGUILayout.IntSlider("Cell Size", _cellSize, 10, 100);
            _spacing = EditorGUILayout.IntSlider("Spacing", _spacing, 0, 20);
        }

        // Displays block size selection buttons (e.g., 1x1, 2x1).
        private void DrawBlockSelection()
        {
            GUILayout.Label("Select Block Size", EditorStyles.boldLabel);

            GUILayout.BeginHorizontal();
            if (GUILayout.Button("1x1")) _selectedSize = new Vector2Int(1, 1);
            if (GUILayout.Button("2x1")) _selectedSize = new Vector2Int(2, 1);
            if (GUILayout.Button("1x2")) _selectedSize = new Vector2Int(1, 2);
            if (GUILayout.Button("2x2")) _selectedSize = new Vector2Int(2, 2);
            if (GUILayout.Button("3x1")) _selectedSize = new Vector2Int(3, 1);
            if (GUILayout.Button("1x3")) _selectedSize = new Vector2Int(1, 3);
            GUILayout.EndHorizontal();
        }

        // Renders the editable grid and places blocks based on mouse interaction.
        // Left click to place block. Shift + Left click to place important block. Right click to delete.
        private void DrawGrid()
        {
            Rect gridRect = GUILayoutUtility.GetRect(_gridWidth * (_cellSize + _spacing), _gridHeight * (_cellSize + _spacing));
            Handles.BeginGUI();

            for (int x = 0; x < _gridWidth; x++)
            {
                for (int y = 0; y < _gridHeight; y++)
                {
                    var rect = new Rect(
                        gridRect.x + x * (_cellSize + _spacing),
                        gridRect.y + (_gridHeight - y - 1) * (_cellSize + _spacing),
                        _cellSize,
                        _cellSize
                    );

                    EditorGUI.DrawRect(rect, new Color(0.9f, 0.9f, 0.9f, 1f));

                    if (rect.Contains(Event.current.mousePosition))
                    {
                        if (Event.current.type == EventType.MouseDown)
                        {
                            if (Event.current.button == 0)
                            {
                                bool isImportant = Event.current.shift;
                                TryPlaceBlock(x, y, isImportant);
                                Event.current.Use();
                            }
                            else if (Event.current.button == 1)
                            {
                                TryRemoveBlock(x, y);
                                Event.current.Use();
                            }
                        }
                    }
                }
            }

            // Draw already placed blocks
            foreach (var block in _placedBlocks)
            {
                var size = block.GetSize();
                var pos = new Rect(
                    gridRect.x + block.Position.x * (_cellSize + _spacing),
                    gridRect.y + (_gridHeight - block.Position.y - size.y) * (_cellSize + _spacing),
                    size.x * (_cellSize + _spacing) - _spacing,
                    size.y * (_cellSize + _spacing) - _spacing
                );

                Color blockColor = block.IsImportantNews ? Color.red : Color.gray;
                EditorGUI.DrawRect(pos, blockColor);

                EditorGUI.LabelField(pos, block.SizeInput, new GUIStyle()
                {
                    alignment = TextAnchor.MiddleCenter,
                    normal = new GUIStyleState { textColor = Color.white }
                });
            }

            Handles.EndGUI();
        }

        private void DrawSaveButton()
        {
            GUILayout.Space(20);

            if (GUILayout.Button("Save Layout Preset"))
            {
                SavePreset();
            }
        }
        #endregion

        #region Logic Methods
        // Attempts to place a new block at the given grid position.
        // Prevents overlaps and out-of-bounds placement.
        private void TryPlaceBlock(int x, int y, bool isImportant)
        {
            if (x + _selectedSize.x > _gridWidth || y + _selectedSize.y > _gridHeight)
            {
                Debug.LogWarning("Cannot place block: Exceeds grid boundaries!");
                return;
            }

            foreach (var block in _placedBlocks)
            {
                var blockArea = new RectInt(block.Position.x, block.Position.y, block.GetSize().x, block.GetSize().y);
                var newArea = new RectInt(x, y, _selectedSize.x, _selectedSize.y);

                if (blockArea.Overlaps(newArea))
                {
                    Debug.LogWarning("Cannot place block: Overlaps existing block!");
                    return;
                }
            }

            _placedBlocks.Add(new LayoutBlock
            {
                Position = new Vector2Int(x, y),
                SizeInput = $"{_selectedSize.x}x{_selectedSize.y}",
                IsImportantNews = isImportant
            });
        }

        // Attempts to remove an existing block at the given grid position.
        private void TryRemoveBlock(int x, int y)
        {
            for (int i = _placedBlocks.Count - 1; i >= 0; i--)
            {
                var block = _placedBlocks[i];
                var area = new RectInt(block.Position.x, block.Position.y, block.GetSize().x, block.GetSize().y);

                if (area.Contains(new Vector2Int(x, y)))
                {
                    _placedBlocks.RemoveAt(i);
                    break;
                }
            }
        }

        private void SavePreset()
        {
            var preset = ScriptableObject.CreateInstance<LayoutPreset>();
            preset.Blocks = new List<LayoutBlock>(_placedBlocks);

            string path = $"Assets/{_presetName}.asset";
            AssetDatabase.CreateAsset(preset, path);
            AssetDatabase.SaveAssets();

            Debug.Log($"[LayoutEditorWindow] LayoutPreset saved at: {path}");
            _placedBlocks.Clear();
        }
        #endregion
    }
}*/