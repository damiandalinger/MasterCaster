using UnityEngine;

namespace ProjectCeros
{
    public class SmoothWobbleY : MonoBehaviour
{
    [Header("Y Rotation Settings")]
    public float rotationAmount = 10f;
    public float speed = 1f;

    private float initialY;

    void Start()
    {
        initialY = transform.eulerAngles.y;
    }

    void Update()
    {
        float angle = Mathf.Sin(Time.time * speed) * rotationAmount;
        Vector3 euler = transform.eulerAngles;
        euler.y = initialY + angle;
        transform.rotation = Quaternion.Euler(euler);
    }
}
}
