/// <summary>
/// Selects a set of important and random headlines based on available StringVariables.
/// The selected lists are later used to populate the newspaper grid.
/// </summary>

/// <remarks>
/// 25/04/2025 by Damian Dalinger: Script Creation.
/// </remarks>

using UnityEngine;
using System.Collections.Generic;
using System.Linq;

namespace ProjectCeros
{
    public class NewsSelector : MonoBehaviour
    {
        [HideInInspector] public List<ClassifiedHeadline> selectedImportant;
        [HideInInspector] public List<ClassifiedHeadline> selectedRandom;

        [SerializeField] private IntReference _importantCount;
        [SerializeField] private List<NewsVariable> _importantHeadlines;
        [SerializeField] private List<NewsVariable> _randomHeadlines;

        [Header("Headline Length Settings")]
        [SerializeField] private IntReference _shortMaxHeadlineLength;
        [SerializeField] private IntReference _mediumMaxHeadlineLength;

        public void SelectNews()
        {
            var rawImportant = _importantHeadlines.OrderBy(x => Random.value).Take(_importantCount).ToList();
            selectedImportant = rawImportant.Select(h => new ClassifiedHeadline
            {
                headline = h.headline,
                description = h.description,
                sizeCategory = ClassifyDescription(h.description)
            }).ToList();

            var rawRandom = _randomHeadlines.OrderBy(x => Random.value).ToList();
            selectedRandom = rawRandom.Select(h => new ClassifiedHeadline
            {
                headline = h.headline,
                description = h.description,
                sizeCategory = ClassifyDescription(h.description)
            }).ToList();
        }

        private string ClassifyDescription(string description)
        {
            int length = description.Length;
            if (length <= _shortMaxHeadlineLength.Value) return "short";
            if (length <= _mediumMaxHeadlineLength.Value) return "medium";
            return "long";
        }
    }
}
