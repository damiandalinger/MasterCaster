using UnityEngine;

namespace ProjectCeros

{
    public class GiveOutGuestSO : MonoBehaviour
    {
        public GuestDatabaseSO GuestDatabase;

        public GuestSO Guest;

        public void GiveOutGuest(int id)
        {
            Guest = GuestDatabase.GetGuestByID(id);

            Debug.Log($"Give out the Guest: {Guest}");

            Guest.isRequested = true;


        }


    }



}