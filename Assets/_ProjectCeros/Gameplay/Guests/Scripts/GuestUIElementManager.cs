/// <summary>
/// This script creates the GuestUI elements.
/// </summary>

/// <remarks>
/// 25/06/2025 by Unik Kelmendi: Initial creation.
/// </remarks>

using UnityEngine;

namespace ProjectCeros
{

    public class GuestUIManager : MonoBehaviour
    {
        [SerializeField] private GuestDatabaseSO _allGuests;
        [SerializeField] private GuestSORuntimeSet _unlockedGuests;
        [SerializeField] private GuestUI _guestUI;

        [SerializeField] private GameObject guestIconPrefab;
        [SerializeField] private Transform iconParent; 

        [SerializeField] private bool _isFirst;


        public void PopulateGuestIcons()
        {
            // Clear existing icons
            foreach (Transform child in iconParent)
            {
                Destroy(child.gameObject);
            }

            _isFirst = true;

            // Create new icons
            foreach (var guest in _unlockedGuests.Items)
            {
                var iconGO = Instantiate(guestIconPrefab, iconParent);
                var iconUI = iconGO.GetComponent<GuestIconUI>();
                iconUI.Setup(guest);


                var iconButtonUI = iconGO.GetComponent<GuestButtonUI>();
                iconButtonUI.TransferData(guest);

                if (_isFirst)
                {
                    _guestUI.ShowGuestDetails(guest);
                    Debug.Log("Show the first info");
                    _isFirst = false;
                }

            }

            foreach (var guest in _allGuests.AllGuests)
            {
                if (!_unlockedGuests.Items.Contains(guest))
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
}