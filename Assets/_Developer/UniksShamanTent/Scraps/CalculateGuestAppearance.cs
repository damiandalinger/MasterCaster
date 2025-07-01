/// <summary>
/// This script looks at the requested guests and starts the process of determing if they accept or not.
/// Also handles guest cooldown.
/// </summary>

/// <remarks>
/// 25/06/2025 by Unik Kelmendi: Initial creation.
/// </remarks>

using UnityEngine;

namespace ProjectCeros
{
    public class CalculateGuestAppearance : MonoBehaviour

    {
        [SerializeField] private GuestDatabaseSO _guestDatabase;

        [SerializeField] private GambleForGuest _gamble;

        [SerializeField] private IntReference _guestCooldown;

        public void CalculateGuest()
        {

            foreach (var guest in _guestDatabase.AllGuests)
            {
                if (guest.isRequested)
                {
                    Debug.Log($"Guest was invited! it's {guest.Name}");

                    guest.isOnCooldown = true;
                    guest.isRequested = false;

                    _gamble.GambleGuest(guest);
                }


                if (guest.isOnCooldown)
                {
                    guest.CooldownCounter++;

                }

                if (guest.CooldownCounter == _guestCooldown)
                {
                    guest.isOnCooldown = false;

                    guest.CooldownCounter = 0;

                }

            }




        }







    }
}