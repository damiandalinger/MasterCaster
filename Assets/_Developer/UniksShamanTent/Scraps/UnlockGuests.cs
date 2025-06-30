using UnityEngine;
using System.Collections.Generic;

namespace ProjectCeros

{
    public class UnlockGuests : MonoBehaviour
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