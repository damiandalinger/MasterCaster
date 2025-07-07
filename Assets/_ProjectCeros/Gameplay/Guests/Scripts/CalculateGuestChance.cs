/// <summary>
/// This script determines the probability for a guest to arrive.
/// </summary>

/// <remarks>
/// 25/06/2025 by Unik Kelmendi: Initial creation.
/// </remarks>

using UnityEngine;

namespace ProjectCeros
{

    public class CalculateGuestChance : MonoBehaviour
    {
        [SerializeField] private IntReference _globalStars;

        [SerializeField] private IntReference _listeners;

        [SerializeField] private IntReference _oldThreshhold;

        [SerializeField] private IntReference _newThreshhold;


        [SerializeField] private float _fixedChance;

        [SerializeField] private float _baseChance;

        [SerializeField] private float _addedChance;

        [SerializeField] private float _multiplier;

        [SerializeField] private float _basePenalty = 0.7f;

        [SerializeField] private float _addedPenalty = 0.1f;


        [SerializeField] private GuestSORuntimeSet _guestDatabase;





        public void Start()
        {
           // SetGuestChance();
        }

        public void SetGuestChance()
        {
            foreach (var guest in _guestDatabase.Items)
            {

                guest.Chance = DetermineGuestChance(guest);


            }

            Debug.Log("Set guest chance is done");
        }


        public float DetermineGuestChance(GuestSO guest)
        {

            // If Stars of the Player are definitive higher than guest stars.
            if (_globalStars.Value > guest.Rating)
            {
                return _fixedChance;

            }

            // If the player has reached the max stars rating.
            else if (_globalStars.Value == 5)

            {
                return _fixedChance;
            }


            // If the Stars of the Player and the Guest are the same.
            else if (_globalStars.Value == guest.Rating)
            {
                _multiplier = Mathf.InverseLerp(_oldThreshhold, _newThreshhold, _listeners);

                float t = _multiplier * _addedChance;

                t += _baseChance;

                return t;

            }



            // If the player has definitive lower stars rating than the guest.
            else if (_globalStars.Value < guest.Rating)

            {
                _multiplier = Mathf.InverseLerp(_oldThreshhold, _newThreshhold, _listeners);

                float t = _multiplier * _addedChance * _addedPenalty;


                t += _baseChance * _basePenalty;

                return t;

            }

            
            else 
            {
                return _fixedChance;

            }


            
        }


    }

}