/// <summary>
/// This script handles how and when the items appear in the room of the player once he acquires them.
///  Creates a Dictionary which lists the items that get displayed, upgraded items get ignored.
///  Fetches the ItemSO by id from the ItemDatabaseSO.
///  Displays the items in the studio, as long as the objects are assigned to roomObjects.
/// </summary>

/// <remarks>
/// 18/06/2025 by Unik Kelmendi: Initial creation.
/// </remarks>

using UnityEngine;
using System.Collections.Generic;

namespace ProjectCeros
{

    public class RoomItemDisplayManager : MonoBehaviour
    {
        [SerializeField, Tooltip("Here goes the ItemDatabase that holds all possible items.")]
        private ItemDatabaseSO itemDatabase;

        [SerializeField, Tooltip("Here go all the GameObjects that contain the RoomItemDisplay script.")]
        private List<RoomItemDisplay> roomObjects;

        [SerializeField, Tooltip("Here goes the RuntimeSet that hold all the item ids.")]
        private IntRuntimeSet unlockedItemIDs;

        [SerializeField, Tooltip("Here goes the RuntimeSet that hold all the active only item ids.")]
        private IntRuntimeSet _activeItemIDs;

        public void Start()
        {
            UpdateRoomDisplay();
        }

        public void UpdateRoomDisplay()
        {
            Dictionary<int, ItemSO> activeUpgrades = new Dictionary<int, ItemSO>();

            foreach (int id in unlockedItemIDs.Items)
            {
                ItemSO item = itemDatabase.GetItemByID(id);
                if (item == null) continue;

                int group = item.UpgradeGroup;

                if (!activeUpgrades.ContainsKey(group))
                    activeUpgrades[group] = item;
                else if (item.Id > activeUpgrades[group].Id)
                    activeUpgrades[group] = item;
            }

            _activeItemIDs.Clear();
            foreach (var kvp in activeUpgrades)
            {
                _activeItemIDs.Add(kvp.Value.Id);
            }


            foreach (var display in roomObjects)
            {
                var item = display.ItemData;
                int group = item.UpgradeGroup;

                bool shouldBeActive = activeUpgrades.ContainsKey(group) &&
                                      activeUpgrades[group].Id == item.Id;

                display.gameObject.SetActive(shouldBeActive);
            }
        }
    }
}