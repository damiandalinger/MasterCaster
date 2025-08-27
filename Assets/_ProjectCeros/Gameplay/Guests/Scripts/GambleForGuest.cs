/// <summary>
/// This script calculates if the guest will come or not by comparing the chance with a random value.
/// </summary>

/// <remarks>
/// 25/06/2025 by Unik Kelmendi: Initial creation.
/// </remarks>

using ProjectCeros.UI;
using UnityEngine;

namespace ProjectCeros
{

    public class GambleForGuest : MonoBehaviour
    {
        [SerializeField] private IntReference _globalStars;

        [SerializeField] private IntReference _listeners;

        [SerializeField] private IntReference _oldThreshhold;

        [SerializeField] private IntReference _newThreshhold;

        [SerializeField] private StringReference _outcomeText;

        [SerializeField] private ScreenFader _fader;


        public void GambleGuest(GuestSO guest)
        {

            // If Stars of the Player are definitive higher than guest stars.
            if (_globalStars.Value > guest.Rating)
            {
                float random = Random.value;

                Debug.Log($"Success probability: {guest.Chance}");

                Debug.Log($"Random roll: {random}");

                if (random < guest.Chance)
                {
                    guest.hasAccepted = true;
                    Debug.Log("OHH YEAAH, GUEST IS COMING");
                    _outcomeText.Variable.SetValue("You have a guest!");
                }

                else
                {
                    Debug.Log("No guest, sad");
                    _outcomeText.Variable.SetValue("Your Guest request was declined. How unfortunate!");
                }
            }

            // If the player has reached the max stars rating.
            else if (_globalStars.Value == 5)
            {
                float random = Random.value;

                Debug.Log($"Success probability: {guest.Chance}");

                Debug.Log($"Random roll: {random}");

                if (random < guest.Chance)
                {
                    guest.hasAccepted = true;
                    Debug.Log("OHH YEAAH, GUEST IS COMING");
                    _outcomeText.Variable.SetValue("You have a guest!");
                }

                else
                {
                    Debug.Log("No guest, sad");
                    _outcomeText.Variable.SetValue("Your Guest request was declined. How unfortunate!");

                }
            }


            // If the Stars of the Player and the Guest are the same.
            else if (_globalStars.Value == guest.Rating)
            {
                float random = Random.value;

                Debug.Log($"Success probability: {guest.Chance}");

                Debug.Log($"Random roll: {random}");


                if (random < guest.Chance)
                {
                    guest.hasAccepted = true;
                    Debug.Log("OHH YEAAH, GUEST IS COMING");
                    _outcomeText.Variable.SetValue("You have a guest!");
                }

                else
                {
                    Debug.Log("No guest, sad");
                    _outcomeText.Variable.SetValue("Your Guest request was declined. How unfortunate!");
                }
            }

            // If the player has definitive lower stars rating than the guest.
            else if (_globalStars.Value < guest.Rating)
            {
                float random = Random.value;


                Debug.Log($"Success probability: {guest.Chance}");

                Debug.Log($"Random roll: {random}");

                if (random < guest.Chance)
                {
                    guest.hasAccepted = true;
                    Debug.Log("OHH YEAAH, GUEST IS COMING");
                    _outcomeText.Variable.SetValue("You have a guest!");
                }

                else
                {
                    Debug.Log("No guest, sad");
                    _outcomeText.Variable.SetValue("Your Guest request was declined. How unfortunate!");
                }
            }

            _fader.FadeTransition();
        }
    }
}