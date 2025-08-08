using UnityEngine;

namespace ProjectCeros
{
    public class CheckWinCondition : MonoBehaviour
    {
        [SerializeField] private BoolReference _hasWon;

        [SerializeField] private BoolReference _activatedOnce;

        public GameObject WinningScreen;

        public void FinishGame()
        {
            if (_hasWon && !_activatedOnce)
            {
                WinningScreen.SetActive(true);

                _activatedOnce.Variable.SetValue(true);
            }
        }


    }
}
