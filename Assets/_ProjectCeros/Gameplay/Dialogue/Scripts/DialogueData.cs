/// <summary>
/// A class system to load json data into.
/// </summary>

/// <remarks>
/// 04/07/2025 by Unik Kelmendi: Initial creation.
/// </remarks>


using System;


namespace ProjectCeros
{
[Serializable]
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

[Serializable]
public class Topic
{
    public string topicName;
    public TopicMessages messages;
}

[Serializable]
public class TopicMessages
{
    public string[] positive;
    public string[] negative;
}

[Serializable]
public class GuestAnswer
{
    public int GuestID;
    public string[] hello;
    public string[] personal;
    public string[] bye;
}
}
