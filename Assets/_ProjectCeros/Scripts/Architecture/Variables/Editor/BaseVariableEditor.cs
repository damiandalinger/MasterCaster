/// <summary>
/// Custom editor for all BaseVariable<T> types.
/// Handles displaying developer description, runtime/initial values, and saveable flag.
/// Also auto-registers saveable variables in the SaveableVariableIndex.
/// </summary>

/// <remarks>
/// 04/06/2025 by Damian Dalinger: Script creation.
/// </remarks>

using UnityEditor;

namespace ProjectCeros
{
    [CustomEditor(typeof(BaseVariable<>), true)]
    public class BaseVariableEditor : UnityEditor.Editor
    {
        #region Fields

        private SerializedProperty _isSaveableProp;
        private SerializedProperty _runtimeValueProp;
        private SerializedProperty _initialValueProp;
        private SerializedProperty _devDescProp;
        private SaveableVariableIndex _cachedIndex;

        #endregion

        #region Lifecycle Methods

        // Finds required properties when the inspector is opened.
        private void OnEnable()
        {
            _isSaveableProp = serializedObject.FindProperty("_isSaveable");
            _runtimeValueProp = serializedObject.FindProperty("RuntimeValue");
            _initialValueProp = serializedObject.FindProperty("InitialValue");
            _devDescProp = serializedObject.FindProperty("DeveloperDescription");
        }

        // Draws the custom inspector GUI for the BaseVariable.
        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            if (_devDescProp != null)
                EditorGUILayout.PropertyField(_devDescProp);

            EditorGUILayout.PropertyField(_isSaveableProp);
            UpdateIndex((SaveableVariableBase)target, _isSaveableProp.boolValue);

            if (_isSaveableProp.boolValue)
                EditorGUILayout.PropertyField(_initialValueProp);

            EditorGUILayout.PropertyField(_runtimeValueProp);

            serializedObject.ApplyModifiedProperties();
        }

        #endregion

        #region Private Methods

        // Finds and caches the first SaveableVariableIndex asset in the project.
        private SaveableVariableIndex GetIndex()
        {
            if (_cachedIndex != null) return _cachedIndex;

            string[] guids = AssetDatabase.FindAssets("t:SaveableVariableIndex");
            if (guids.Length == 0) return null;

            string path = AssetDatabase.GUIDToAssetPath(guids[0]);
            _cachedIndex = AssetDatabase.LoadAssetAtPath<SaveableVariableIndex>(path);
            return _cachedIndex;
        }

        // Adds or removes the variable from the SaveableVariableIndex based on saveable state.
        private void UpdateIndex(SaveableVariableBase variable, bool isSaveable)
        {
            var index = GetIndex();
            if (index == null) return;

            if (isSaveable)
                index.AddIfSaveable(variable);
            else
                index.RemoveIfPresent(variable);
        }

        #endregion
    }
}
