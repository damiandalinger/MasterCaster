/// <summary>
/// This script unlocks all the possible guests that are not in the game by default.
/// </summary>

/// <remarks>
/// 25/06/2025 by Unik Kelmendi: Initial creation.
/// </remarks>

using UnityEngine;

namespace ProjectCeros

{
    public class UnlockGuests : MonoBehaviour
    {
        [SerializeField] private IntRuntimeSet _equipment;

        [SerializeField] private IntReference _globalStars;


        public IntRuntimeSet GuestIDs;

        public void Start()
        {
            UnlockGuest();
        }


        public void UnlockGuest()
        {
            // Unlock by ShopItem
            foreach (int id in _equipment.Items)
            {
                //Unlock Trapclap
                if (id == 101)
                {
                    GuestIDs.Add(1);

                }

                //Unlock Hollowknight
                if (id == 103)
                {
                    GuestIDs.Add(6);

                }

                //Unlock Firekeeper
                if (id == 106)
                {
                    GuestIDs.Add(13);

                }

            }

            // Unlock guests based on Strarrating.
            if (_globalStars >= 0)
            {
                GuestIDs.Add(2);
                GuestIDs.Add(3);
                GuestIDs.Add(4);
            }

            if (_globalStars >= 1)
            {
                GuestIDs.Add(5);
                GuestIDs.Add(7);

            }

            if (_globalStars >= 2)
            {
                GuestIDs.Add(8);
                GuestIDs.Add(9);
                GuestIDs.Add(10);
            }

            if (_globalStars >= 3)
            {
                GuestIDs.Add(11);
                GuestIDs.Add(12);

            }

            if (_globalStars >= 4)
            {
                GuestIDs.Add(14);
                GuestIDs.Add(15);
                GuestIDs.Add(16);
            }





        }



    }



}