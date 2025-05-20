/// <summary>
/// Editor-only utility that auto-loads the bootstrap scene additively when testing from a subscene.
/// Optionally starts a new game automatically after loading.
/// </summary>

/// <remarks>
/// 13/05/2025 by Damian Dalinger: Script creation.
/// </remarks>

using UnityEngine;
using UnityEngine.SceneManagement;
using System.Threading.Tasks;

namespace ProjectCeros
{
    public class SceneInitializer : MonoBehaviour
    {
        #region Fields

        [Tooltip("Name of the bootstrap scene to load additively during testing.")]
        [SerializeField] private string _bootstrapSceneName = "Bootstrapper";

        [Tooltip("Whether to automatically start a new game after loading the bootstrap scene.")]
        [SerializeField] private bool _startNewGame = true;

        #endregion

        #region Unity Methods

#if UNITY_EDITOR
        // Checks if the current scene is the only one loaded and injects the bootstrap scene if needed.
        private async void Awake()
        {
            if (SceneManager.sceneCount > 1)
                return;

            var loadOp = SceneManager.LoadSceneAsync(_bootstrapSceneName, LoadSceneMode.Additive);
            while (!loadOp.isDone)
                await Task.Yield();

            if (_startNewGame)
            {
                var starter = FindFirstObjectByType<NewGameStarter>();
                starter?.StartNewGame();
            }
        }
#endif

        #endregion
    }
}
