/// <summary>
/// This script handles UI animation.
/// </summary>

/// <remarks>
/// 17/07/2025 by Unik Kelmendi: Initial creation.
/// </remarks>


using UnityEngine;
using UnityEngine.EventSystems;
 
namespace ProjectCeros
{
    public class ButtonHoverYRotation : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [Header("Swing Settings")]
    public float swingAngle = 10f;
    public float swingSpeed = 4f;
    public float decayDuration = 0.5f; // Time to continue swinging after hover ends

    private Quaternion originalRotation;
    private bool isHovering = false;
    private bool isDecaying = false;
    private float swingTimer = 0f;
    private float decayTimer = 0f;

    void Start()
    {
        originalRotation = transform.localRotation;
    }

    void Update()
    {
        if (isHovering)
        {
            swingTimer += Time.deltaTime * swingSpeed;
            float angle = Mathf.Sin(swingTimer) * swingAngle;
            ApplySwing(angle);
        }
        else if (isDecaying)
        {
            decayTimer += Time.deltaTime;
            float t = 1f - (decayTimer / decayDuration);
            float angle = Mathf.Sin(swingTimer) * swingAngle * t;

            swingTimer += Time.deltaTime * swingSpeed;
            ApplySwing(angle);

            if (decayTimer >= decayDuration)
            {
                isDecaying = false;
            }
        }
        else
        {
            // Fully reset to original rotation
            transform.localRotation = Quaternion.Lerp(transform.localRotation, originalRotation, Time.deltaTime * swingSpeed);
        }
    }

    void ApplySwing(float angle)
    {
        Vector3 euler = originalRotation.eulerAngles;
        transform.localRotation = Quaternion.Euler(euler.x, euler.y + angle, euler.z);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        isHovering = true;
        isDecaying = false;
        decayTimer = 0f;
        swingTimer = 0f;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        isHovering = false;
        isDecaying = true;
        decayTimer = 0f;
    }
}
}
