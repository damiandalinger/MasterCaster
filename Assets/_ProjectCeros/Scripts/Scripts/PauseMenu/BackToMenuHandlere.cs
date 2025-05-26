using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace ProjectCeros
{
    public class BackToMenuHandler : MonoBehaviour
    {
        [SerializeField] private GameObject _loadingScreen;
        [SerializeField] private string _mainMenuSceneName = "MainMenu";
        [SerializeField] private SaveManager _saveManager;
        void Start()
        {
            _saveManager = FindFirstObjectByType<SaveManager>();
        }
        public void SaveAndReturnToMenu()
        {
            StartCoroutine(BackToMenuRoutine());
        }

        private IEnumerator BackToMenuRoutine()
        {
            _loadingScreen?.SetActive(true);
            yield return null;

           _saveManager.Save();

            // 1. MainMenuScene additiv laden
            var loadOp = SceneManager.LoadSceneAsync(_mainMenuSceneName, LoadSceneMode.Additive);
            while (!loadOp.isDone)
                yield return null;

            // 2. Gameplay-Szenen entladen
            var scenesToUnload = new System.Collections.Generic.List<Scene>();

            for (int i = 0; i < SceneManager.sceneCount; i++)
            {
                var scene = SceneManager.GetSceneAt(i);

                if (scene.isLoaded && scene.name != "Bootstrapper" && scene.name != _mainMenuSceneName)
                    scenesToUnload.Add(scene);
            }

            foreach (var scene in scenesToUnload)
            {
                var unloadOp = SceneManager.UnloadSceneAsync(scene);
                while (!unloadOp.isDone)
                    yield return null;
            }

            _loadingScreen?.SetActive(false);
            Debug.Log("✅ Zurück im Hauptmenü.");
        }
    }
}
