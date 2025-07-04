using System;
using System.Collections.Generic;

namespace ProjectCeros
{
[System.Serializable]
public class DialogueData
{
    public string[] welcome;
    public string[] goodbye;
    public Topic[] topics;

    public string[] GuestWelcome;
    public string[] GuestMain;
    public string[] GuestGoodbye;
    public GuestAnswer[] GuestAnswers;
}

[System.Serializable]
public class Topic
{
    public string topicName;
    public TopicMessages messages;
}

[System.Serializable]
public class TopicMessages
{
    public string[] positive;
    public string[] negative;
}

[System.Serializable]
public class GuestAnswer
{
    public int GuestID;
    public string[] hello;
    public string[] personal;
    public string[] bye;
}
}
