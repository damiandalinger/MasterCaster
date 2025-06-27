using UnityEngine;
// using UnityEngine.Random;


namespace ProjectCeros

{

    public class DetermineGlobalStart : MonoBehaviour
    {

        [SerializeField] private IntReference _listerners;
        public IntReference GlobalStars;

        public IntReference OldThreshhold;

        public IntReference NewThreshhold;



        public void Start()
        {
            UpadteStarRating();
        }


        public void UpadteStarRating()
        {
            if (_listerners.Value > 0 && _listerners.Value <= 9)
            {
                GlobalStars.Variable.SetValue(0);
                OldThreshhold.Variable.SetValue(0);
                NewThreshhold.Variable.SetValue(9);


            }


            if (_listerners.Value >= 10 && _listerners.Value <= 99)
            {
                GlobalStars.Variable.SetValue(1);
                OldThreshhold.Variable.SetValue(10);
                NewThreshhold.Variable.SetValue(99);
            }


            if (_listerners.Value > 100 && _listerners.Value <= 999)
            {
                GlobalStars.Variable.SetValue(2);
                OldThreshhold.Variable.SetValue(100);
                NewThreshhold.Variable.SetValue(999);
            }


            if (_listerners.Value > 1000 && _listerners.Value <= 9999)
            {
                GlobalStars.Variable.SetValue(3);
                OldThreshhold.Variable.SetValue(1000);
                NewThreshhold.Variable.SetValue(9999);
            }


            if (_listerners.Value > 10000 && _listerners.Value <= 99999)
            {
                GlobalStars.Variable.SetValue(4);
                OldThreshhold.Variable.SetValue(10000);
                NewThreshhold.Variable.SetValue(99999);
            }


            if (_listerners.Value > 100000)
            {
                GlobalStars.Variable.SetValue(5);
                OldThreshhold.Variable.SetValue(10000);
                NewThreshhold.Variable.SetValue(10000);
            }

        }



    }

}