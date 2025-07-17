using UnityEngine;

namespace ProjectCeros
{
    public class LaterWeekTutorial : MonoBehaviour
    {
        [SerializeField] private TutorialManager _manager;
        [SerializeField] private BoolReference _showedOnce;

        [SerializeField] private IntReference _weeks;

        
        public void StartDialogue()
        {
            if (!_showedOnce && _weeks == 2)
            { _manager.ShowClue(); }

        }


    }
}
