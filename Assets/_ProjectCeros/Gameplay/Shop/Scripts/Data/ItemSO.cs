/// <summary>
/// Creates the SO for Items in the Shop.
/// </summary>

/// <remarks>
/// 02/06/2025 by Unik Kelmendi: Initial creation.
/// </remarks>


using UnityEngine;

namespace ProjectCeros
{
    [CreateAssetMenu(fileName = "NewItem", menuName = "Other/Shop/Item")]
    public class ItemSO : ScriptableObject
    {
        [Tooltip("Name of the Shop Item.")]
        public string ItemName;

        [Tooltip("Item description.")]
        [TextArea(3, 10)]
        public string Description;

        [Tooltip("Sprite displayed in the Shop UI.")]
        public Sprite ItemSprite;

        [Tooltip("Item price.")]
        public int Price;

        [Tooltip("Every item that upgrades into another item needs to have the same number here.")]
        public int UpgradeGroup;

        [Tooltip("The id of the item. One id per item. Important: lower id items upgrade into higher id items.")]
        public int Id;

        [Tooltip("Modifier for the podcast calculation.")]
        public float Modifier;
    }
}
