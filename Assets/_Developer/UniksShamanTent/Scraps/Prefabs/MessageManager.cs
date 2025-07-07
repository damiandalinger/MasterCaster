using System.IO;
using UnityEngine;


namespace ProjectCeros
{
    public class MessageManager : MonoBehaviour
    {
        private MessageData messageData;

        void Start()
        {
            LoadMessages();
            ShowRandomMessage();
        }

        void LoadMessages()
        {
            string path = Path.Combine(Application.streamingAssetsPath, "DialogueText.json");


            if (File.Exists(path))
            {
                string json = File.ReadAllText(path);
                messageData = JsonUtility.FromJson<MessageData>(json);
            }
            else
            {
                Debug.LogError("Could not find messages.json at: " + path);
            }
        }

        void ShowRandomMessage()
        {
            if (messageData != null && messageData.messages.Count > 0)
            {
                int index = Random.Range(0, messageData.messages.Count);
                Debug.Log("Random Message: " + messageData.messages[index]);
            }
        }
    }
}