using UnityEngine;
// using UnityEngine.Random;


namespace ProjectCeros

{

    public class DetermineGlobalStart : MonoBehaviour
    {

        [SerializeField] private IntReference _listerners;
        public IntReference _globalStars;



        public void Start()
        {
            UpadteStarRating();
        }


        public void UpadteStarRating()
        {
            if (_listerners.Value > 0 && _listerners.Value <= 9)
            {
                _globalStars.Variable.SetValue(0);
            }


            if (_listerners.Value >= 10 && _listerners.Value <= 99)
            {
                _globalStars.Variable.SetValue(1);
            }


            if (_listerners.Value > 100 && _listerners.Value <= 999)
            {
                _globalStars.Variable.SetValue(2);
            }


            if (_listerners.Value > 1000 && _listerners.Value <= 9999)
            {
                _globalStars.Variable.SetValue(3);
            }


            if (_listerners.Value > 10000 && _listerners.Value <= 99999)
            {
                _globalStars.Variable.SetValue(4);
            }


            if (_listerners.Value > 100000)
            {
                _globalStars.Variable.SetValue(5);
            }

        }



    }

}