using System.Collections.Generic;
using UnityEngine;

namespace ProjectCeros
{

    [CreateAssetMenu(menuName = "News/HeadlinePool")]
    public class HeadlinePool : ScriptableObject
    {
        public List<Headline> importantHeadlines;
        public List<Headline> randomHeadlines;
    }
}