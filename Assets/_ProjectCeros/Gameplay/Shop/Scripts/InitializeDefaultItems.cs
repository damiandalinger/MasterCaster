/// <summary>
/// This script adds all the default Items that are in the game from the start.
/// </summary>

/// <remarks>
/// 25/06/2025 by Unik Kelmendi: Initial creation.
/// </remarks>

using System.Collections.Generic;
using UnityEngine;


namespace ProjectCeros

{
    public class InitializeDefaultItems : MonoBehaviour
    {
        public IntRuntimeSet ItemID;

        [SerializeField] private List<int> _defaultUnlocks;


        public void Awake()
        {
            UnlockBaseItemsRoster();
        }

        public void UnlockBaseItemsRoster()
        {
            foreach (int id in _defaultUnlocks)
            {
                ItemID.Add(id);

            }
        }



    }
}