/// <summary>
/// Creates the Guest Database SO. In this, all Guests that the player can see invite are stored.
/// Then, based on the guest id this script can return the GuestSO that is needed.
/// </summary>

/// <remarks>
/// 24/06/2025 by Unik Kelmendi: Initial creation.
/// </remarks>

using UnityEngine;
using System.Collections.Generic;

namespace ProjectCeros
{
    [CreateAssetMenu(menuName = "Other/Guests/Guest Database")]
    public class GuestDatabaseSO : ScriptableObject
    {

        [Tooltip("Here go all Guests that the Player can potentially have (Only by Default unlocked guests!).")]
        public List<GuestSO> AllGuests;

        private Dictionary<int, GuestSO> _lookup;

        // Setup the dictionary with the guest id and GuestSO.
        public void Initialize()
        {
            _lookup = new Dictionary<int, GuestSO>();
            foreach (var item in AllGuests)
            {
                if (!_lookup.ContainsKey(item.GuestID))
                    _lookup[item.GuestID] = item;
                else
                    Debug.LogWarning($"Duplicate ID {item.GuestID} in GuestDatabase.");
            }
        }


        // Call this method to get the GuestSO in exchange for the guest id.
        public GuestSO GetGuestByID(int id)
        {
            if (_lookup == null)
                Initialize();

            if (_lookup.TryGetValue(id, out var item))
                return item;

            Debug.LogWarning($"Guest ID {id} not found in GuestDatabase.");
            return null;
        }
    }
}