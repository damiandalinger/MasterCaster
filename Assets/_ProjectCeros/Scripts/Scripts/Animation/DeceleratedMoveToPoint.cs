/// <summary>
/// This script handles UI animation.
/// </summary>

/// <remarks>
/// 17/07/2025 by Unik Kelmendi: Initial creation.
/// </remarks>

using UnityEngine;

namespace ProjectCeros
{
   public class DeceleratedMoveToPoint : MonoBehaviour
{
    [Header("Movement Settings")]
    public Transform targetPoint;
    public float maxSpeed = 10f;
    public float decelerationDistance = 5f;
    public float stopThreshold = 0.05f;

    [SerializeField] private BoolReference _isMoving;

    void Update()
    {
        if (!_isMoving) return;

        Vector3 direction = targetPoint.position - transform.position;
        float distance = direction.magnitude;

        if (distance < stopThreshold)
        {
            transform.position = targetPoint.position;
            _isMoving.Variable.SetValue(false);
            return;
        }

        float speed = Mathf.Lerp(0f, maxSpeed, Mathf.Clamp01(distance / decelerationDistance));
        transform.position += direction.normalized * speed * Time.deltaTime;
    }

    public void StartMoving()
    {
        _isMoving.Variable.SetValue(true);
    }
}
}
