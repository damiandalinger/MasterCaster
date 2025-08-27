/// <summary>
/// This script handles UI animation.
/// </summary>

/// <remarks>
/// 17/07/2025 by Unik Kelmendi: Initial creation.
/// </remarks>

using UnityEngine;

namespace ProjectCeros
{
    public class WobbleZRotation : MonoBehaviour
    {
        [Header("Z Rotation Range")]
        public float minZ = -10f;
        public float maxZ = 10f;

        [Header("Timing")]
        public float interval = 0.5f;

        private Quaternion initialRotation;

        void Start()
        {
            initialRotation = transform.rotation;
            InvokeRepeating(nameof(ApplyRandomZRotation), 0f, interval);
        }

        void ApplyRandomZRotation()
        {
            float randomZ = Random.Range(minZ, maxZ);
            Vector3 euler = initialRotation.eulerAngles;
            euler.z = randomZ;
            transform.rotation = Quaternion.Euler(euler);
        }
    }
}

