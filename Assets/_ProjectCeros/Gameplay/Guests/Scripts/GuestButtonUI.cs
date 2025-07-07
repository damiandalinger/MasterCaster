/// <summary>
/// This script shows the correct guest once the player clicks on the button that is connected to the
/// guest info in the guest tab.
/// </summary>

/// <remarks>
/// 18/06/2025 by Unik Kelmendi: Initial creation.
/// </remarks>

using UnityEngine;
using UnityEngine.UI;


namespace ProjectCeros
{

    public class GuestButtonUI : MonoBehaviour
    {
        [SerializeField] private GuestSO _guestData;

        [SerializeField] private IntRuntimeSet _unlockedId;

        public Button button;

        // This tells the GuestUI the SO data.
        public void TransferData(GuestSO guest)
        {

            _guestData = guest;


            if (_unlockedId.Items.Contains(guest.GuestID) && (button != null))
            {
                button.onClick.RemoveAllListeners(); // Clear any previous bindings
                button.onClick.AddListener(OnClick); // Add this instance’s click
            }

            else
            {
                 button.enabled = false;
            }
        }

        private void OnClick()
        {
            GuestUI.Instance.ShowGuestDetails(_guestData);

        }

    }
}