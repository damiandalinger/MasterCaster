/// <summary>
/// Custom editor for RuntimeSet<T>. Handles display of tracked runtime items and saveability logic.
/// Dynamically determines item type via reflection and disables save option if type is not serializable.
/// </summary>

/// <remarks>
/// 04/06/2025 by Damian Dalinger: Script creation.
/// </remarks>

using UnityEditor;
using System;
using System.Reflection;
using System.Collections;

namespace ProjectCeros
{
    [CustomEditor(typeof(RuntimeSet<>), true)]
    public class RuntimeSetEditor : UnityEditor.Editor
    {
        #region Fields

        private SerializedProperty _isSaveableProp;
        private SerializedProperty _itemsProp;
        private FieldInfo _itemsField;
        private Type _itemType;
        private bool _isSerializable;
        private SaveableVariableIndex _cachedIndex;

        #endregion

        #region Lifecycle Methods

        // Called when the inspector is opened. Caches references and type info.
        private void OnEnable()
        {
            _isSaveableProp = serializedObject.FindProperty("_isSaveable");
            _itemsProp = serializedObject.FindProperty("Items");

            var targetType = target.GetType();

            while (targetType != null)
            {
                if (targetType.IsGenericType && targetType.GetGenericTypeDefinition() == typeof(RuntimeSet<>))
                {
                    _itemType = targetType.GetGenericArguments()[0];
                    _itemsField = targetType.GetField("Items", BindingFlags.Public | BindingFlags.Instance);
                    break;
                }
                targetType = targetType.BaseType;
            }

            _isSerializable = IsTrulySerializable(_itemType);
        }

        // Draws the custom inspector GUI for the RuntimeSet.
        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            if (_isSerializable)
            {
                EditorGUILayout.PropertyField(_isSaveableProp);
                UpdateIndex((SaveableVariableBase)target, _isSaveableProp.boolValue);

                if (_itemsProp != null)
                    EditorGUILayout.PropertyField(_itemsProp, true);
            }
            else
            {
                _isSaveableProp.boolValue = false;

                ShowItemsManually();
                EditorGUILayout.HelpBox(
                    $"The type '{_itemType?.Name}' is not serialisable. Saving is disabled.",
                    MessageType.Warning
                );
            }

            serializedObject.ApplyModifiedProperties();
        }

        #endregion

        #region Private Methods

        // Determines if a type is serializable by Unity and .NET standards.
        private bool IsTrulySerializable(Type type)
        {
            return type != null && type.IsSerializable && !typeof(UnityEngine.Object).IsAssignableFrom(type);
        }

        // Finds and caches the SaveableVariableIndex asset in the project.
        private SaveableVariableIndex GetIndex()
        {
            if (_cachedIndex != null) return _cachedIndex;

            string[] guids = AssetDatabase.FindAssets("t:SaveableVariableIndex");
            if (guids.Length == 0) return null;

            string path = AssetDatabase.GUIDToAssetPath(guids[0]);
            _cachedIndex = AssetDatabase.LoadAssetAtPath<SaveableVariableIndex>(path);
            return _cachedIndex;
        }

        // Displays the items manually as read-only fields when serialization isn't supported.
        private void ShowItemsManually()
        {
            if (_itemsField == null) return;

            var list = _itemsField.GetValue(target) as IList;
            if (list == null) return;

            EditorGUILayout.Space();
            EditorGUILayout.LabelField($"Items ({list.Count}):", EditorStyles.boldLabel);

            EditorGUI.BeginDisabledGroup(true);
            foreach (var item in list)
            {
                EditorGUILayout.ObjectField(item as UnityEngine.Object, typeof(UnityEngine.Object), true);
            }
            EditorGUI.EndDisabledGroup();
        }

        // Adds or removes the variable from the index based on saveable status.
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