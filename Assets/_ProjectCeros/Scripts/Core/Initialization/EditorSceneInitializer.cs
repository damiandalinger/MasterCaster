#if UNITY_EDITOR
/// <summary>
/// Editor-only utility that starts a new game from the bootstrapper when testing from a subscene.
/// </summary>

/// <remarks>
/// 13/05/2025 by Damian Dalinger: Script creation.
/// 27/05/2025 by Damian Dalinger: Changed to a coroutine.
/// 12/08/2025 by Damian Dalinger: Added DontSaveInBuild Flags.
/// </remarks>

using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

namespace ProjectCeros
{
    public class EditorSceneInitializer : MonoBehaviour
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

        private void Awake()
        {
            gameObject.hideFlags |= HideFlags.DontSaveInBuild;
        }

        private void Start()
        {
            if (SceneManager.sceneCount > 1)
            {
                Destroy(gameObject);
                return;
            }

            if (Application.isPlaying)
            {
                DontDestroyOnLoad(gameObject);
            }
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
                if (Application.isPlaying)
                {
                    DontDestroyOnLoad(gameObject);
                }
            }

            Destroy(gameObject);
        }

        private void OnValidate()
        {
            gameObject.hideFlags |= HideFlags.DontSaveInBuild;
        }

        private void Reset()
        {
            gameObject.hideFlags |= HideFlags.DontSaveInBuild;
        }

        #endregion
    }
}
#endif
