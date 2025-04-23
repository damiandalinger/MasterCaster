using UnityEngine;

namespace ProjectCeros
{
    [System.Serializable]
    public class LayoutBlock
    {
        public Vector2Int position;
        public string sizeInput = "1x1"; // Editor-friendly input
        public bool isForImportantNews;

        public Vector2Int GetSize()
        {
            var split = sizeInput.Split('x');
            int x = int.Parse(split[0]);
            int y = int.Parse(split[1]);
            return new Vector2Int(x, y);
        }
    }
}