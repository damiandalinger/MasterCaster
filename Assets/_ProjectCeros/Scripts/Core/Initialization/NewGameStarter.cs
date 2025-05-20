/// <summary>
/// Handles the startup sequence of a new game session: loading scenes, instantiating managers,
/// initializing systems, and then cleaning up temporary scenes.
/// </summary>

/// <remarks>
/// 13/05/2025 by Damian Dalinger: Script creation.
/// </remarks>

using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;


#if UNITY_EDITOR
using UnityEditor;
#endif

namespace ProjectCeros
{
    public class NewGameStarter : MonoBehaviour
    {
        #region Fields

        [Tooltip("The loading screen to show during initialization.")]
        [SerializeField] private GameObject _loadingScreen;

        [Tooltip("Prefabs to instantiate. Must not exist in the scene already.")]
        [SerializeField] private List<GameObject> _persistentPrefabs = new();

        [Tooltip("Event raised after the game is fully initialized.")]
        [SerializeField] private GameEvent _onNewGameStarted;

        [Tooltip("Names of scenes to load additively during game start.")]
        [SerializeField] private List<string> _sceneNamesToLoad = new();

        [Tooltip("Names of scenes to unload after game start is complete.")]
        [SerializeField] private List<string> _sceneNamesToUnload = new();

        [Header("Values")]
        [Tooltip("The IntVariable of the listener count.")]
        [SerializeField] private IntVariable _listenerCount;

        [Tooltip("The IntVariable of the day count.")]
        [SerializeField] private IntVariable _currentDay;

        #endregion

        #region Public Methods

        // Starts a new game session.
        public async void StartNewGame()
        {
            _loadingScreen?.SetActive(true);

            // Delay to ensure UI updates.
            await Task.Yield();

            _listenerCount.SetValue(0);
            _currentDay.SetValue(1);

            await LoadAdditiveScenes();

            InstantiateManagers();

            await InitializeManagers();

            await UnloadAdditiveScenes();

            _onNewGameStarted?.Raise();

            _loadingScreen?.SetActive(false);
        }

        #endregion

        #region Private Methods

        // Instantiates required manager prefabs and ensures only one instance exists.
        private void InstantiateManagers()
        {
            foreach (var prefab in _persistentPrefabs)
            {
                if (prefab == null) continue;

                var prefabType = prefab.GetComponent<MonoBehaviour>().GetType();

                if (FindFirstObjectByType(prefabType) != null)
                    continue;

                var instance = Instantiate(prefab);
                instance.name = prefab.name;
                DontDestroyOnLoad(instance);
            }
        }

        // Calls setup methods on critical systems required for a new game session.
        private async Task InitializeManagers()
        {
            await Task.Yield();

            var importer = FindFirstObjectByType<NewsImporter>();
            importer?.ImportAll();
            if (importer != null)
                Destroy(importer.gameObject);

            var reshuffler = FindFirstObjectByType<NewsDatabaseReshuffler>();
            reshuffler?.RefreshAllPools();
        }

        // Loads all configured scenes additively if they are not already loaded.
        private async Task LoadAdditiveScenes()
        {
            foreach (var sceneName in _sceneNamesToLoad)
            {
                if (SceneManager.GetSceneByName(sceneName).isLoaded)
                    continue;

                var op = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
                while (!op.isDone)
                    await Task.Yield();
            }
        }

        // Unloads all configured additive scenes if they are currently loaded.
        private async Task UnloadAdditiveScenes()
        {
            foreach (var sceneName in _sceneNamesToUnload)
            {
                var scene = SceneManager.GetSceneByName(sceneName);
                if (!scene.IsValid() || !scene.isLoaded)
                    continue;

                var op = SceneManager.UnloadSceneAsync(scene);
                while (!op.isDone)
                    await Task.Yield();
            }
        }

        #endregion
    }
}