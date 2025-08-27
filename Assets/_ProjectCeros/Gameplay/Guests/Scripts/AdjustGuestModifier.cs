/// <summary>
/// This script adjust the guest modifier for the podcast calculation depending on the guest that has accepted.
/// It resets the modifier for guests bakc to zero.
/// </summary>

/// <remarks>
/// 25/06/2025 by Unik Kelmendi: Initial creation.
/// </remarks>

using UnityEngine;

namespace ProjectCeros
{
    public class AdjustGuestModifier : MonoBehaviour
    {
        [SerializeField] private GuestDatabaseSO _allguests;

        public FloatReference _guestModifier;


        public void ModifyGuestModifier()
        {
            foreach (var guest in _allguests.AllGuests)
            {
                if (guest.hasAccepted)
                {
                    _guestModifier.Variable.SetValue(guest.Modifier);
                }
            }
        }

        public void ResetModifier()
        {
            _guestModifier.Variable.SetValue(0);
        }
    }
}