/// <summary>
/// Manages a group of UI containers where only one is visible at a time.
/// Useful for toggling between panels or grouped button layouts.
/// </summary>

/// <remarks>
/// 25/06/2025 by Damian Dalinger: Initial creation.
/// 15/08/2025 by Damian Dalinger: Added the ability to save the current index and display it on Start.
/// </remarks>

using System.Collections.Generic;
using UnityEngine;

namespace ProjectCeros
{
    [System.Serializable]
    public class GameObjectGroup
    {
        [Tooltip("GameObjects in this group.")]
        public List<GameObject> groupObjects = new();
    }
    public class GameObjectSwitcher : MonoBehaviour
    {
        #region Fields

        [Tooltip("UI containers (e.g., panels or button groups). Only one is visible at a time.")]
        [SerializeField] private List<GameObjectGroup> _groups = new();

        [Tooltip("If enabled, the switcher will restore the saved index on Start.")]
        [SerializeField] private bool _restoreOnStart = false;

        [Tooltip("Variable used to store the last shown group index.")]
        [SerializeField] private IntReference _currentIndexVariable;

        #endregion

        private void Start()
        {
            if (_restoreOnStart && _currentIndexVariable.Variable != null)
            {
                ShowGroup(_currentIndexVariable.Variable.RuntimeValue);
            }
        }

        // Displays the specified group and hides all others.
        public void ShowGroup(int index)
        {
            if (index < 0 || index >= _groups.Count)
            {
                Debug.LogWarning($"[GameObjectGroupSwitcher] Invalid group index: {index}");
                return;
            }

            for (int i = 0; i < _groups.Count; i++)
            {
                bool shouldShow = (i == index);
                foreach (var go in _groups[i].groupObjects)
                {
                    if (go != null)
                        go.SetActive(shouldShow);
                }
            }
            
            if (_currentIndexVariable.Variable != null)
            {
                _currentIndexVariable.Variable.SetValue(index);
            }

        }

        // Hides all GameObjects in all groups.
        public void HideAll()
        {
            foreach (var group in _groups)
            {
                foreach (var go in group.groupObjects)
                {
                    if (go != null)
                        go.SetActive(false);
                }
            }
        }
    }
}