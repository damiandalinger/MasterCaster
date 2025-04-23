using UnityEngine;
using System.Collections.Generic;
using System.Linq;

namespace ProjectCeros
{
    public class NewsSelector : MonoBehaviour
    {
        public HeadlinePool headlinePool;
        public int importantCount = 5;
        public List<Headline> selectedImportant;
        public List<Headline> selectedRandom;

        public void SelectNews()
        {
            selectedImportant = headlinePool.importantHeadlines.OrderBy(x => Random.value).Take(importantCount).ToList();
            selectedRandom = headlinePool.randomHeadlines.OrderBy(x => Random.value).ToList();
        }
    }
}
