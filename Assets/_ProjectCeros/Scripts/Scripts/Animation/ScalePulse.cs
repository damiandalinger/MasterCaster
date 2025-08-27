/// <summary>
/// This script handles UI animation.
/// </summary>

/// <remarks>
/// 17/07/2025 by Unik Kelmendi: Initial creation.
/// </remarks>

using UnityEngine;

namespace ProjectCeros
{
    public class ScalePulse : MonoBehaviour
    {

        [Header("Scale Settings")]
        public Vector3 minScale = new Vector3(0.9f, 0.9f, 0.9f);
        public Vector3 maxScale = new Vector3(1.1f, 1.1f, 1.1f);
        public float pulseSpeed = 1f;

        private Vector3 targetScale;
        private bool scalingUp = true;

        void Start()
        {
            transform.localScale = minScale;
            targetScale = maxScale;
        }

        void Update()
        {
            transform.localScale = Vector3.Lerp(transform.localScale, targetScale, Time.deltaTime * pulseSpeed);

            if (Vector3.Distance(transform.localScale, targetScale) < 0.01f)
            {
                scalingUp = !scalingUp;
                targetScale = scalingUp ? maxScale : minScale;
            }
        }

    }
}
