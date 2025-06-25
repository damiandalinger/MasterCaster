using UnityEngine;
// using UnityEngine.Random;


namespace ProjectCeros

{

    public class ListenerValueTest : MonoBehaviour
    {

        public int StarRating;

        [SerializeField] private IntReference _listerners;

        [SerializeField] private int _nextThreshhold;

        [SerializeField] private float _nextStarDistance;




        public void GambleGuest(GuestSO guest)
        {
            if (_listerners.Value > 0 && _listerners.Value <= 9)

            {
                Debug.Log("StarRating is 0");

                _nextThreshhold = 9;

                _nextStarDistance = (float)_listerners.Value / _nextThreshhold;



                Debug.Log($"Distance to the next Star in % {_nextStarDistance}");

                _nextStarDistance = _nextStarDistance * 70;
                _nextStarDistance += 25;

                Debug.Log($"Guest Chance for 0 StarRating is {_nextStarDistance}% ");

                float random = Random.value;

                Debug.Log($"Random roll is {random}");


            }


            else if (_listerners.Value >= 10 && _listerners.Value <= 99)


            {
                Debug.Log("StarRating is 1");

                _nextThreshhold = 99;

                _nextStarDistance = (float)_listerners.Value / _nextThreshhold;



                Debug.Log($"Distance to the next Star in % {_nextStarDistance}");

                _nextStarDistance = _nextStarDistance * 70;
                _nextStarDistance += 25;

                Debug.Log($"Guest Chance for 1 StarRating is {_nextStarDistance}% ");


            }



        }


    }

}