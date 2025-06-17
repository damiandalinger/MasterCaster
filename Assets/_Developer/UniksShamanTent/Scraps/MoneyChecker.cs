using System.Collections.Generic;
using UnityEngine;

namespace ProjectCeros
{
    public class MoneyChecker : MonoBehaviour
    {
        [SerializeField] private IntReference _money;

        [SerializeField] public ItemSO _itemToPurchase;

        [SerializeField] private IntGameEvent _itemEvent;

        [SerializeField] private List<int> boughtItems;


        public void SetItemToPurchase(ItemSO item)
        {
            _itemToPurchase = item;
        }



        public void CheckMoney()
        {
            // Has enough money?
            if (_money.Value >= _itemToPurchase.price)
            {
                // Hasn't bought this item yet?
                if (!boughtItems.Contains(_itemToPurchase.id))
                {
                    _itemEvent.Raise(_itemToPurchase.id);
                    _money.Variable.ApplyChange(-_itemToPurchase.price);
                    boughtItems.Add(_itemToPurchase.id);
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
