using UnityEngine;

namespace ProjectCeros
{

    public class GambleForGuest : MonoBehaviour
    {
        [SerializeField] private IntReference _globalStars;

        [SerializeField] private IntReference _listeners;

        [SerializeField] private IntReference _oldThreshhold;

        [SerializeField] private IntReference _newThreshhold;


        // [SerializeField] private GuestSO guest;

        [SerializeField] private float _baseChance;

        [SerializeField] private float _addedChance;

        [SerializeField] private float _multiplier;

        // [SerializeField] private float _oldThreshhold;

        // [SerializeField] private float _nextThreshhold;




        public void GambleGuest(GuestSO guest)
        {

            // If Stars of the Player are definitive higher than guest stars.
            if (_globalStars.Value > guest.Rating)
            {
                float random = Random.value;

                Debug.Log($"Success probability: {95}");

                Debug.Log($"Random roll: {random}");

                if (random < 0.95)
                {
                    guest.hasAccepted = true;
                    Debug.Log("OHH YEAAH, GUEST IS COMING");


                }

                else

                {
                    Debug.Log("No guest, sad");
                }

            }

            // If the player has reached the max stars rating.
            else if (_globalStars.Value == 5)

            {
                float random = Random.value;

                Debug.Log($"Success probability: {95}");

                Debug.Log($"Random roll: {random}");

                if (random < 0.95)
                {
                    guest.hasAccepted = true;
                    Debug.Log("OHH YEAAH, GUEST IS COMING");
                }

                else

                {
                    Debug.Log("No guest, sad");
                }

            }


            // If the Stars of the Player and the Guest are the same.
            else if (_globalStars.Value == guest.Rating)
            {
                _multiplier = Mathf.InverseLerp(_oldThreshhold, _newThreshhold, _listeners);

                float t = _multiplier * _addedChance;

                t += _baseChance;

                float random = Random.value;

                Debug.Log($"Success probability: {t * 100}");

                Debug.Log($"Random roll: {random}");


                if (random < t)
                {
                    guest.hasAccepted = true;
                    Debug.Log("OHH YEAAH, GUEST IS COMING");
                }

                else

                {
                    Debug.Log("No guest, sad");
                }
            }

            // If the player has definitive lower stars rating than the guest.
            else if (_globalStars.Value < guest.Rating)

            {
                _multiplier = Mathf.InverseLerp(_oldThreshhold, _newThreshhold, _listeners);

                float t = _multiplier * _addedChance * 0.1f;


                t += _baseChance * 0.7f;

                float random = Random.value;



                Debug.Log($"Success probability: {t * 100}");

                Debug.Log($"Random roll: {random}");

                if (random < t)
                {
                    guest.hasAccepted = true;
                    Debug.Log("OHH YEAAH, GUEST IS COMING");

                }

                else

                {
                    Debug.Log("No guest, sad");
                }


            }

        }


    }

}