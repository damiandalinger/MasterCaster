/// <summary>
/// Checks if the player has enough money to complete the purchase
/// </summary>

/// <remarks>
/// 18/06/2025 by Unik Kelmendi: Initial creation.
/// </remarks>

using System.Collections.Generic;
using UnityEngine;

namespace ProjectCeros
{
    public class MoneyChecker : MonoBehaviour
    {
        [SerializeField] public ItemSO ItemToPurchase;

        [SerializeField] private IntReference _money;

        [SerializeField] private IntGameEvent _itemEvent;

        [SerializeField] private List<int> _boughtItems;

        // Accesed by other scripts to tell MoneyChecker which ItemSo is in question for purchase
        public void SetItemToPurchase(ItemSO item)
        {
            ItemToPurchase = item;
        }

        // Checks if player has enough money and if the item was already bought or not
        public void CheckMoney()
        {
            // Has enough money.
            if (_money.Value >= ItemToPurchase.Price)
            {
                // Hasn't bought this item yet.
                if (!_boughtItems.Contains(ItemToPurchase.Id))
                {
                    _money.Variable.ApplyChange(-ItemToPurchase.Price);
                    _itemEvent.Raise(ItemToPurchase.Id);
                    _boughtItems.Add(ItemToPurchase.Id);
                }

                else
                {
                    Debug.Log("You already bought this item!");
                }
            }

            else
            {
                Debug.Log("Rack off, you wet wallet!");
            }
        }
    }
}
