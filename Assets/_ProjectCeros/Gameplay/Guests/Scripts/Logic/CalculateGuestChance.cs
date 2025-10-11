/// <summary>
/// This script determines the probability for a guest to arrive. It allows designers to adjust probabilities in the engine.
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

        [SerializeField] private GuestSORuntimeSet _guestDatabase;

        [Header("Guest Calculation Values")]
        [Header("Player Stars higher than Guest Stars")]
        [Tooltip("This is the chance of a Guest appearing when the player has a higher star rating than the guest.")]
        [SerializeField] private float _fixedChance;

        [Header("Player Stars same as Guest Stars")]
        [Tooltip("This is the base probality for a guest arriving.")]
        [SerializeField] private float _baseChance;

        [Tooltip("base chance + added chance * multiplier determine the final chance of the guest appearing.")]
        [SerializeField] private float _addedChance;

        [Tooltip("This is the multiplier for the added chance, it is determined by how far away the player is to the next Starrating (Handled by code).")]
        [SerializeField] private float _multiplier;

        [Header("Player Stars lower than Guest Stars")]
        [Tooltip("By this amount the base chance is multiplied when inviting a guest with a higher star rating")]
        [SerializeField] private float _basePenalty = 0.7f;
        [Tooltip("By this amount the added chance is multiplied when inviting a guest with a higher star rating")]
        [SerializeField] private float _addedPenalty = 0.1f;


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
            if (_globalStars.Value > guest.Rating || guest.Rating == 0)
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