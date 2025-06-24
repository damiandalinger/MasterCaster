/// <summary>
/// Creates the item Database SO. In this, all items that the player can ever own are listed.
/// Then, based on the item id this script can return the ItemSO that is needed.
/// </summary>

/// <remarks>
/// 18/06/2025 by Unik Kelmendi: Initial creation.
/// </remarks>

using UnityEngine;
using System.Collections.Generic;

namespace ProjectCeros
{
    [CreateAssetMenu(menuName = "Guests/Guest Database")]
    public class GuestDatabaseSO : ScriptableObject
    {

        [Tooltip("Here go all Guests that the Player can potentially have (Only by Default unlocked guests!).")]
        public List<GuestSO> AllGuests;

        private Dictionary<int, GuestSO> _lookup;

        // Setup the dictionary with the item id and ItemSO.
        public void Initialize()
        {
            _lookup = new Dictionary<int, GuestSO>();
            foreach (var item in AllGuests)
            {
                if (!_lookup.ContainsKey(item.GuestID))
                    _lookup[item.GuestID] = item;
                else
                    Debug.LogWarning($"Duplicate ID {item.GuestID} in ItemDatabase.");
            }
        }


        // Call this method to get the ItemSO in exchange for the item id.
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