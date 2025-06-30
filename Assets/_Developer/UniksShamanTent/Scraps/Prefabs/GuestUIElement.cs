using UnityEngine;
using UnityEngine.UI;

namespace ProjectCeros
{


    public class GuestIconUI : MonoBehaviour
    {
        [SerializeField] private Image guestPortrait;

        public void Setup(GuestSO guest)
        {
            guestPortrait.sprite = guest.GuestSprite; // assuming GuestSO has a portrait sprite
        }
    }


}