using UnityEngine;
using UnityEngine.UI;


namespace ProjectCeros

{

    public class InitializeShopUIText : MonoBehaviour
    {  
        
         [SerializeField] private ItemSO itemData;

        public void Start()
        {
            ShopUI.Instance.ShowItemDetails(itemData);

        }
    
    }

}