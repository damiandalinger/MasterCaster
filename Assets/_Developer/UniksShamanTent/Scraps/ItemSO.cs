// <summary>
/// Creates SO for Items in the Shop
/// </summary>

/// <remarks>
/// 02/06/2025 by Unik Kelmendi: Initial creation.
/// </remarks>


using UnityEngine;

namespace ProjectCeros
{
    [CreateAssetMenu(fileName = "NewItem", menuName = "Shop/Items")]
    public class ItemSO : ScriptableObject
    {

        [Tooltip("Name of the Shop Item")]
        [SerializeField] public string itemName;


        [TextArea(3, 10)]
        [SerializeField] public string description;

        public Sprite itemSprite;
        public int price;

        public int upgradeGroup;

        public int id; 




    }
}
