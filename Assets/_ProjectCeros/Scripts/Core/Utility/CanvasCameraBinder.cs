/// <summary>
/// Binds the canvas to the main camera at runtime (used when scenes are loaded additively).
/// </summary>

/// <remarks>
/// 17/07/2025 by Damian Dalinger: Initial implementation.
/// </remarks>

using UnityEngine;

namespace ProjectCeros
{

    [RequireComponent(typeof(Canvas))]
    public class CanvasCameraBinder : MonoBehaviour
    {
        #region Fields

        [Tooltip("The desired sorting layer for the canvas.")]
        [SerializeField] private string sortingLayerName = "UI";

        private Canvas _canvas;
        private bool _cameraAssigned;

        #endregion

        #region Unity Lifecycle

        private void Awake()
        {
            _canvas = GetComponent<Canvas>();
        }

        void OnEnable()
        {
            AssignCameraByTag();
        }

        private void Update()
        {
            if (_cameraAssigned)
            {
                return;
            }

            AssignCameraByTag();
        }

        #endregion

        #region Private Methods

        // Attempts to find a Camera with the "MainCamera" tag and assign it to this Canvas.
        private void AssignCameraByTag()
        {
            GameObject camObject = GameObject.FindWithTag("MainCamera");

            if (camObject == null)
            {
                return;
            }

            Camera cam = camObject.GetComponent<Camera>();

            if (cam == null)
            {
                return;
            }

            _canvas.worldCamera = cam;
            _canvas.sortingLayerName = sortingLayerName;
            _cameraAssigned = true;
        }

        #endregion
    }
}
