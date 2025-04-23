using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace ProjectCeros
{
    public class NewspaperGridRenderer : MonoBehaviour
    {
        public BlockPrefabMapping blockPrefabMapping;
        public Transform gridParent;

        public void RenderNewspaper(LayoutPreset layout, List<Headline> important, List<Headline> random)
        {
            foreach (Transform child in gridParent)
            {
                Destroy(child.gameObject);
            }

            int impIndex = 0;
            int randIndex = 0;

            List<LayoutBlock> shuffledBlocks = layout.blocks.OrderBy(x => Random.value).ToList();

            foreach (var block in shuffledBlocks)
            {
                Vector2Int size = block.GetSize();
                GameObject prefab = blockPrefabMapping.GetPrefab(block.sizeInput);

                if (prefab == null)
                {
                    Debug.LogWarning($"No prefab found for size {block.sizeInput}");
                    continue;
                }

                GameObject go = Instantiate(prefab, gridParent.transform);

                // Positionierung mit Spacing
                int cellSize = 100;
                int spacing = 10;

                float posX = block.position.x * (cellSize + spacing);
                float posY = -block.position.y * (cellSize + spacing); // Minus, damit Y nach unten geht

                go.transform.localPosition = new Vector3(posX, posY, 0);

                // Set Text
                var label = go.GetComponentInChildren<TMPro.TextMeshProUGUI>();
                if (block.isForImportantNews && impIndex < important.Count)
                {
                    label.text = important[impIndex++].headlineText;
                }
                else if (!block.isForImportantNews && randIndex < random.Count)
                {
                    label.text = random[randIndex++].headlineText;
                }
            }
        }
    }
}