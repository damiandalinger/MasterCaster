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