/// <summary>
/// This script shows the correct item once the player clicks on the button that is connected to the
/// item info in the shop tab.
/// </summary>

/// <remarks>
/// 18/06/2025 by Unik Kelmendi: Initial creation.
/// </remarks>

using UnityEngine;


namespace ProjectCeros
{

    public class GuestButtonUI : MonoBehaviour
    {
        [SerializeField] private GuestSO _guestData;
         
        // This tells the GuestUI the SO data.
        public void TransferData()
        {
            GuestUI.Instance.ShowGuestDetails(_guestData);
        }
    }
}