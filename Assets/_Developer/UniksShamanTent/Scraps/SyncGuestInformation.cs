/// <summary>
/// This script checks the GuestitemIds and adds them to a runtime set only containing the unlocked GuestSO.
/// </summary>

/// <remarks>
/// 25/06/2025 by Unik Kelmendi: Initial creation.
/// </remarks>

using UnityEngine;

namespace ProjectCeros

{
    public class SyncGuestInformation : MonoBehaviour
    {

        [SerializeField] private GuestDatabaseSO _allGuests;

        [SerializeField] private IntRuntimeSet unlockedGuestIDs;

        public GuestSORuntimeSet unlockedGuests;

        [SerializeField] private bool _initialize = false;



        public void SyncGuestData()
        {
            if (!_initialize)
            {
                unlockedGuests.Items.Clear();
                _initialize = true;
            }

            foreach (var guest in _allGuests.AllGuests)
            {
                if (unlockedGuestIDs.Items.Contains(guest.GuestID))
                {
                    unlockedGuests.Add(guest);
                }
            }
        }




    }


}