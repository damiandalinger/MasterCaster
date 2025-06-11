/// <summary>
/// Manages saving and loading of ScriptableObjects implementing ISaveable.
/// Uses JSON file stored in persistent data path.
/// </summary>

/// <remarks>
/// 30/05/2025 by Damian Dalinger: Script creation.
/// 04/06/2025 by Damian Dalinger: Changed the system from a hardcoded list to a automatic adding list.
/// </remarks>

using UnityEngine;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace ProjectCeros
{
    public class SaveManager : MonoBehaviour
    {
        #region Fields

        [Tooltip("The Index who keeps track of all saveable Variables.")]
        [SerializeField] private SaveableVariableIndex _variableIndex;

        #endregion

        #region Properties

        // Global instance of the SaveManager.
        public static SaveManager Instance { get; private set; }

        #endregion

        #region Lifecycle Methods

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
        }

        #endregion

        #region Public Methods

        // Saves all saveable variables in the index to disk as a JSON file.
        [ContextMenu("Save Now")]
        public void Save()
        {
            if (_variableIndex == null) return;

            var data = new Dictionary<string, object>();
            foreach (var saveable in _variableIndex.Saveables)
            {
                if (saveable.IsSaveable)
                    data[saveable.SaveKey] = saveable.CaptureState();
            }

            var json = JsonConvert.SerializeObject(data, Formatting.Indented);
            File.WriteAllText(GetSavePath(), json);
        }

        // Loads saved data from disk and applies it to registered saveables.
        [ContextMenu("Load Now")]
        public void Load()
        {
            if (!File.Exists(GetSavePath()) || _variableIndex == null) return;

            var json = File.ReadAllText(GetSavePath());
            var data = JsonConvert.DeserializeObject<Dictionary<string, object>>(json);

            foreach (var saveable in _variableIndex.Saveables)
            {
                if (saveable.IsSaveable && data.TryGetValue(saveable.SaveKey, out var state))
                    saveable.RestoreState(state);
            }
        }

        // Opens the folder containing the save file in the OS file browser (Editor only).
        [ContextMenu("Open Save Folder")]
        public void OpenSaveFolder()
        {
#if UNITY_EDITOR
            EditorUtility.RevealInFinder(Application.persistentDataPath);
#endif
        }

        // Deletes the existing save file from disk.
        public void DeleteSave()
        {
            var path = GetSavePath();
            if (File.Exists(path))
            {
                File.Delete(path);
            }
        }

        // Checks if a save file exists.
        public bool SaveFileExists()
        {
            return File.Exists(GetSavePath());
        }

        // Resets all saveable variables to their default values.
        public void ResetSaveables()
        {
            foreach (var saveable in _variableIndex.Saveables)
            {
                saveable.ResetToDefault();
            }
        }

        #endregion

        #region Private Methods

        // Returns the full file path to the save file on disk.
        private string GetSavePath() => Path.Combine(Application.persistentDataPath, "save.json");

        #endregion
    }
}
