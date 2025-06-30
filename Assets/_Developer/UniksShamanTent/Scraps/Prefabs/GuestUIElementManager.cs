using UnityEngine;

namespace ProjectCeros
{

    public class GuestUIManager : MonoBehaviour
    {
        [SerializeField] private GuestSORuntimeSet unlockedGuests;
        [SerializeField] private GameObject guestIconPrefab;
        [SerializeField] private Transform iconParent; // assign the GuestIconGrid here


        public void PopulateGuestIcons()
        {
            // Clear existing icons
            foreach (Transform child in iconParent)
            {
                Destroy(child.gameObject);
            }

            // Create new icons
            foreach (var guest in unlockedGuests.Items)
            {
                var iconGO = Instantiate(guestIconPrefab, iconParent);
                var iconUI = iconGO.GetComponent<GuestIconUI>();
                iconUI.Setup(guest);

                var iconButtonUI = iconGO.GetComponent<GuestButtonUI>();
                iconButtonUI.TransferData(guest);
            }
        }
    }
}