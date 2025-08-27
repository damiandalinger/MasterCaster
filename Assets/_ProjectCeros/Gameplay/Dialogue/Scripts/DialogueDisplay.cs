/// <summary>
/// Handles Dialogue displaying letter by letter and guests changing Color when speaking.
/// </summary>

/// <remarks>
/// 04/07/2025 by Unik Kelmendi: Initial creation.
/// </remarks>

using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;


namespace ProjectCeros
{
    public class DialogueDisplay : MonoBehaviour
    {
        [SerializeField] private GuestDatabaseSO _allguests;
        [SerializeField] private DialogueManager _dialogueManager;
        [SerializeField] private GameEvent _endDialogue;


        [SerializeField] private BackgroundFader _faderPositive;
        [SerializeField] private BackgroundFader _faderNegative;
        [SerializeField] private BackgroundFader _faderGuest;

        [SerializeField] private BackgroundFader _faderPositiveM;
        [SerializeField] private BackgroundFader _faderNegativeM;
        [SerializeField] private BackgroundFader _faderGuestM;

        [SerializeField] private IntReference _topicID;
        [SerializeField] private IntReference _spinID;
        [SerializeField] private StringReference _playerName;

        [SerializeField] private TMP_Text _playerNameText;
        [SerializeField] private TMP_Text _playerNameDialogueText;
        [SerializeField] private TMP_Text _guestNameText;
        [SerializeField] private TMP_Text dialogueText;

        [SerializeField] private GameObject _soloBox;
        [SerializeField] private GameObject _playerBox;
        [SerializeField] private GameObject _guestBox;
        [SerializeField] private GameObject _guestUI;

        [SerializeField] private string _topic;
        [SerializeField] private bool _isPositive;

        [SerializeField] private bool _hasGuest = false;
        [SerializeField] private GuestSO _guest;
        [SerializeField] private Image _guestImage;
        [SerializeField] private Color colorPlayer = Color.black;

        [SerializeField] private float letterDelay = 0.05f;
        [SerializeField] private float autoAdvanceDelay = 3f; // Customize in Inspector

        private Coroutine autoAdvanceCoroutine;
        private string[] dialogueSegments;
        private int currentSegmentIndex;
        private bool isTyping;
        private bool _isReady;
        private bool _toggle = false;
        private bool _markus;


        void Start()
        {
            SetupDialogue();

            _playerNameText.text = _playerName.Value;
            _playerNameDialogueText.text = _playerName.Value;

            if (_playerName.Value.Contains("Markus"))
            {
                _markus = true;
            }

            else
            {
                _markus = false;
            }
        }

        // This is the button that force advances the dialogue.
        public void OnDialogueClick()
        {
            if (autoAdvanceCoroutine != null)
            {
                StopCoroutine(autoAdvanceCoroutine);
                autoAdvanceCoroutine = null;
            }

            if (isTyping)
            {
                StopAllCoroutines();
                dialogueText.text = dialogueSegments[currentSegmentIndex - 1];
                isTyping = false;
            }

            else
            {
                AdvanceDialogue();
            }
        }

        // This method sets up the UI and starts the dialogue screen.
        public void SetupDialogue()
        {
            FindAcceptedGuest();

            if (!_hasGuest)
            {
                _soloBox.SetActive(true);
                _playerBox.SetActive(false);
                _guestBox.SetActive(false);

                _guestUI.SetActive(false);
                _guestImage.enabled = false;

                StartCoroutine(StartDialogueSequence());
            }

            else
            {
                _soloBox.SetActive(false);
                _playerBox.SetActive(true);
                _guestBox.SetActive(false);

                _guestUI.SetActive(true);
                _guestImage.enabled = true;
                _guestImage.sprite = _guest.GuestSpriteDialogue;

                StartCoroutine(StartDialogueGuestSequence());
            }
        }


        #region AdvanceDialogue
        //This Method handles how the dialogue segments that are saved get advanced throughout the dialogue.
        public void AdvanceDialogue()
        {
            if (_isReady)
            {
                if (isTyping) return;

                if (currentSegmentIndex >= dialogueSegments.Length)
                {
                    dialogueText.text = "";
                    Debug.Log("Dialogue finished!");
                    _endDialogue.Raise();
                    return;
                }

                if (_hasGuest)
                {
                    // Change text color depending on who's speaking
                    dialogueText.color = _toggle
                        ? _guest.Color
                        : colorPlayer;

                    _playerNameDialogueText.text = _playerName.Value;

                    _guestNameText.text = _guest.Name;
                    _guestNameText.color = _guest.Color;

                    _playerBox.SetActive(!_toggle);
                    _guestBox.SetActive(_toggle);

                    _toggle = !_toggle;
                }

                else
                {
                    dialogueText.color = colorPlayer;

                    _playerNameDialogueText.text = _playerName.Value;
                    _guestNameText.text = null;

                    _playerBox.SetActive(true);
                    _guestBox.SetActive(false);
                }


                if (_hasGuest)
                {
                    if (!_markus)
                    {
                        _faderGuest.ChangeBackground();
                    }

                    else
                    {
                        _faderGuestM.ChangeBackground();
                    }
                }

                else if (_spinID.Value == 1)
                {
                    if (!_markus)
                    {
                        _faderPositive.ChangeBackground();
                    }
                    else
                    {
                        _faderPositiveM.ChangeBackground();
                    }
                }

                else
                {
                    if (!_markus)
                    {
                        _faderNegative.ChangeBackground();
                    }
                    else
                    {
                        _faderNegativeM.ChangeBackground();
                    }
                }

                // Start typing the current segment
                StartCoroutine(TypeDialogue(dialogueSegments[currentSegmentIndex]));
                currentSegmentIndex++;
            }
        }

        #endregion


        #region FindAcceptedGuest
        // This script checks if the player has aqcuired a guest.
        private void FindAcceptedGuest()
        {
            _guest = null;

            foreach (var guest in _allguests.AllGuests)
            {
                if (guest.hasAccepted)
                {
                    _guest = guest;
                }
            }

            if (_guest != null)
            {
                _hasGuest = true;
            }

            else
            {
                _hasGuest = false;
            }
        }
        #endregion


        #region DialogueSequencing
        // These methods are the coroutines that start when the dialogue is finished being set up.
        IEnumerator StartDialogueSequence()
        {
            DetermineTopic(_topicID.Value);
            DetermineSpin(_spinID.Value);

            Debug.Log(_topic);
            Debug.Log(_isPositive);

            yield return null;

            if (_dialogueManager == null)
            {
                Debug.LogError("DialogueManager not found!");
                yield break;
            }

            string welcomeMsg = _dialogueManager.InjectVariables(_dialogueManager.GetRandomWelcome());
            string topicMsg = _dialogueManager.InjectVariables(_dialogueManager.GetTopicMessage(_topic, _isPositive));
            string goodbyeMsg = _dialogueManager.InjectVariables(_dialogueManager.GetRandomGoodbye());

            dialogueSegments = new string[] { welcomeMsg, topicMsg, goodbyeMsg };
            currentSegmentIndex = 0;

            _isReady = true;
            AdvanceDialogue();
        }

        IEnumerator StartDialogueGuestSequence()
        {
            _dialogueManager.SetGuest(_guest);
            _dialogueManager.GetGuestDialogue();

            yield return null;

            if (_dialogueManager == null)
            {
                Debug.LogError("DialogueManager not found!");
                yield break;
            }

            string PlayerHelloMsg = _dialogueManager.InjectVariables(_dialogueManager.GetRandomGuestWelcome()); //fixed
            string GuestHelloMsg = _dialogueManager.InjectVariables(_dialogueManager.GetGuestHello());

            string PlayerMainMsg = _dialogueManager.InjectVariables(_dialogueManager.GetRandomGuestMain()); //fixed
            string GuestMainMsg = _dialogueManager.InjectVariables(_dialogueManager.GetGuestPersonal());

            string PlayerbyeMsg = _dialogueManager.InjectVariables(_dialogueManager.GetRandomGuestGoodbye()); //fixed
            string GuestByeMsg = _dialogueManager.InjectVariables(_dialogueManager.GetGuestBye());

            dialogueSegments = new string[] { PlayerHelloMsg, GuestHelloMsg, PlayerMainMsg, GuestMainMsg, PlayerbyeMsg, GuestByeMsg };
            currentSegmentIndex = 0;

            _isReady = true;
            AdvanceDialogue();
        }
        #endregion


        // This method automatically advances the dialogue after a delay.
        IEnumerator AutoAdvanceAfterDelay()
        {
            yield return new WaitForSeconds(autoAdvanceDelay);
            AdvanceDialogue();
        }

        // This coroutine types out the dialogue snippets letter by letter.
        IEnumerator TypeDialogue(string segment)
        {
            isTyping = true;
            dialogueText.text = "";


            string[] words = segment.Split(' ');
            bool isFirstWord = true;

            foreach (string word in words)
            {
                string wordWithSpace = word + " ";

                // Only predict line break if it's not the first word
                bool causesLineBreak = false;
                if (!isFirstWord)
                {
                    // Predict wrapping
                    dialogueText.ForceMeshUpdate();
                    int linesBefore = dialogueText.textInfo.lineCount;

                    string testText = dialogueText.text + wordWithSpace;
                    dialogueText.text = testText;
                    dialogueText.ForceMeshUpdate();

                    int linesAfter = dialogueText.textInfo.lineCount;

                    dialogueText.text = dialogueText.text.Remove(dialogueText.text.Length - wordWithSpace.Length); // revert

                    causesLineBreak = linesAfter > linesBefore;
                }

                if (causesLineBreak)
                {
                    dialogueText.text += "\n";
                }

                foreach (char c in wordWithSpace)
                {
                    dialogueText.text += c;
                    yield return new WaitForSeconds(letterDelay);
                }

                isFirstWord = false;
            }

            isTyping = false;

            autoAdvanceCoroutine = StartCoroutine(AutoAdvanceAfterDelay());
        }

        // This method determins the topic for the DialogueManager.
        private void DetermineTopic(int id)
        {
            switch (id)
            {
                case 1: _topic = "Action"; break;
                case 2: _topic = "Indie"; break;
                case 3: _topic = "RPG"; break;
                case 4: _topic = "Shooter"; break;
                case 5: _topic = "Simulation"; break;
                case 6: _topic = "Strategy"; break;
                case 7: _topic = "???"; break;
            }
        }

        // This method determins the topic spin for the DialogueManager.
        private void DetermineSpin(int id)
        {
            if (id == 1)
            {
                _isPositive = true;
            }

            else
            {
                _isPositive = false;
            }
        }
    }
}
