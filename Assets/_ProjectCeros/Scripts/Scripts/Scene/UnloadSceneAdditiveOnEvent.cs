/// <summary>
/// Unloads previously loaded additive scenes using their names.
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
    public class UnloadSceneAdditiveOnEvent : MonoBehaviour
    {
        #region Fields

#if UNITY_EDITOR
        [Tooltip("SceneAssets used in the Editor. Scene names will be extracted automatically.")]
        [SerializeField] private List<SceneAsset> _sceneAssets = new();
#endif

        [HideInInspector, SerializeField] private List<string> _sceneNames = new();

        #endregion

        #region Public Methods

        // Unloads all previously loaded additive scenes by name, if valid and loaded.
        public async void UnloadScenesAdditively()
        {
            if (_sceneNames == null || _sceneNames.Count == 0)
                return;

            foreach (var sceneName in _sceneNames)
            {
                if (string.IsNullOrWhiteSpace(sceneName))
                    continue;

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
