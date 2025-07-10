using UnityEngine;


namespace ProjectCeros
{
    public class PlayStartSound : MonoBehaviour
    {

        [SerializeField] private GameEvent _play;


        public void Start()
        {
            _play.Raise();
        }

    }

}