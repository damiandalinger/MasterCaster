/// <summary>
/// This script shows the correct item once the player clicks on the button that is connected to the
/// item info in the shop tab.
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

    

        public Button button;

        // This tells the GuestUI the SO data.
        public void TransferData(GuestSO guestData)
        {

            _guestData = guestData;

            
            if (button != null)
            {
                button.onClick.RemoveAllListeners(); // Clear any previous bindings
                button.onClick.AddListener(OnClick); // Add this instance’s click
            }

        }

        private void OnClick()
        {
            GuestUI.Instance.ShowGuestDetails(_guestData);

        }

    }
}