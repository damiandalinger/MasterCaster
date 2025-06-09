using UnityEngine;

namespace ProjectCeros
{
    public class MoneyChecker : MonoBehaviour
    {
        [SerializeField] private IntReference _money;

        [SerializeField] private ItemSO _itemToPurchase;

        [SerializeField] private IntGameEvent _itemEvent;

        public void CheckMoney()
        {


            if (_money.Value >= _itemToPurchase.price)
            {
                 _itemEvent.Raise(_itemToPurchase.id);
            }

            else
            {
                Debug.Log("Rack off, you wet wallet!");
            }

        }



    }
}
