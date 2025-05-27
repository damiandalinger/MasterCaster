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
        [System.Serializable]
        public class SaveGroup
        {
            public string GroupName;
            public List<ScriptableObject> Saveables;
        }

        [SerializeField] private List<SaveGroup> saveGroups;

        public static SaveManager Instance { get; private set; }

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
        }

        [ContextMenu("Save Now")]
        public void Save()
        {
            var data = new Dictionary<string, object>();

            foreach (var group in saveGroups)
            {
                foreach (var so in group.Saveables)
                {
                    if (so is ISaveable saveable)
                        data[saveable.SaveKey] = saveable.CaptureState();
                }
            }

            var json = JsonConvert.SerializeObject(data, Formatting.Indented);
            File.WriteAllText(GetSavePath(), json);
            Debug.Log("Game saved to: " + GetSavePath());
        }

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

            foreach (var group in saveGroups)
            {
                foreach (var so in group.Saveables)
                {
                    if (so is ISaveable saveable && data.TryGetValue(saveable.SaveKey, out var state))
                        saveable.RestoreState(state);
                }
            }

            Debug.Log("Game loaded from: " + GetSavePath());
        }

        [ContextMenu("Open Save Folder")]
        public void OpenSaveFolder()
        {
#if UNITY_EDITOR
            EditorUtility.RevealInFinder(Application.persistentDataPath);
#endif
        }

        private string GetSavePath() =>
            Path.Combine(Application.persistentDataPath, "save.json");


        public void DeleteSave()
        {
            var path = GetSavePath();
            if (File.Exists(path))
            {
                File.Delete(path);
                Debug.Log("🗑️ Alte Save-Datei gelöscht.");
            }
        }

        public bool SaveFileExists()
        {
            return File.Exists(GetSavePath());
        }
    }
}
