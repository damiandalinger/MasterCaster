/// <summary>
/// Saves the game state and returns to the main menu by loading the menu scene and unloading gameplay scenes.
/// </summary>

/// <remarks>
/// 26/05/2025 by Damian Dalinger: Initial creation.
/// </remarks>

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace ProjectCeros
{
    public class BackToMenuHandler : MonoBehaviour
    {
        #region Fields

        [Tooltip("The loading screen to show during initialization.")]
        [SerializeField] private GameObject _loadingScreen;

        [Tooltip("Names of scenes keep after ending the game session.")]
        [SerializeField] private List<string> _sceneNamesToKeep = new();

        #endregion

        #region Lifecycle Methods

        // Starts the return-to-menu sequence on component start.
        public void Start()
        {
            StartCoroutine(BackToMenuRoutine());
        }

        #endregion

        #region Private Methods

        // Saves the game, unloads unnecessary scenes, and returns to the menu.
        private IEnumerator BackToMenuRoutine()
        {
            LoadingScreenManager.Instance.Show();
            yield return null;

            SaveManager.Instance.Save();

            Time.timeScale = 1f;

            yield return StartCoroutine(UnloadScenesAndKeepSpecified());

            LoadingScreenManager.Instance.HideAndDestroy();
            Destroy(gameObject);
        }

        // Loads scenes listed to be kept, and unloads all others.
        private IEnumerator UnloadScenesAndKeepSpecified()
        {
            // Ensure all required scenes are loaded.
            foreach (var sceneName in _sceneNamesToKeep)
            {
                if (!SceneManager.GetSceneByName(sceneName).isLoaded)
                {
                    var loadOp = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
                    while (!loadOp.isDone)
                        yield return null;
                }
            }

            // Collect all other scenes to be unloaded.
            var scenesToUnload = new List<Scene>();

            for (int i = 0; i < SceneManager.sceneCount; i++)
            {
                var scene = SceneManager.GetSceneAt(i);

                if (scene.isLoaded && !_sceneNamesToKeep.Contains(scene.name))
                {
                    scenesToUnload.Add(scene);
                }
            }

            // Unload the scenes not in the keep list.
            foreach (var scene in scenesToUnload)
            {
                var unloadOp = SceneManager.UnloadSceneAsync(scene);
                while (!unloadOp.isDone)
                    yield return null;
            }
        }

        #endregion
    }
}
