/// <summary>
/// Manages a group of UI containers where only one is visible at a time.
/// Useful for toggling between panels or grouped button layouts.
/// </summary>

/// <remarks>
/// 25/06/2025 by Damian Dalinger: Initial creation.
/// </remarks>

using System.Collections.Generic;
using UnityEngine;

namespace ProjectCeros
{

    public class GameObjectSwitcher : MonoBehaviour
    {
        #region Fields

        [Tooltip("UI containers (e.g., panels or button groups). Only one is visible at a time.")]
        [SerializeField] private List<GameObject> _gameObjects = new();

        #endregion

        // Displays the specified group and hides all others.
        public void ShowGroup(int index)
        {
            if (index < 0 || index >= _gameObjects.Count)
            {
                Debug.LogWarning($"[GameObjectSwitcher] Invalid group index: {index}");
                return;
            }

            for (int i = 0; i < _gameObjects.Count; i++)
            {
                _gameObjects[i].SetActive(i == index);
            }
        }

        // Deactivates all UI groups.
        public void HideAll()
        {
            foreach (var group in _gameObjects)
                group.SetActive(false);
        }
    }
}
