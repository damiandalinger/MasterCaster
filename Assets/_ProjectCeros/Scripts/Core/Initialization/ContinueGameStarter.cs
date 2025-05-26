/// <summary>
/// Handles the startup sequence for continuing a saved game session.
/// </summary>

/// <remarks>
/// 26/05/2025 by Damian Dalinger: Script creation.
/// </remarks>

using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace ProjectCeros
{
    public class ContinueGameStarter : MonoBehaviour
    {
        #region Fields

        [Tooltip("The loading screen to show during initialization.")]
        [SerializeField] private GameObject _loadingScreen;

        [Tooltip("Prefabs to instantiate. Must not exist in the scene already.")]
        [SerializeField] private List<GameObject> _persistentPrefabs = new();

        [Tooltip("Event raised after the game is fully initialized.")]
        [SerializeField] private GameEvent _onGameContinued;

        [Tooltip("Names of scenes to load additively during game start.")]
        [SerializeField] private List<string> _sceneNamesToLoad = new();

        [Tooltip("Names of scenes to unload after game start is complete.")]
        [SerializeField] private List<string> _sceneNamesToUnload = new();

        #endregion

        #region Public Methods

        // Continues a game session.
        public async void ContinueGame()
        {
            _loadingScreen?.SetActive(true);

            // Delay to ensure UI updates.
            await Task.Yield();

            await LoadAdditiveScenes();

            InstantiateManagers();

            FindFirstObjectByType<SaveManager>()?.Load();

            await UnloadAdditiveScenes();

            _onGameContinued?.Raise();

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
