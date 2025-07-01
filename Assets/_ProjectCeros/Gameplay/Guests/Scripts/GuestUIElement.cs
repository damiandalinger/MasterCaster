/// <summary>
/// This Script is attached to the instantiated guestUI
/// </summary>

/// <remarks>
/// 25/06/2025 by Unik Kelmendi: Initial creation.
/// </remarks>

using UnityEngine;
using UnityEngine.UI;

namespace ProjectCeros
{


    public class GuestIconUI : MonoBehaviour
    {
        [SerializeField] private Image _guestPortrait;

        public void Setup(GuestSO guest)
        {
            _guestPortrait.sprite = guest.GuestSprite;

        }
    }


}