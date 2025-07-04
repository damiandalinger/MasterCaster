using UnityEngine;
using TMPro; // Use this for TextMeshPro; for regular Text UI use UnityEngine.UI
using System.Collections;
using System.Data.Common;

namespace ProjectCeros
{

    public class DialogueDisplay : MonoBehaviour
    {
        public TMP_Text dialogueText;
        public float letterDelay = 0.05f;

        private string[] dialogueSegments;
        private int currentSegmentIndex;
        private bool isTyping;

        [SerializeField] private IntReference _topicID;

        [SerializeField] private string _topic;

        [SerializeField] private IntReference _spinID;

        [SerializeField] private bool _isPositive;

        [SerializeField] private bool _hasGuest = false;

        [SerializeField] private GuestSO _guest;

        [SerializeField] private GuestDatabaseSO _allguests;

        void Start()
        {
            FindAccptedGuest();

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

        private void FindAccptedGuest()
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

            DialogueManager dm = FindFirstObjectByType<DialogueManager>();

            if (dm == null)
            {
                Debug.LogError("DialogueManager not found!");
                yield break;
            }

            // Prepare dialogue sequence
            string welcomeMsg = dm.InjectVariables(dm.GetRandomWelcome());
            string topicMsg = dm.InjectVariables(dm.GetTopicMessage(_topic, _isPositive));
            string goodbyeMsg = dm.InjectVariables(dm.GetRandomGoodbye());


            dialogueSegments = new string[] { welcomeMsg, topicMsg, goodbyeMsg };
            currentSegmentIndex = 0;

            //Debug.Log(topicMsg);

            yield return TypeDialogue(dialogueSegments[currentSegmentIndex]);
        }

         IEnumerator StartDialogueGuestSequence()
        {
            DetermineTopic(_topicID.Value);
            DetermineSpin(_spinID.Value);

            Debug.Log(_topic);
            Debug.Log(_isPositive);


            // Wait one frame to make sure DialogueManager Awake() ran
            yield return null;

            DialogueManager dm = FindFirstObjectByType<DialogueManager>();

            if (dm == null)
            {
                Debug.LogError("DialogueManager not found!");
                yield break;
            }

            // Prepare dialogue sequence
            string PlayerHelloMsg = dm.InjectVariables(dm.GetRandomGuestWelcome()); //fixed
            string GuestHelloMsg = dm.InjectVariables(dm.GetGuestHello(_guest.GuestID));

            string PlayerMainMsg = dm.InjectVariables(dm.GetRandomGuestMain()); //fixed
            string GuestMainMsg = dm.InjectVariables(dm.GetGuestMain(_guest.GuestID));

            string PlayerbyeMsg = dm.InjectVariables(dm.GetRandomGuestGoodbye()); //fixed
            string GuestByeMsg = dm.InjectVariables(dm.GetGuestBye(_guest.GuestID));

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
