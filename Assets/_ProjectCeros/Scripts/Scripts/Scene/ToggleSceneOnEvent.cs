/// <summary>
/// Toggles a set of additive scenes on or off based on their current load state.
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
    public class ToggleSceneOnEvent : MonoBehaviour
    {
        #region Fields

#if UNITY_EDITOR
        [Tooltip("SceneAssets used in the Editor. Scene names will be extracted automatically.")]
        [SerializeField] private List<SceneAsset> _sceneAssets = new();
#endif

        [HideInInspector, SerializeField] private List<string> _sceneNames = new();

        private bool _scenesAreLoaded = false;

        #endregion

        #region Public Methods

        // Toggles scenes: loads if not loaded, unloads if already loaded.
        public async void ToggleScenes()
        {
            if (!_scenesAreLoaded)
                await LoadScenes();
            else
                await UnloadScenes();

            _scenesAreLoaded = !_scenesAreLoaded;
        }

        #endregion

        #region Private Methods

        // Loads all scenes additively by name if not already loaded.
        private async Task LoadScenes()
        {
            foreach (var sceneName in _sceneNames)
            {
                if (SceneManager.GetSceneByName(sceneName).isLoaded)
                    continue;

                var loadOperation = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
                while (!loadOperation.isDone)
                    await Task.Yield();
            }
        }

        // Unloads all loaded scenes by name if valid and loaded.
        private async Task UnloadScenes()
        {
            foreach (var sceneName in _sceneNames)
            {
                var scene = SceneManager.GetSceneByName(sceneName);
                if (!scene.IsValid() || !scene.isLoaded)
                    continue;

                var unloadOperation = SceneManager.UnloadSceneAsync(scene);
                while (!unloadOperation.isDone)
                    await Task.Yield();
            }
        }

        #endregion

#if UNITY_EDITOR
        #region Editor Only

        /// Automatically updates the internal scene name list based on assigned SceneAssets in the editor.
        private void OnValidate()
        {
            if (_sceneAssets == null) return;

            _sceneNames.Clear();

            foreach (var sceneAsset in _sceneAssets)
            {
                if (sceneAsset == null) continue;

                var sceneName = sceneAsset.name;
                if (!_sceneNames.Contains(sceneName))
                    _sceneNames.Add(sceneName);
            }

            EditorUtility.SetDirty(this);
        }

        #endregion
#endif
    }
}
