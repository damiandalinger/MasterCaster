using TMPro;
using UnityEngine;

namespace ProjectCeros
{
    public class LatestPodcastTitleDisplay : MonoBehaviour
    {
        [SerializeField] private StringRuntimeSet _podcastTitles;
        [SerializeField] private TMP_Text _titleText;

        private void OnEnable()
        {
            if (_podcastTitles == null || _podcastTitles.Items.Count == 0)
            {
                _titleText.text = "No Title";
                return;
            }

            string latest = _podcastTitles.Items[_podcastTitles.Items.Count - 1];
            _titleText.text = latest;
        }
    }
}
