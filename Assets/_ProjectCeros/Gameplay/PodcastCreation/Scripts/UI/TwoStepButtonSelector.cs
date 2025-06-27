using UnityEngine;
using System.Collections.Generic;

public class TwoStepButtonSelector : MonoBehaviour
{
    [Header("Sub Button Groups (1 group per main button)")]
    [Tooltip("Each group holds 3 sub-buttons for a main button.")]
    [SerializeField] private List<List<GameObject>> _subButtonGroups = new();

    public void ShowSubButtons(int mainButtonIndex)
    {
        ResetUI();

        if (mainButtonIndex < 0 || mainButtonIndex >= _subButtonGroups.Count)
        {
            Debug.LogWarning("Invalid button index");
            return;
        }

        foreach (var button in _subButtonGroups[mainButtonIndex])
            button.SetActive(true);
    }

    public void ResetUI()
    {
        foreach (var group in _subButtonGroups)
        {
            foreach (var button in group)
                button.SetActive(false);
        }
    }
}
