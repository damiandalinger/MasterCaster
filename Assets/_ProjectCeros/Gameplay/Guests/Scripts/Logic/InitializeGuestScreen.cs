/// <summary>
/// This script initializes the GuestScreen whenever the player enters the scene.
/// </summary>

/// <remarks>
/// 25/06/2025 by Unik Kelmendi: Initial creation.
/// </remarks>

using UnityEngine;

namespace ProjectCeros

{
    public class InitializeGuestScreens : MonoBehaviour
    {
        [SerializeField] private GameEvent gameEvent;

        public void Awake()
        {
            gameEvent.Raise();
        }
    }
}