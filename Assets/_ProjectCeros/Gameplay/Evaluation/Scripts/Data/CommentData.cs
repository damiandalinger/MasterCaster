/// <summary>
/// Represents a comment entry with an ID and text content.
/// </summary>

/// <remarks>
/// 25/06/2025 by Damian Dalinger: Script creation.
/// </remarks>

using UnityEngine;

[System.Serializable]
public class CommentData
{
    [Tooltip("Unique identifier for the comment.")]
    public int ID;

    [Tooltip("The actual comment text.")]
    public string Comment;
}