/// <summary>
/// This script updates the item modifier that dictates bonuses from bought items.
/// </summary>

/// <remarks>
/// 20/06/2025 by Unik Kelmendi: Initial creation.
/// </remarks>

using UnityEngine;

namespace ProjectCeros
{
    public class ItemModifierUpdater : MonoBehaviour
    {
        [SerializeField] private IntRuntimeSet _itemIds;

        [SerializeField] private ItemDatabaseSO _itemDatabase;

        [SerializeField] private FloatReference _shopModifier;

        private ItemSO _item;

        public void UpdateModifier(int id)
        {
            _item = _itemDatabase.GetItemByID(id);

            _shopModifier.Variable.ApplyChange(_item.Modifier);

            Debug.Log($"Value changed to: {_shopModifier.Value}");
        }
    }
}
