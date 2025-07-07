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