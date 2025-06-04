
/// <summary>
/// Creates SO for Items in the Shop
/// </summary>

/// <remarks>
/// 02/06/2025 by Unik Kelmendi: Initial creation.
/// </remarks>


using UnityEngine;

namespace ProjectCeros
{
    [CreateAssetMenu(fileName = "NewItem", menuName = "Shop/Items")]
    public class ItemObjects : ScriptableObject
    {

        [Tooltip("Name of the Shop Item")]
        [SerializeField] public string ItemName;




    }
}
