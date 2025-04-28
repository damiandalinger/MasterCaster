#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using System.Linq;
using System.Collections.Generic;

namespace ProjectCeros
{
    public class LayoutEditorWindow : EditorWindow
    {
        private int gridWidth = 6;
        private int gridHeight = 3;
        private int cellSize = 40;
        private int spacing = 4;
        private Vector2Int selectedSize = new Vector2Int(1, 1);
        private bool isImportant = false;
        private List<LayoutBlock> placedBlocks = new();
        private string presetName = "NewLayoutPreset";

        [MenuItem("Tools/Visual Layout Editor")]
        public static void ShowWindow()
        {
            GetWindow<LayoutEditorWindow>("Layout Preset Editor");
        }

        private void OnGUI()
        {
            GUILayout.Label("Layout Editor", EditorStyles.boldLabel);

            GUILayout.BeginHorizontal();
            presetName = EditorGUILayout.TextField("Preset Name", presetName);
            if (GUILayout.Button("Clear")) placedBlocks.Clear();
            GUILayout.EndHorizontal();

            gridWidth = EditorGUILayout.IntField("Grid Width", gridWidth);
            gridHeight = EditorGUILayout.IntField("Grid Height", gridHeight);
            cellSize = EditorGUILayout.IntSlider("Cell Size", cellSize, 10, 100);
            spacing = EditorGUILayout.IntSlider("Spacing", spacing, 0, 20);

            string[] sizeOptions = new[] { "1x1", "2x1", "1x2", "2x2", "3x1", "1x3" };
            string selectedLabel = selectedSize.x + "x" + selectedSize.y;
            int index = Mathf.Max(0, System.Array.IndexOf(sizeOptions, selectedLabel));
            int newIndex = EditorGUILayout.Popup("Block Size", index, sizeOptions);
            if (newIndex != index)
            {
                var parts = sizeOptions[newIndex].Split('x');
                selectedSize = new Vector2Int(int.Parse(parts[0]), int.Parse(parts[1]));
            }

            isImportant = EditorGUILayout.Toggle("Is Important Block", isImportant);

            Rect gridRect = GUILayoutUtility.GetRect(gridWidth * (cellSize + spacing), gridHeight * (cellSize + spacing));

            Handles.BeginGUI();

            for (int x = 0; x < gridWidth; x++)
            {
                for (int y = 0; y < gridHeight; y++)
                {
                    var rect = new Rect(
                        gridRect.x + x * (cellSize + spacing),
                        gridRect.y + (gridHeight - y - 1) * (cellSize + spacing),
                        cellSize,
                        cellSize
                    );

                    EditorGUI.DrawRect(rect, new Color(0.9f, 0.9f, 0.9f, 1f));

                    // Platzieren oder Löschen
                    if (rect.Contains(Event.current.mousePosition))
                    {
                        if (Event.current.type == EventType.MouseDown)
                        {
                            if (Event.current.button == 0) // Linksklick = Platzieren
                            {
                                TryPlaceBlock(x, y);
                                Event.current.Use();
                            }
                            else if (Event.current.button == 1) // Rechtsklick = Entfernen
                            {
                                TryRemoveBlock(x, y);
                                Event.current.Use();
                            }
                        }
                    }
                }
            }

            // Blöcke zeichnen
            foreach (var block in placedBlocks)
            {
                var size = block.GetSize();
                var pos = new Rect(
                    gridRect.x + block.position.x * (cellSize + spacing),
                    gridRect.y + (gridHeight - block.position.y - size.y) * (cellSize + spacing),
                    size.x * (cellSize + spacing) - spacing,
                    size.y * (cellSize + spacing) - spacing
                );
                Color col = block.isForImportantNews ? Color.red : Color.gray;
                EditorGUI.DrawRect(pos, col);
                EditorGUI.LabelField(pos, block.sizeInput, new GUIStyle() { alignment = TextAnchor.MiddleCenter, normal = new GUIStyleState { textColor = Color.white } });
            }

            Handles.EndGUI();

            GUILayout.Space(20);
            if (GUILayout.Button("Save Preset"))
            {
                var preset = ScriptableObject.CreateInstance<LayoutPreset>();
                preset.blocks = new List<LayoutBlock>(placedBlocks);
                AssetDatabase.CreateAsset(preset, $"Assets/{presetName}.asset");
                AssetDatabase.SaveAssets();
                Debug.Log("LayoutPreset saved as " + presetName);
                placedBlocks.Clear();
            }
        }

        private void TryPlaceBlock(int x, int y)
        {
            // Überprüfen: Ragt der neue Block aus dem Grid raus?
            if (x + selectedSize.x > gridWidth || y + selectedSize.y > gridHeight)
            {
                Debug.LogWarning("Cannot place block: would exceed grid boundaries!");
                return;
            }

            // Überprüfen: Überlappt neuer Block mit bestehenden?
            foreach (var block in placedBlocks)
            {
                var blockSize = block.GetSize();
                var blockArea = new RectInt(block.position.x, block.position.y, blockSize.x, blockSize.y);

                var newArea = new RectInt(x, y, selectedSize.x, selectedSize.y);

                if (blockArea.Overlaps(newArea))
                {
                    Debug.LogWarning("Cannot place block: position already occupied!");
                    return;
                }
            }

            // Platzieren
            placedBlocks.Add(new LayoutBlock
            {
                position = new Vector2Int(x, y),
                sizeInput = selectedSize.x + "x" + selectedSize.y,
                isForImportantNews = isImportant
            });
        }

        private void TryRemoveBlock(int x, int y)
        {
            for (int i = placedBlocks.Count - 1; i >= 0; i--)
            {
                var block = placedBlocks[i];
                var blockSize = block.GetSize();
                var blockArea = new RectInt(block.position.x, block.position.y, blockSize.x, blockSize.y);

                if (blockArea.Contains(new Vector2Int(x, y)))
                {
                    placedBlocks.RemoveAt(i);
                    break;
                }
            }
        }
    }
}
#endif