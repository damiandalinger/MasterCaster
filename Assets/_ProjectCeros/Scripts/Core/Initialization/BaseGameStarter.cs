/// <summary>
/// Base class for starting a new game session by loading required scenes and instantiating managers.
/// </summary>

/// <remarks>
/// 27/05/2025 by Damian Dalinger: Script creation.
/// </remarks>

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace ProjectCeros
{
    public abstract class BaseGameStarter : MonoBehaviour
    {
        #region Fields

        [Tooltip("Prefabs to instantiate. Must not exist in the scene already.")]
        [SerializeField] protected List<GameObject> _persistentPrefabs = new();

        [Tooltip("Names of scenes to load additively during game start.")]
        [SerializeField] protected List<string> _sceneNamesToLoad = new();

        [Tooltip("Names of scenes to unload after game start is complete.")]
        [SerializeField] protected List<string> _sceneNamesToUnload = new();

        #endregion

        #region Lifecycle Methods

        private void Start()
        {
            StartCoroutine(StartSequenceRoutine());
        }

        #endregion

        #region Private Methods

        // Main sequence for loading scenes, instantiating prefabs, and triggering custom logic.
        private IEnumerator StartSequenceRoutine()
        {
            LoadingScreenManager.Instance.Show();
            yield return null;

            ResetInitialGameState();

            yield return LoadAdditiveScenes();

            InstantiateManagers();

            yield return ManagerInitialization();

            yield return UnloadAdditiveScenes();

            RaiseFinishedEvent();

            LoadingScreenManager.Instance.HideAndDestroy();

            Destroy(gameObject);
        }

        // Loads each listed scene additively if it's not already loaded.
        private IEnumerator LoadAdditiveScenes()
        {
            foreach (var sceneName in _sceneNamesToLoad)
            {
                if (SceneManager.GetSceneByName(sceneName).isLoaded)
                    continue;

                var op = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
                while (!op.isDone)
                    yield return null;
            }
        }

        // Unloads each listed scene if it is currently loaded and valid.
        private IEnumerator UnloadAdditiveScenes()
        {
            foreach (var sceneName in _sceneNamesToUnload)
            {
                var scene = SceneManager.GetSceneByName(sceneName);
                if (!scene.IsValid() || !scene.isLoaded)
                    continue;

                var op = SceneManager.UnloadSceneAsync(scene);
                while (!op.isDone)
                    yield return null;
            }
        }

        // Instantiates persistent managers only if they are not already present in the scene.
        private void InstantiateManagers()
        {
            foreach (var prefab in _persistentPrefabs)
            {
                if (prefab == null) continue;

                var type = prefab.GetComponent<MonoBehaviour>()?.GetType();
                if (type == null || FindFirstObjectByType(type) != null)
                    continue;

                var instance = Instantiate(prefab);
                instance.name = prefab.name;
            }
        }

        #endregion

        #region Abstract Methods

        // Implement in subclasses
        protected abstract void ResetInitialGameState();
        protected abstract IEnumerator ManagerInitialization();
        protected abstract void RaiseFinishedEvent();

        #endregion
    }
}
