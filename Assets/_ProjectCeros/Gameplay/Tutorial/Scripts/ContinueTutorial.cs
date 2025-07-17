using UnityEngine;

namespace ProjectCeros
{
    public class ContinueTutorial : MonoBehaviour
    {
        [SerializeField] private TutorialManager _manager;
        [SerializeField] private BoolReference _showedOnce;

        
        void Start()
        {
            if (!_showedOnce)
            { _manager.ShowClue(); }

        }


    }
}
