/// <summary>
/// This script handles UI animation.
/// </summary>

/// <remarks>
/// 17/07/2025 by Unik Kelmendi: Initial creation.
/// </remarks>

using UnityEngine;

namespace ProjectCeros
{
    public class SmoothWobbleZ : MonoBehaviour
{
    [Header("Z Rotation Settings")]
    public float rotationAmount = 10f;
    public float speed = 1f;

    private float initialZ;

    void Start()
    {
        initialZ = transform.eulerAngles.z;
    }

    void Update()
    {
        float angle = Mathf.Sin(Time.time * speed) * rotationAmount;
        Vector3 euler = transform.eulerAngles;
        euler.z = initialZ + angle;
        transform.rotation = Quaternion.Euler(euler);
    }
}
}
