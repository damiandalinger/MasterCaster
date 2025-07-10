using UnityEngine;

namespace ProjectCeros
{
    public class StartTutorial : MonoBehaviour
    {
        [SerializeField] private TutorialManager _manager;
        [SerializeField] private BoolReference _showedOnce;

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            if (!_showedOnce)
            { _manager.FirstTutioralPrompt(); }

        }


    }
}
