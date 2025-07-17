/// <summary>
/// Binds the canvas to the main camera at runtime (used when scenes are loaded additively).
/// </summary>

/// <remarks>
/// 17/07/2025 by Damian Dalinger: Initial implementation.
/// </remarks>

using UnityEngine;

namespace ProjectCeros.Core
{
    [ExecuteAlways]
    [RequireComponent(typeof(Camera))]
    public class FixedAspectRatio : MonoBehaviour
    {
        #region Fields

        [Tooltip("Target aspect ratio in width / height (e.g. 16:9 = 1.777...)")]
        [SerializeField] private float targetAspectRatio = 16f / 9f;

        private Camera _camera;

        #endregion

        #region Lifecycle Methods

        private void Awake()
        {
            _camera = GetComponent<Camera>();
            UpdateViewport();
        }

        private void Update()
        {
            UpdateViewport();
        }

        #endregion

        #region Private Methods

        // Updates the camera's viewport to enforce the target aspect ratio, adding letterboxing or pillarboxing if needed.
        private void UpdateViewport()
        {
            float windowAspect = (float)Screen.width / Screen.height;
            float scaleHeight = windowAspect / targetAspectRatio;

            if (scaleHeight < 1.0f)
            {
                // Letterbox (black bars top & bottom)
                float yOffset = (1.0f - scaleHeight) / 2.0f;
                _camera.rect = new Rect(0f, yOffset, 1f, scaleHeight);
            }
            else
            {
                // Pillarbox (black bars left & right)
                float scaleWidth = 1.0f / scaleHeight;
                float xOffset = (1.0f - scaleWidth) / 2.0f;
                _camera.rect = new Rect(xOffset, 0f, scaleWidth, 1f);
            }
        }

        #endregion
    }
}
