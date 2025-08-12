/// <summary>
/// Manages the lifecycle of the loading screen during scene transitions.
/// </summary>

/// <remarks>
/// 27/05/2025 by Damian Dalinger: Script creation.
/// </remarks>

using UnityEngine;
using UnityEngine.Events;

namespace ProjectCeros
{
    public class LoadingScreenManager : MonoBehaviour
    {
        #region Fields

        [Tooltip("The loading screen prefab to be instantiated.")]
        [SerializeField] private GameObject _loadingScreenPrefab;

        [Tooltip("Invoked when the loading screen is shown.")]
        [SerializeField] private UnityEvent _onShow;

        [Tooltip("Invoked when the loading screen is hidden.")]
        [SerializeField] private UnityEvent _onHide;

        private GameObject _instance;

        #endregion

        #region Properties

        // Static singleton instance of the LoadingScreenManager.
        public static LoadingScreenManager Instance { get; private set; }

        #endregion

        #region Lifecycle Methods

        // Ensures only one instance of the manager exists across scenes.
        private void Awake()
        {
            if (Instance != null)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
        }

        #endregion

        #region Public Methods

        // Hides and destroys the loading screen instance.
        public void HideAndDestroy()
        {
            if (_instance != null)
            {
                _onHide?.Invoke();
                Destroy(_instance);
                _instance = null;
            }
        }

        // Shows the loading screen. Instantiates it if necessary.
        public void Show()
        {
            if (_instance == null)
            {
                _instance = Instantiate(_loadingScreenPrefab);
                _instance.name = "LoadingScreen";
                DontDestroyOnLoad(_instance);
            }

            _instance.SetActive(true);
            _onShow?.Invoke();
        }

        #endregion
    }
}
