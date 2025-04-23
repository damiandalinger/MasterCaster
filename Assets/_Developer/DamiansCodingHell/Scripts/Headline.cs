using UnityEngine;

namespace ProjectCeros
{

    [CreateAssetMenu(menuName = "News/Headline")]
    public class Headline : ScriptableObject
    {
        public string headlineText;
        public bool isImportant;
    }

}