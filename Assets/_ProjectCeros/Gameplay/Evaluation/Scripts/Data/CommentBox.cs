/// <summary>
/// Handles a single UI box for displaying a user comment.
/// </summary>

/// <remarks>
/// 25/06/2025 by Damian Dalinger: Script creation.
/// </remarks>

using UnityEngine;
using TMPro;
using UnityEngine.UI;

[System.Serializable]
public class CommentBox
{
    #region Serialized Fields

    [Tooltip("UI Text element for the displayed name.")]
    public TMP_Text NameText;

    [Tooltip("UI Text element for the displayed comment.")]
    public TMP_Text CommentText;

    [Tooltip("UI Image for the profile icon.")]
    public Image ProfileImage;

    #endregion

    #region Public Methods

    // Sets the visual content of the comment box.
    public void SetContent(string name, string comment, Sprite icon)
    {
        NameText.text = name;
        CommentText.text = comment;
        ProfileImage.sprite = icon;
        ProfileImage.enabled = true;
    }

    // Clears the content of the comment box.
    public void Clear()
    {
        NameText.text = "";
        CommentText.text = "";
        ProfileImage.sprite = null;
        ProfileImage.enabled = false;
    }

    #endregion
}