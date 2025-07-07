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

        [SerializeField] private IntRuntimeSet _unlockedId;

        public void Setup(GuestSO guest)
        {
            if (!_unlockedId.Items.Contains(guest.GuestID))
            {
                _guestPortrait.sprite = guest.GuestSpriteLocked;
            }


            else if (guest.isOnCooldown)
            {
                _guestPortrait.sprite = guest.GuestSpriteUnavailable;
            }

            else
            {
                _guestPortrait.sprite = guest.GuestSpriteavailable;
            }
        }


    }
}