using System.Collections.Generic;
using UnityEngine;


namespace ProjectCeros

{
    public class InitializeGuestScreens : MonoBehaviour
    {
    

        [SerializeField] private GameEvent gameEvent;


        public void Awake()
        {
            gameEvent.Raise();
        }



    }
}