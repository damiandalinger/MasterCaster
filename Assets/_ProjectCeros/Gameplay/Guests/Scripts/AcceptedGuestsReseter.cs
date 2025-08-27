/// <summary>
/// This script resets all accepted guests at the end of a day.
/// </summary>

/// <remarks>
/// 25/06/2025 by Unik Kelmendi: Initial creation.
/// </remarks>

using UnityEngine;

namespace ProjectCeros
{


    public class AcceptedGuestReseter : MonoBehaviour
    {
        [SerializeField] private GuestDatabaseSO _allguests;

        public void DeacceptEverybody()
        {
            foreach (var guest in _allguests.AllGuests)
            {
                guest.hasAccepted = false;
            }
        }
    }
}