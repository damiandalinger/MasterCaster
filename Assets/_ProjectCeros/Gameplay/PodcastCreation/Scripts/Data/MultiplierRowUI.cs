/// <summary>
/// Represents a single UI row displaying a named multiplier value with optional icon.
/// </summary>

/// <remarks>
/// 23/06/2025 by Damian Dalinger: Script creation.
/// </remarks>

using TMPro;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class MultiplierRowUI
{
    #region Fields

    [Tooltip("Root GameObject for the row (controls overall visibility).")]
    public GameObject RootObject;

    [Tooltip("Text field displaying the name or label of the multiplier.")]
    public TMP_Text TextName;

    [Tooltip("Text field displaying the value (e.g., +1.50).")]
    public TMP_Text TextValue;

    [Tooltip("Icon representing the multiplier type.")]
    public Image Icon;

    #endregion
}
