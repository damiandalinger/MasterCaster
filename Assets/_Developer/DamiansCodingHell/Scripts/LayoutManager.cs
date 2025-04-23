using System.Collections.Generic;
using UnityEngine;

namespace ProjectCeros
{

    public class LayoutManager : MonoBehaviour
    {
        public List<LayoutPreset> layoutPresets4;
        public List<LayoutPreset> layoutPresets5;
        public List<LayoutPreset> layoutPresets6;
        public NewsSelector selector;
        public NewspaperGridRenderer Vrenderer;

        public void BuildLayout()
        {
            selector.SelectNews();
            LayoutPreset chosenPreset = GetRandomPreset(selector.selectedImportant.Count);
            Vrenderer.RenderNewspaper(chosenPreset, selector.selectedImportant, selector.selectedRandom);
        }

        private LayoutPreset GetRandomPreset(int importantCount)
        {
            switch (importantCount)
            {
                case 4: return layoutPresets4[Random.Range(0, layoutPresets4.Count)];
                case 5: return layoutPresets5[Random.Range(0, layoutPresets5.Count)];
                case 6: return layoutPresets6[Random.Range(0, layoutPresets6.Count)];
                default: return layoutPresets5[0];
            }
        }
    }

}