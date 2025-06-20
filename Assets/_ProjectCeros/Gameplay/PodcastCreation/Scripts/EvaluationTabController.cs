using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ProjectCeros
{
    public class EvaluationTabController : MonoBehaviour
    {
        [Header("Tabs")]
        [SerializeField] private List<GameObject> _tabs;

        [Header("Tab Buttons (optional)")]
        [SerializeField] private List<Button> _tabButtons;
        [SerializeField] private Sprite _normalSprite;
        [SerializeField] private Sprite _selectedSprite;

        [Header("Next Button")]
        [SerializeField] private Button _nextButton;

        [Header("Evaluation UI Root")]
        [SerializeField] private GameObject _evaluationUIRoot;
        [SerializeField] private PodcastResultVisualizer _podcastVisualizer;

        private int _currentTab = 0;

        private void Start()
        {
            ShowTab(_currentTab);

            // Optional: Tab Buttons click listeners
            for (int i = 0; i < _tabButtons.Count; i++)
            {
                int index = i;
                _tabButtons[i].onClick.AddListener(() =>
                {
                    ShowTab(index);
                });
            }

            _nextButton.onClick.AddListener(OnNextClicked);
        }

        private void ShowTab(int index)
        {
            _currentTab = Mathf.Clamp(index, 0, _tabs.Count - 1);

            for (int i = 0; i < _tabs.Count; i++)
            {
                _tabs[i].SetActive(i == _currentTab);
            }

            // Optional: Update tab button visuals (e.g. highlight current)
            UpdateTabButtonStates();

            // Optional: Update button text or icon if needed
            UpdateNextButtonText();
        }

        private void OnNextClicked()
        {

            if (_currentTab < _tabs.Count - 1)
            {
                ShowTab(_currentTab + 1);
            }
            else
            {
                _evaluationUIRoot.SetActive(false);
            }
        }

        private void UpdateTabButtonStates()
        {
            for (int i = 0; i < _tabButtons.Count; i++)
            {
                var image = _tabButtons[i].GetComponent<Image>();
                if (image != null)
                {
                    image.sprite = (i == _currentTab) ? _selectedSprite : _normalSprite;
                }

                // Optional: Disable interaction for selected tab
                _tabButtons[i].interactable = (i != _currentTab);
            }
        }

        private void UpdateNextButtonText()
        {
            var text = _nextButton.GetComponentInChildren<TMPro.TMP_Text>();
            if (text != null)
                text.text = (_currentTab < _tabs.Count - 1) ? "Next" : "Close";
        }
    }
}
