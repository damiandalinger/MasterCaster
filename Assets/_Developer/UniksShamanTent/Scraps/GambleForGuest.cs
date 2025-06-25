using UnityEngine;

namespace ProjectCeros
{

    public class GambleForGuest : MonoBehaviour
    {
        [SerializeField] private IntReference _globalStars;

        [SerializeField] private IntReference _listeners;

        // [SerializeField] private GuestSO guest;

        [SerializeField] private float _baseChance;

        [SerializeField] private float _addedChance;

        [SerializeField] private float _multiplier;

        [SerializeField] private float _oldThreshhold;

        [SerializeField] private float _nextThreshhold;




        public void GambleGuest(GuestSO guest)
        {

            //If Stars of the Player are definitive higher thant guest stars
            if (_globalStars.Value > guest.Rating)
            {
                float random = Random.value;

                if (random < 0.95)
                {
                    guest.hasAccepted = true;
                }

            }

            else
            {
                _multiplier = Mathf.InverseLerp(_oldThreshhold, _nextThreshhold, _listeners);

                float t = _multiplier * _addedChance;

                t += _baseChance;

                float random = Random.value;

                if (random < t)

                {
                    guest.hasAccepted = true;
                    Debug.Log("OHH YEAAH");
                }

                else

                {
                    Debug.Log("No guest, sad");
                }


            }

        }


    }

}