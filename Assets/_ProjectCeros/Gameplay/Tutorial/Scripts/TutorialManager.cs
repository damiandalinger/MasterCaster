using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace ProjectCeros
{
    public class TutorialManager : MonoBehaviour
    {
        [SerializeField] private BoolReference _wantsTutorial;
        [SerializeField] private BoolReference _showedOnce;

        [SerializeField] private TextMeshProUGUI _headingBox;
        [SerializeField] private TextMeshProUGUI _textBox;

        [SerializeField] private GameObject _uiElement;

        [SerializeField] private string _heading;

        [TextArea(3, 10)]
        [SerializeField] private string _text;


        public void FirstTutioralPrompt()
        {
            if (!_showedOnce)
            {
                _headingBox.text = _heading;
                _textBox.text = _text;
                _uiElement.SetActive(true);
            }
        }


        public void ShowClue()
        {
            if (_wantsTutorial && !_showedOnce)
            {
                _headingBox.text = _heading;
                _textBox.text = _text;
                _uiElement.SetActive(true);

                Debug.Log("Show clue!");
            }

        }

        public void WantsTutorial()
        {
            _wantsTutorial.Variable.SetValue(true);
            _uiElement.SetActive(false);
            _showedOnce.Variable.SetValue(true);

            Debug.Log("wants tutorial yes!");
        }

        public void WantsNoTutorial()
        {
            _wantsTutorial.Variable.SetValue(false);
            _uiElement.SetActive(false);
            _showedOnce.Variable.SetValue(true);
        }


        public void ClueRead()
        {
            _uiElement.SetActive(false);
            _showedOnce.Variable.SetValue(true);
        }


    }

}