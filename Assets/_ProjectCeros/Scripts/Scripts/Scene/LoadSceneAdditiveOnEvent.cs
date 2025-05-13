/// <summary>
/// Loads scenes additively at runtime. Uses SceneAsset references in editor but stores scene names internally.
/// </summary>

/// <remarks>
/// 12/05/2025 by Damian Dalinger: Script creation.
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
    public class LoadSceneAdditiveOnEvent : MonoBehaviour
    {
        #region Fields

#if UNITY_EDITOR
        [Tooltip("SceneAssets used in the Editor. Scene names will be extracted automatically.")]
        [SerializeField] private List<SceneAsset> _sceneAssets = new();
#endif
        [HideInInspector, SerializeField] private List<string> _sceneNames = new();

        #endregion

        #region Public Methods

        /// Loads all scenes additively by name if not already loaded.
        public async void LoadScenesAdditively()
        {
            if (_sceneNames == null || _sceneNames.Count == 0)
                return;

            foreach (var sceneName in _sceneNames)
            {
                if (string.IsNullOrWhiteSpace(sceneName)) continue;

                if (SceneManager.GetSceneByName(sceneName).isLoaded)
                    continue;

                var loadOperation = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
                while (!loadOperation.isDone)
                    await Task.Yield();
            }
        }

        #endregion

#if UNITY_EDITOR
        #region Editor Only

        // Automatically updates the internal scene name list based on assigned SceneAssets in the editor.
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