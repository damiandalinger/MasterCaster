using UnityEngine;

namespace ProjectCeros
{
    [CreateAssetMenu(menuName = "News/NewsVariable")]
    public class NewsVariable : ScriptableObject
    {
        public string headline;
        public string description;
    }
}