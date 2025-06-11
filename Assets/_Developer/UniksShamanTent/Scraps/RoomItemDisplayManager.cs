using UnityEngine;
using System.Collections.Generic;

namespace ProjectCeros
{
    
    // instantiate object based on item id when event is triggered
    // instantiated object needs to replace old item
    // Once the game reloads, it needs to look at the items and based on the item id, the inventory needs to be restored


public class RoomItemDisplayManager : MonoBehaviour
    {
        public ItemDatabaseSO itemDatabase;
        public List<RoomItemDisplay> roomObjects;
        public IntRuntimeSet unlockedItemIDs; // This comes from your runtime set



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

                int group = item.upgradeGroup;

                if (!activeUpgrades.ContainsKey(group))
                    activeUpgrades[group] = item;
                else if (item.id > activeUpgrades[group].id)
                    activeUpgrades[group] = item;
            }

            foreach (var display in roomObjects)
            {
                var item = display.itemData;
                int group = item.upgradeGroup;

                bool shouldBeActive = activeUpgrades.ContainsKey(group) &&
                                      activeUpgrades[group].id == item.id;

                display.gameObject.SetActive(shouldBeActive);
            }
        }
    }
}