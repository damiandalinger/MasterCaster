/// <summary>
/// Holds a list of saveable variables used by the SaveSystem.
/// </summary>

/// <remarks>
/// 04/06/2025 by Damian Dalinger: Script creation.
/// </remarks>

using UnityEngine;
using System.Collections.Generic;

namespace ProjectCeros
{
    [CreateAssetMenu(menuName = "SaveSystem/Saveable Variable Index")]
    public class SaveableVariableIndex : ScriptableObject
    {
        #region Fields

        [Tooltip("All registered saveable variables used by the SaveSystem.")]
        [SerializeField] private List<SaveableVariableBase> _saveableVariables = new();

        #endregion

        #region Properties

        public IReadOnlyList<SaveableVariableBase> Saveables => _saveableVariables;

        #endregion

#if UNITY_EDITOR

        #region Editor Methods

        // Adds the variable to the list if it's saveable and not already added.
        public void AddIfSaveable(SaveableVariableBase variable)
        {
            if (variable.IsSaveable && !_saveableVariables.Contains(variable))
            {
                _saveableVariables.Add(variable);
                UnityEditor.EditorUtility.SetDirty(this);
                UnityEditor.AssetDatabase.SaveAssets();
            }
        }

        // Removes the variable from the list if present.
        public void RemoveIfPresent(SaveableVariableBase variable)
        {
            if (_saveableVariables.Contains(variable))
            {
                _saveableVariables.Remove(variable);
                UnityEditor.EditorUtility.SetDirty(this);
                UnityEditor.AssetDatabase.SaveAssets();
            }
        }

        #endregion
#endif
    }
}

