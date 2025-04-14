/// <summary>
/// Custom Inspector for GameEvents that adds a runtime-only "Raise" button for testing.
/// </summary>

/// <remarks>
/// 11/04/2025 by Damian Dalinger: Script creation.
/// </remarks>

using UnityEditor;
using UnityEngine;

namespace ProjectCeros
{
    [CustomEditor(typeof(GameEvent), editorForChildClasses: true)]
    public class EventEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            GUI.enabled = Application.isPlaying;

            GameEvent e = target as GameEvent;
            if (GUILayout.Button("Raise"))
                e.Raise();
        }
    }
}
