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
        private DialogueData dialogueData;

        [SerializeField] private StringReference _podcastName;

        [SerializeField] private GuestSO _guest;

        private Dictionary<string, string> dialogueVariables = new Dictionary<string, string>();

        [SerializeField] private string[] _guestHello;

        [SerializeField] private string[] _guestPersonal;

        [SerializeField] private string[] _guestBye;

        [SerializeField] private Color injectedVariableColor; // Or any default



        void Awake()
        {
            LoadDialogueData();

            SetDialogueVariable("Podcastname", _podcastName);

        }



        public void SetDialogueVariable(string key, string value)
        {
            dialogueVariables[key] = value;
        }

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

        public string GetRandomWelcome()
        {
            return GetRandomFromList(dialogueData?.welcome);
        }

        public string GetRandomGoodbye()
        {
            return GetRandomFromList(dialogueData?.goodbye);

        }

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

        private string GetRandomFromList(string[] list)
        {
            if (list == null || list.Length == 0)
                return "???";

            int index = UnityEngine.Random.Range(0, list.Length);

            // Debug.Log(list[index]);

            return list[index];
        }
    }
}
