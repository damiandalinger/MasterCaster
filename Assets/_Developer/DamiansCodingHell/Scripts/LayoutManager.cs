/// <summary>
/// Manages building the newspaper layout by coordinating the NewsSelector and NewsGridRenderer.
/// Chooses a LayoutPreset based on the number of important headlines selected.
/// </summary>

/// <remarks>
/// 25/04/2025 by Damian Dalinger: Script Creation.
/// </remarks>

using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace ProjectCeros
{

    public class LayoutManager : MonoBehaviour
    {
        [Header("All Available Layout Presets")]
        [SerializeField] private List<LayoutPreset> _layoutPresets;

        [Header("Block Size Settings")]
        [SerializeField] private IntReference _shortBlockAreaLimit;
        [SerializeField] private IntReference _mediumBlockAreaLimit;

        private NewsSelector _selector;
        private NewsGridRenderer _renderer;

        private void Awake()
        {
            _selector = GetComponent<NewsSelector>();
            _renderer = GetComponent<NewsGridRenderer>();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                BuildLayout();
            }
        }

        public void BuildLayout()
        {
            _selector.SelectNews();

            var importantHeadlines = _selector.selectedImportant;
            var randomHeadlines = _selector.selectedRandom;

            LayoutPreset chosenPreset = FindSuitablePreset(importantHeadlines);

            if (chosenPreset == null)
            {
                Debug.LogError("[LayoutManager] ❌ No suitable preset found!");
                return;
            }

            Debug.Log($"[LayoutManager] ✅ Preset {chosenPreset.name} selected!");

            List<BlockAssignment> assignments = AssignBlocks(chosenPreset, importantHeadlines, randomHeadlines);

            _renderer.Render(assignments);
        }

        private LayoutPreset FindSuitablePreset(List<ClassifiedHeadline> importantHeadlines)
        {
            int requiredShort = importantHeadlines.Count(h => h.sizeCategory == "short");
            int requiredMedium = importantHeadlines.Count(h => h.sizeCategory == "medium");
            int requiredLong = importantHeadlines.Count(h => h.sizeCategory == "long");

            var shuffledPresets = _layoutPresets.OrderBy(x => Random.value).ToList();

            foreach (var preset in shuffledPresets)
            {
                var importantBlocks = preset.blocks.Where(b => b.isForImportantNews).ToList();

                int availableShort = importantBlocks.Count(b => b.GetSizeCategory(_shortBlockAreaLimit, _mediumBlockAreaLimit) == "short");
                int availableMedium = importantBlocks.Count(b => b.GetSizeCategory(_shortBlockAreaLimit, _mediumBlockAreaLimit) == "medium");
                int availableLong = importantBlocks.Count(b => b.GetSizeCategory(_shortBlockAreaLimit, _mediumBlockAreaLimit) == "long");

                if (availableShort >= requiredShort && availableMedium >= requiredMedium && availableLong >= requiredLong)
                    return preset;
            }

            return null;
        }

        private List<BlockAssignment> AssignBlocks(LayoutPreset preset, List<ClassifiedHeadline> important, List<ClassifiedHeadline> random)
        {
            List<BlockAssignment> result = new List<BlockAssignment>();

            // Wichtige Headlines nach Kategorie sortieren
            var longImportant = important.Where(h => h.sizeCategory == "long").ToList();
            var mediumImportant = important.Where(h => h.sizeCategory == "medium").ToList();
            var shortImportant = important.Where(h => h.sizeCategory == "short").ToList();

            // Random Headlines nach Kategorie sortieren
            var longRandom = random.Where(h => h.sizeCategory == "long").ToList();
            var mediumRandom = random.Where(h => h.sizeCategory == "medium").ToList();
            var shortRandom = random.Where(h => h.sizeCategory == "short").ToList();

            var blocks = preset.blocks.OrderByDescending(b => BlockCategoryOrder(b)).ToList();

            foreach (var block in blocks)
            {
                string category = block.GetSizeCategory(_shortBlockAreaLimit, _mediumBlockAreaLimit);

                if (block.isForImportantNews)
                {
                    var headline = GetMatchingHeadline(category, longImportant, mediumImportant, shortImportant);

                    if (headline == null)
                    {
                        Debug.LogError($"[LayoutManager] ❌ Could not assign important headline to block {block.sizeInput}");
                        continue;
                    }

                    result.Add(new BlockAssignment
                    {
                        block = block,
                        headlineTitle = headline.headline,
                        headlineDescription = headline.description,
                        isImportant = true
                    });
                }
                else
                {
                    var filler = GetMatchingHeadline(category, longRandom, mediumRandom, shortRandom);

                    if (filler == null)
                    {
                        Debug.LogWarning($"[LayoutManager] ⚠️ Could not assign random headline to block {block.sizeInput}");
                        result.Add(new BlockAssignment
                        {
                            block = block,
                            headlineTitle = "[No Random]",
                            headlineDescription = "",
                            isImportant = false
                        });
                    }
                    else
                    {
                        result.Add(new BlockAssignment
                        {
                            block = block,
                            headlineTitle = filler.headline,
                            headlineDescription = filler.description,
                            isImportant = false
                        });
                    }
                }
            }

            return result;
        }

        private ClassifiedHeadline GetMatchingHeadline(string blockCategory, List<ClassifiedHeadline> longList, List<ClassifiedHeadline> mediumList, List<ClassifiedHeadline> shortList)
        {
            if (blockCategory == "long")
            {
                if (longList.Count > 0)
                {
                    var h = longList[0];
                    longList.RemoveAt(0);
                    return h;
                }
                else if (mediumList.Count > 0)
                {
                    var h = mediumList[0];
                    mediumList.RemoveAt(0);
                    return h;
                }
                else if (shortList.Count > 0)
                {
                    var h = shortList[0];
                    shortList.RemoveAt(0);
                    return h;
                }
            }
            else if (blockCategory == "medium")
            {
                if (mediumList.Count > 0)
                {
                    var h = mediumList[0];
                    mediumList.RemoveAt(0);
                    return h;
                }
                else if (shortList.Count > 0)
                {
                    var h = shortList[0];
                    shortList.RemoveAt(0);
                    return h;
                }
            }
            else if (blockCategory == "short")
            {
                if (shortList.Count > 0)
                {
                    var h = shortList[0];
                    shortList.RemoveAt(0);
                    return h;
                }
            }

            return null;
        }

        private int BlockCategoryOrder(LayoutBlock block)
        {
            string category = block.GetSizeCategory(_shortBlockAreaLimit, _mediumBlockAreaLimit);
            return category == "long" ? 3 : category == "medium" ? 2 : 1;
        }
    }
}