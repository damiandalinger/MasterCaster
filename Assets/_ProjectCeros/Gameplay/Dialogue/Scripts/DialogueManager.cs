/// <summary>
/// Extracts the proper Dialogues from the DialogueText.json. Replaces keywords and chooses random messages.
/// </summary>

/// <remarks>
/// 04/07/2025 by Unik Kelmendi: Initial creation.
/// </remarks>

using System.Collections.Generic;
using System.IO;
using UnityEngine;
using System;

namespace ProjectCeros
{

    public class DialogueManager : MonoBehaviour
    {
        public string fileName = "DialogueText.json";
        
        [SerializeField] private StringReference _podcastName;

        [SerializeField] private GuestSO _guest;

        private Dictionary<string, string> dialogueVariables = new Dictionary<string, string>();

        [SerializeField] private string[] _guestHello;
        [SerializeField] private string[] _guestPersonal;
        [SerializeField] private string[] _guestBye;

        private DialogueData dialogueData;

        void Awake()
        {
            LoadDialogueData();

            SetDialogueVariable("Podcastname", _podcastName);
        }

        // Parse the data of the Dialogue JSON file to the DialogueData class.
        public void LoadDialogueData()
        {
            string path = Path.Combine(Application.streamingAssetsPath, fileName);
            if (File.Exists(path))
            {
                string json = File.ReadAllText(path);
                dialogueData = JsonUtility.FromJson<DialogueData>(json);
            }
            else
            {
                Debug.LogError("Dialogue file not found at: " + path);
            }
        }

        // This Method determines which Placeholders(Key) in the JSON file will be replaced by what words(value) once 
        // they get injected by InjectVariables.
        public void SetDialogueVariable(string key, string value)
        {
            dialogueVariables[key] = value;
        }

        // This Method replaces the Placeholders in the JSON files with the words that are determined in SetDialogueVariable.
        public string InjectVariables(string rawText)
        {
            foreach (var pair in dialogueVariables)
            {
                string placeholder = $"<{pair.Key}>";
                rawText = rawText.Replace(placeholder, pair.Value);
            }

            // Remove any leftover <UnknownTag>
            return System.Text.RegularExpressions.Regex.Replace(rawText, @"<[^<>]+>", "");
        }

        #region Solo Dialogue
        // Solo dialogues consist of 3 textbits, A welcome message, a topic related message and a godbye message.
        // These methods pick a random text from the pool of possible messages.
        public string GetRandomWelcome()
        {
            return GetRandomFromList(dialogueData?.welcome);
        }

        public string GetRandomGoodbye()
        {
            return GetRandomFromList(dialogueData?.goodbye);
        }

        // This method determines the Topic message. When called, this method receives a boolean that represents the wether
        // or not the player chose to rant or to praise. It also receives a boolean that determines the genre from which a message
        // was selected.
        public string GetTopicMessage(string topic, bool positive)
        {
            if (dialogueData == null || dialogueData.topics == null)
            {
                Debug.LogWarning("Dialogue data or topics is null");
                return "???";
            }

            Topic entry = Array.Find(dialogueData.topics, t => t.topicName == topic);
            if (entry == null)
            {
                Debug.LogWarning($"Topic '{topic}' not found");
                return "???";
            }

            string[] pool = positive ? entry.messages.positive : entry.messages.negative;

            if (pool == null || pool.Length == 0)
            {
                Debug.LogWarning($"No messages found for topic '{topic}' with sentiment {(positive ? "positive" : "negative")}");
                return "???";
            }

            string chosenMessage = GetRandomFromList(pool);

            Debug.Log($"Selected topic message ({topic} - {(positive ? "positive" : "negative")}): {chosenMessage}");

            return chosenMessage;
        }
        #endregion

        #region Guest Dialogue

        // This method changes the name of the current guest to the successfully invited one. It also sets up the the dialogue lines of the
        // guest from which the other methods can later randomly choose.
        public void GetGuestDialogue()
        {
            SetDialogueVariable("Guest", _guest.Name);

            if (dialogueData == null || dialogueData.topics == null)
            {
                Debug.LogWarning("Dialogue data or topics is null");
            }

            GuestAnswer guest = null;
            foreach (var g in dialogueData.GuestAnswers)
            {
                if (g.GuestID == _guest.GuestID)
                {
                    guest = g;
                    break;
                }
            }

            _guestHello = guest.hello;
            _guestPersonal = guest.personal;
            _guestBye = guest.bye;
        }

        // These methods fetch the dialogue messages for when the player has succesfully invited a guest. These dialogues consist of 6 
        // textbits that each get chosen randomly.
        public string GetGuestHello()
        {
            return GetRandomFromList(_guestHello);
        }

        public string GetGuestPersonal()
        {
            return GetRandomFromList(_guestPersonal);
        }

        public string GetGuestBye()
        {
            return GetRandomFromList(_guestBye);
        }


        public string GetRandomGuestWelcome()
        {
            return GetRandomFromList(dialogueData?.GuestWelcome);
        }

        public string GetRandomGuestGoodbye()
        {
            return GetRandomFromList(dialogueData?.GuestGoodbye);
        }

        public string GetRandomGuestMain()
        {
            return GetRandomFromList(dialogueData?.GuestMain);
        }

        public void SetGuest(GuestSO guest)
        {
            _guest = guest;
        }

        #endregion

        // This method is used to pick a random textline from an array of strings.
        private string GetRandomFromList(string[] list)
        {
            if (list == null || list.Length == 0)
                return "???";

            int index = UnityEngine.Random.Range(0, list.Length);

            return list[index];
        }
    }
}
