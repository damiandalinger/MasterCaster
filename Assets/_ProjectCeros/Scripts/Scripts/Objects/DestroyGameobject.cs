/// <summary>
/// Handles the destruction of a GameObject.
/// </summary>

/// <remarks>
///07/07/2025 by Damian Dalinger: Script creation.
/// </remarks>

using UnityEngine;

namespace ProjectCeros
{
    public class DestroyGameobject : MonoBehaviour
    {
        [Tooltip("Optional: If set, this GameObject will be destroyed instead of this component's object.")]
        [SerializeField] private GameObject _objectToDestroy;

        // Destroys the assigned object. Falls back to self if none assigned.
        public void DestroyTarget()
        {
            if (_objectToDestroy != null)
            {
                Destroy(_objectToDestroy);
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }
}
