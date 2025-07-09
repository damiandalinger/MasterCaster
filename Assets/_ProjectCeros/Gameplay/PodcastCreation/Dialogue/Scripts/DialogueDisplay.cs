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
        [SerializeField] private TMP_Text _namePlayer;
        [SerializeField] private TMP_Text _nameGuest;
        [SerializeField] private TMP_Text dialogueText;

        [SerializeField] private GameObject _soloBox;
        [SerializeField] private GameObject _playerBox;
        [SerializeField] private GameObject _guestBox;

        [SerializeField] private float letterDelay = 0.05f;

        private string[] dialogueSegments;
        private int currentSegmentIndex;
        private bool isTyping;
        private bool _isReady;

        [SerializeField] private IntReference _topicID;
        [SerializeField] private IntReference _spinID;

        [SerializeField] private string _topic;

        [SerializeField] private bool _isPositive;


        [SerializeField] private bool _hasGuest = false;
        [SerializeField] private GuestSO _guest;
        [SerializeField] private Podcaster _player;
        [SerializeField] private Color colorPlayer = Color.black;

        private bool _toggle = false;

        [SerializeField] private GameEvent _endDialogue;

        private Coroutine autoAdvanceCoroutine;

        [SerializeField] private float autoAdvanceDelay = 3f; // Customize in Inspector

        [SerializeField] private BackgroundFader _faderPositive;
        [SerializeField] private BackgroundFader _faderNegative;

        [SerializeField] private BackgroundFader _faderGuest;

        [SerializeField] private Image _guestImage;
        [SerializeField] private Image _guestUI;

        void Start()
        {
            SetupDialogue();

            _namePlayer.text = _player.PersonName;

        }

        public void SetupDialogue()
        {
            FindAcceptedGuest();

            // Example: wait for DialogueManager to load data
            if (!_hasGuest)
            {
                _soloBox.SetActive(true);
                _playerBox.SetActive(false);
                _guestBox.SetActive(false);

                _guestUI.enabled = false;
                _guestImage.enabled = false;

                StartCoroutine(StartDialogueSequence());
            }

            else
            {
                _soloBox.SetActive(false);
                _playerBox.SetActive(true);
                _guestBox.SetActive(false);

                _guestUI.enabled = true;
                _guestImage.enabled = true;
                _guestImage.sprite = _guest.GuestSprite;

                StartCoroutine(StartDialogueGuestSequence());
            }

            //AdvanceDialogue();
        }

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

        IEnumerator StartDialogueSequence()
        {
            DetermineTopic(_topicID.Value);
            DetermineSpin(_spinID.Value);




            Debug.Log(_topic);
            Debug.Log(_isPositive);


            // Wait one frame to make sure DialogueManager Awake() ran
            yield return null;



            if (_dialogueManager == null)
            {
                Debug.LogError("DialogueManager not found!");
                yield break;
            }

            // Prepare dialogue sequence
            string welcomeMsg = _dialogueManager.InjectVariables(_dialogueManager.GetRandomWelcome());
            string topicMsg = _dialogueManager.InjectVariables(_dialogueManager.GetTopicMessage(_topic, _isPositive));
            string goodbyeMsg = _dialogueManager.InjectVariables(_dialogueManager.GetRandomGoodbye());


            dialogueSegments = new string[] { welcomeMsg, topicMsg, goodbyeMsg };
            currentSegmentIndex = 0;

            Debug.Log(string.Join(",", dialogueSegments));

            _isReady = true;
            AdvanceDialogue();
            //yield return TypeDialogue(dialogueSegments[currentSegmentIndex]);
            //AdvanceDialogue();
        }

        IEnumerator StartDialogueGuestSequence()
        {

            _dialogueManager.SetGuest(_guest);
            _dialogueManager.GetGuestDialogue();

            // Wait one frame to make sure DialogueManager Awake() ran
            yield return null;



            if (_dialogueManager == null)
            {
                Debug.LogError("DialogueManager not found!");
                yield break;
            }

            // Prepare dialogue sequence
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
            //yield return TypeDialogue(dialogueSegments[currentSegmentIndex]);
            //AdvanceDialogue();
        }


        // Call this method from a UI Button (via OnClick)
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
                dialogueText.text = dialogueSegments[currentSegmentIndex - 1]; // Show full segment
                isTyping = false;
            }
            else
            {
                AdvanceDialogue();
            }
        }

        public void AdvanceDialogue()
        {
            if (_isReady)
            {
                if (isTyping) return;

                if (currentSegmentIndex >= dialogueSegments.Length)
                {
                    dialogueText.text = "";
                    Debug.Log("Dialogue finished!");

                    StartCoroutine(EndDialogueSequence());
                    return;
                }

                if (_hasGuest)
                {
                    // Change text color depending on who's speaking
                    dialogueText.color = _toggle
                        ? _guest.Color // Guest: dark blue
                        : colorPlayer;               // Player: black


                    _namePlayer.text = _toggle ? null : _player.PersonName;
                    _nameGuest.text = _toggle ? _guest.Name : null;
                    _nameGuest.color = _guest.Color;

                    _playerBox.SetActive(!_toggle);
                    _guestBox.SetActive(_toggle);
                    // Toggle the speaker flag
                    _toggle = !_toggle;
                }

                else
                {
                    dialogueText.color = colorPlayer;

                    _namePlayer.text = _player.PersonName;
                    _nameGuest.text = null;

                    _playerBox.SetActive(true);
                    _guestBox.SetActive(false);

                }


                if (_hasGuest)
                {
                    _faderGuest.ChangeBackground();
                }

                else if (_spinID.Value == 1)
                {
                    _faderPositive.ChangeBackground();
                }

                else
                {
                    _faderNegative.ChangeBackground();
                }

                // Start typing the current segment
                StartCoroutine(TypeDialogue(dialogueSegments[currentSegmentIndex]));
                currentSegmentIndex++;
            }
        }

        private IEnumerator EndDialogueSequence()
        {
            _faderPositive.FadeOut();
            yield return new WaitForSeconds(1.5f);
            _endDialogue.Raise();
        }


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

        IEnumerator AutoAdvanceAfterDelay()
        {
            yield return new WaitForSeconds(autoAdvanceDelay);
            AdvanceDialogue();
        }


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

        public void DetermineSpin(int id)
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
