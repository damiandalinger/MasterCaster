/// <summary>
/// Custom inspector for FMODParameterSetter.
/// Displays either label or float value depending on toggle.
/// </summary>

/// <remarks>
/// 10/07/2025 by Damian Dalinger: Initial implementation.
/// </remarks>

using UnityEditor;

namespace ProjectCeros
{
    [CustomEditor(typeof(FMODParameterSetter))]
    public class FMODParameterSetterEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            SerializedProperty soundEvent = serializedObject.FindProperty("_soundEvent");
            SerializedProperty target = serializedObject.FindProperty("_target");
            SerializedProperty parameterName = serializedObject.FindProperty("_parameterName");
            SerializedProperty useLabel = serializedObject.FindProperty("_useLabel");
            SerializedProperty parameterLabel = serializedObject.FindProperty("_parameterLabel");
            SerializedProperty parameterValue = serializedObject.FindProperty("_parameterValue");

            EditorGUILayout.PropertyField(soundEvent);
            EditorGUILayout.PropertyField(target);
            EditorGUILayout.PropertyField(parameterName);
            EditorGUILayout.PropertyField(useLabel);

            if (useLabel.boolValue)
                EditorGUILayout.PropertyField(parameterLabel);
            else
                EditorGUILayout.PropertyField(parameterValue);

            serializedObject.ApplyModifiedProperties();
        }
    }
}
