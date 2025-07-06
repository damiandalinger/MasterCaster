/// <summary>
/// Handles Dialogue displaying letter by letter and guests changing Color when speaking.
/// </summary>

/// <remarks>
/// 04/07/2025 by Unik Kelmendi: Initial creation.
/// </remarks>

using UnityEngine;
using TMPro; 
using System.Collections;


namespace ProjectCeros
{

    public class DialogueDisplay : MonoBehaviour
    {
        [SerializeField] private GuestDatabaseSO _allguests;
        [SerializeField] private DialogueManager _dialogueManager;
        [SerializeField] private TMP_Text dialogueText;
        [SerializeField] private float letterDelay = 0.05f;

        private string[] dialogueSegments;
        private int currentSegmentIndex;
        private bool isTyping;

        [SerializeField] private IntReference _topicID;
        [SerializeField] private IntReference _spinID;

        [SerializeField] private string _topic;

        [SerializeField] private bool _isPositive;


        [SerializeField] private bool _hasGuest = false;
        [SerializeField] private GuestSO _guest;
        [SerializeField] private Color colorPlayer = Color.black;
        [SerializeField] private Color colorGuest = new Color(0.1f, 0.1f, 0.4f); // dark blue
        private bool _toggleColor = false;

        void Start()
        {
            FindAcceptedGuest();

            // Example: wait for DialogueManager to load data
            if (!_hasGuest)
            {
                StartCoroutine(StartDialogueSequence());
            }

            else
            {
                StartCoroutine(StartDialogueGuestSequence());
            }
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

            //Debug.Log(topicMsg);

            yield return TypeDialogue(dialogueSegments[currentSegmentIndex]);
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



            yield return TypeDialogue(dialogueSegments[currentSegmentIndex]);
        }


        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                if (isTyping)
                {
                    // Finish instantly
                    StopAllCoroutines();
                    dialogueText.text = dialogueSegments[currentSegmentIndex];
                    isTyping = false;
                }
                else
                {
                    // Next segment
                    currentSegmentIndex++;
                    if (currentSegmentIndex < dialogueSegments.Length)
                    {
                        if (_hasGuest)
                        {
                            // Toggle the speaker flag
                            _toggleColor = !_toggleColor;

                            // Change text color depending on who's speaking
                            dialogueText.color = _toggleColor
                                ? colorGuest // Guest: dark blue
                                : colorPlayer;               // Player: black

                        }

                        StartCoroutine(TypeDialogue(dialogueSegments[currentSegmentIndex]));
                    }
                    else
                    {
                        Debug.Log("Dialogue finished!");
                        dialogueText.text = "";
                    }
                }
            }
        }

        IEnumerator TypeDialogue(string segment)
        {
            isTyping = true;
            dialogueText.text = "";

            Debug.Log($"Typing new dialogue segment: {segment}");

            foreach (char c in segment)
            {
                dialogueText.text += c;
                yield return new WaitForSeconds(letterDelay);
            }

            isTyping = false;
        }

        private void DetermineTopic(int id)
        {
            if (id == 1)
            {
                _topic = "Action";

            }

            if (id == 2)
            {
                _topic = "Indie";

            }
            if (id == 3)
            {
                _topic = "RPG";

            }
            if (id == 4)
            {
                _topic = "Shooter";

            }
            if (id == 5)
            {
                _topic = "Simulation";

            }
            if (id == 6)
            {
                _topic = "Strategy";

            }
            if (id == 7)
            {
                _topic = "???";

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
