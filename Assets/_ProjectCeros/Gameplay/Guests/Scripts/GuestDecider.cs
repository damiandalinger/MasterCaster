using UnityEngine;
using UnityEngine.UI;

namespace ProjectCeros
{
    public class GuestDecider : MonoBehaviour
    {
        [SerializeField] private GuestDatabaseSO _allGuests;

        [SerializeField] private GuestSO _guest;

        [SerializeField] private GameObject _note;

        [SerializeField] private Image _guestSprite;

        public FloatReference _guestModifier;



        public void Start()
        {
            _guest = null;

            foreach (var guest in _allGuests.AllGuests)
            {
                if (guest.hasAccepted)
                {
                    _guest = guest;
                }
            }

            AskForGuest();
        }

        public void AskForGuest()
        {
            if (_guest != null)
            {
                _note.SetActive(true);
                _guestSprite.sprite = _guest.GuestSprite;
            }

            else
            {
                _note.SetActive(false);
            }
        }

        public void AcceptGuest()
        {
            _guest.wasInterviewed = true;
            _note.SetActive(false);

        }

        public void DeclineGuest()
        {
            _note.SetActive(false);
            _guest.hasAccepted = false;
            _guestModifier.Variable.SetValue(0);
        }
       

    }
}