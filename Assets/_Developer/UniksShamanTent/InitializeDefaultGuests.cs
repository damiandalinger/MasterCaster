using System.Collections.Generic;
using UnityEngine;


namespace ProjectCeros

{
    public class InitializeDefaultGuests : MonoBehaviour
    {
        public IntRuntimeSet GuestIDs;

        [SerializeField] private List<int> _defaultUnlocks;


        public void UnlockBaseRoster()
        {
            foreach (int id in _defaultUnlocks)
            {
                GuestIDs.Add(id);

            }
        }



    }
}