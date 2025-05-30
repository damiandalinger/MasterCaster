#if UNITY_EDITOR
/// <summary>
/// Editor-only utility that starts a new game from the bootstrapper when testing from a subscene.
/// </summary>

/// <remarks>
/// 13/05/2025 by Damian Dalinger: Script creation.
/// 27/05/2025 by Damian Dalinger: Changed to a coroutine. 
/// </remarks>

using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

namespace ProjectCeros
{
    public class SceneInitializer : MonoBehaviour
    {
        #region Fields

        [Tooltip("Name of the bootstrap scene to load additively during testing.")]
        [SerializeField] private string _bootstrapSceneName = "Bootstrapper";

        [Tooltip("Whether to automatically start a new game after loading the bootstrap scene.")]
        [SerializeField] private bool _shouldStartNewGame = true;

        [Tooltip("Prefab to use for starting the new game.")]
        [SerializeField] private BaseGameStarter _newGameStarterPrefab;

        #endregion

        #region Lifecycle Methods

        private void Start()
        {
            if (SceneManager.sceneCount > 1)
            {
                Destroy(gameObject);
                return;
            }
            
            DontDestroyOnLoad(gameObject);
            StartCoroutine(InitRoutine());
        }

        #endregion

        #region Private Methods

        // Handles the loading of the bootstrap scene and optional new game startup.
        private IEnumerator InitRoutine()
        {
            Scene activeScene = SceneManager.GetActiveScene();

            if (activeScene.name != _bootstrapSceneName)
            {
                AsyncOperation loadOperation = SceneManager.LoadSceneAsync(_bootstrapSceneName, LoadSceneMode.Single);
                while (!loadOperation.isDone)
                {
                    yield return null;
                }
            }

            if (_shouldStartNewGame && _newGameStarterPrefab != null)
            {
                BaseGameStarter instance = Instantiate(_newGameStarterPrefab);
                DontDestroyOnLoad(instance.gameObject);
            }

            Destroy(gameObject);
        }

        #endregion
    }
}
#endif
