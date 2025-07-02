using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace ProjectCeros
{

    public class DialogueManager : MonoBehaviour
    {
        public string fileName = "DialogueText.json";
        private DialogueData dialogueData;

        [SerializeField] private StringReference _podcastName;

        [SerializeField] private StringReference _guest;

        private Dictionary<string, string> dialogueVariables = new Dictionary<string, string>();

        void Awake()
        {
            LoadDialogueData();

            SetDialogueVariable("Podcastname", _podcastName);
            SetDialogueVariable("PlayerName", "Unik");
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
                return "";
            }

            TopicEntry entry = dialogueData.topics.Find(t => t.topicName == topic);
            if (entry == null)
            {
                Debug.LogWarning($"Topic '{topic}' not found");
                return "";
            }

            List<string> pool = positive ? entry.messages.positive : entry.messages.negative;

            if (pool == null || pool.Count == 0)
            {
                Debug.LogWarning($"No messages found for topic '{topic}' with sentiment {(positive ? "positive" : "negative")}");
                return "";
            }

            string chosenMessage = GetRandomFromList(pool);

            Debug.Log($"Selected topic message ({topic} - {(positive ? "positive" : "negative")}): {chosenMessage}");

            return chosenMessage;
        }

        private string GetRandomFromList(List<string> list)
        {
            if (list == null || list.Count == 0)
                return "";

            int index = Random.Range(0, list.Count);



            return list[index];
        }
    }
}
