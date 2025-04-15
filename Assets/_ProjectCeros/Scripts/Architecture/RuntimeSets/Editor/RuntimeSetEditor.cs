/// <summary>
/// Custom Editor for ScriptableObjects that are derived from RuntimeSet<T>.
/// It provides a user-friendly display of the items contained in a RuntimeSet in the Inspector.
/// </summary>

/// <remarks>
/// 14/04/2025 by Damian Dalinger: Script creation.
/// </remarks>

using System.Collections;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace ProjectCeros
{
    [CustomEditor(typeof(ScriptableObject), true)]
    public class RuntimeSetEditor : UnityEditor.Editor
    {
        private FieldInfo _itemsField;
        private IList _itemsList;
        private string _label;

        // Override of the OnInspectorGUI method to display a custom inspector for RuntimeSets.
        // It shows a read-only list of items contained in the RuntimeSet.
        public override void OnInspectorGUI()
        {
            var type = target.GetType();
            if (!IsRuntimeSet(type))
            {
                base.OnInspectorGUI();
                return;
            }

            // Try to get the "Items" field (it may be inherited from a base class).
            if (_itemsField == null)
            {
                _itemsField = type.GetField("Items", BindingFlags.Public | BindingFlags.Instance);
            }

            _itemsList = _itemsField.GetValue(target) as IList;

            _label = ObjectNames.NicifyVariableName(type.Name);
            EditorGUILayout.LabelField(_label + " (Runtime Set)", EditorStyles.boldLabel);

            EditorGUILayout.Space();

            EditorGUILayout.LabelField($"Items ({_itemsList.Count}):");

            EditorGUI.BeginDisabledGroup(true); 
            foreach (var item in _itemsList)
            {
                EditorGUILayout.ObjectField(item as Object, typeof(Object), true);
            }
            EditorGUI.EndDisabledGroup();

            EditorUtility.SetDirty(target); 
        }

        private bool IsRuntimeSet(System.Type type)
        {
            while (type != null)
            {
                if (type.IsGenericType && type.GetGenericTypeDefinition().Name.StartsWith("RuntimeSet"))
                    return true;

                type = type.BaseType;
            }

            return false;
        }
    }
}
