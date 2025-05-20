/// <summary>
/// Boots up the game runtime by instantiating persistent managers and raising initialization events.
/// </summary>

/// <remarks>
/// 13/05/2025 by Damian Dalinger: Script creation.
/// </remarks>

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace ProjectCeros
{
    public class GameInitiator : MonoBehaviour
    {
        #region Fields

        [Tooltip("Prefabs that will be instantiated once and persist across scenes.")]
        [SerializeField] private List<GameObject> _persistentPrefabs = new();

        [Tooltip("Event raised after persistent managers have been initialized.")]
        [SerializeField] private GameEvent _onGameInitialized;

        #endregion

        #region Lifecycle Methods

        private void Start()
        {
            Application.runInBackground = true;

            InstantiatePersistentPrefabs();

            _onGameInitialized?.Raise();
        }

        #endregion

        #region Private Methods

        // Instantiates all configured persistent prefabs if not already present in the scene.
        private void InstantiatePersistentPrefabs()
        {
            foreach (var prefab in _persistentPrefabs)
            {
                if (prefab == null) continue;

                var prefabType = prefab.GetComponent<MonoBehaviour>()?.GetType();
                if (prefabType == null) continue;

                if (FindFirstObjectByType(prefabType) != null)
                    continue;

                var instance = Instantiate(prefab);
                instance.name = prefab.name;
                SceneManager.MoveGameObjectToScene(instance, gameObject.scene);
            }
        }

        #endregion
    }
}