/// <summary>
/// This script is attached to the GameObjects that represent the items in the room when they get enabled.
/// The GameObjects hold the script and the script only holds the corresponding ItemSO.
/// </summary>

/// <remarks>
/// 18/06/2025 by Unik Kelmendi: Initial creation.
/// </remarks>

using UnityEngine;

namespace ProjectCeros
{
    public class RoomItemDisplay : MonoBehaviour
    {
        public ItemSO ItemData;
    }
}