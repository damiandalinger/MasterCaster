/// <summary>
/// Handles UI tab switching logic. Supports optional next-button navigation and custom tab visuals.
/// </summary>

/// <remarks>
/// 25/06/2025 by Damian Dalinger: Script Creation.
/// 27/06/2025 by Damian Dalinger: Refactored for generic usage.
/// </remarks>

using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ProjectCeros
{
    public class TabController : MonoBehaviour
    {
        #region Fields

        [Tooltip("All tab panels to switch between.")]
        [SerializeField] private List<GameObject> _tabs;

        [Tooltip("Buttons that correspond to the tab index.")]
        [SerializeField] private List<Button> _tabButtons;

        [Tooltip("Sprite used for unselected tab buttons.")]
        [SerializeField] private Sprite _normalSprite;

        [Tooltip("Sprite used for selected tab buttons.")]
        [SerializeField] private Sprite _selectedSprite;

        [Header("Next Button (optional)")]
        [Tooltip("Optional next/close button for linear navigation.")]
        [SerializeField] private Button _nextButton;

        [Tooltip("Optional GameEvent to raise after the last tab.")]
        [SerializeField] private GameEvent _onTabsFinished;

        private int _currentTab = 0;
        private bool _useNextButton => _nextButton != null;

        #endregion

        #region Lifecycle Methods

        private void Start()
        {
            ShowTab(_currentTab);
            InitializeTabButtons();

            if (_useNextButton)
            {
                _nextButton.onClick.AddListener(OnNextClicked);
            }
        }

        #endregion

        #region Private Methods

        private void InitializeTabButtons()
        {
            for (int i = 0; i < _tabButtons.Count; i++)
            {
                int index = i;
                _tabButtons[i].onClick.AddListener(() => ShowTab(index));
            }
        }

        // Shows a specific tab and updates button visuals and next-button state.
        private void ShowTab(int index)
        {
            _currentTab = Mathf.Clamp(index, 0, _tabs.Count - 1);

            for (int i = 0; i < _tabs.Count; i++)
            {
                _tabs[i].SetActive(i == _currentTab);
            }

            UpdateTabButtonStates();
        }

        // Attempts to advance to the next tab unless an interruptible animation is still active.
        private void OnNextClicked()
        {
            GameObject activeTab = _tabs[_currentTab];

            var interruptibles = activeTab.GetComponentsInChildren<ITabInterruptible>(true);
            foreach (var i in interruptibles)
            {
                if (i.IsBusy)
                {
                    i.SkipToEnd();
                    return; 
                }
            }
            
            if (_currentTab < _tabs.Count - 1)
            {
                ShowTab(_currentTab + 1);
            }
            else if (_onTabsFinished != null)
            {
                _onTabsFinished.Raise();
            }
        }

        // Updates visual state of all tab buttons.
        private void UpdateTabButtonStates()
        {
            for (int i = 0; i < _tabButtons.Count; i++)
            {
                var image = _tabButtons[i].GetComponent<Image>();
                if (image != null)
                {
                    image.sprite = (i == _currentTab) ? _selectedSprite : _normalSprite;
                }

                _tabButtons[i].interactable = (i != _currentTab);
            }
        }

        #endregion
    }
}
