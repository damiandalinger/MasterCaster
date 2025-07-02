using System;
using System.Collections.Generic;

namespace ProjectCeros
{

    [Serializable]
    public class DialogueData
    {
        public List<string> welcome;
        public List<string> goodbye;
        public List<TopicEntry> topics;  // Use list instead of dictionary
    }

    [Serializable]
    public class TopicEntry
    {
        public string topicName;        // The name of the topic (like "magic")
        public TopicMessages messages;  // The messages for this topic
    }

    [Serializable]
    public class TopicMessages
    {
        public List<string> positive;
        public List<string> negative;
    }
}
