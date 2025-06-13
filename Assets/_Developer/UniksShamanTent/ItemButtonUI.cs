using UnityEngine;
using UnityEngine.UI;


namespace ProjectCeros
{

    public class ItemButtonUI : MonoBehaviour
    {
        [SerializeField] private ItemSO itemData;
        [SerializeField] private Button button;

        private void Awake()
        {
            button.onClick.AddListener(OnClick);
        }

        private void OnClick()
        {
            ShopUI.Instance.ShowItemDetails(itemData);
        }
    }
}