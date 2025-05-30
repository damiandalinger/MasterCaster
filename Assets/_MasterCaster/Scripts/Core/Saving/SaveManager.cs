/// <summary>
/// Manages saving and loading of ScriptableObjects implementing ISaveable.
/// Uses JSON file stored in persistent data path.
/// </summary>
/// <remarks>
/// 30/05/2025 by Damian Dalinger: Script creation.
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

        [Tooltip("Groups of ScriptableObjects to be saved and loaded.")]
        [SerializeField] private List<SaveGroup> _saveGroups = new();

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

        // Saves all ISaveable objects in configured save groups to disk.
        [ContextMenu("Save Now")]
        public void Save()
        {
            var data = new Dictionary<string, object>();

            foreach (var group in _saveGroups)
            {
                foreach (var so in group.Saveables)
                {
                    if (so is ISaveable saveable)
                        data[saveable.SaveKey] = saveable.CaptureState();
                }
            }

            var json = JsonConvert.SerializeObject(data, Formatting.Indented);
            File.WriteAllText(GetSavePath(), json);
        }

        // Loads and restores state for all ISaveable objects from disk.
        [ContextMenu("Load Now")]
        public void Load()
        {
            if (!File.Exists(GetSavePath()))
            {
                Debug.LogWarning("Save file not found.");
                return;
            }

            var json = File.ReadAllText(GetSavePath());
            var data = JsonConvert.DeserializeObject<Dictionary<string, object>>(json);

            foreach (var group in _saveGroups)
            {
                foreach (var so in group.Saveables)
                {
                    if (so is ISaveable saveable && data.TryGetValue(saveable.SaveKey, out var state))
                        saveable.RestoreState(state);
                }
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

        private string GetSavePath() =>
            Path.Combine(Application.persistentDataPath, "save.json");

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

        #endregion

        #region Nested Types

        [System.Serializable]
        public class SaveGroup
        {
            public string GroupName;

            [Tooltip("List of ScriptableObjects to include in this save group.")]
            public List<ScriptableObject> Saveables;
        }

        #endregion
    }
}
