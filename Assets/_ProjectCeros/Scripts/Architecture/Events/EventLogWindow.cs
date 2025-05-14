/// <summary>
/// Custom Unity Editor window for displaying a live log of GameEvent invocations,
/// including which listeners were active at the time of the event.
/// Helps debug and visualize event-driven communication.
/// </summary>

/// <remarks>
/// 11/04/2025 by Damian Dalinger: Script creation.
/// </remarks>

#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System;

namespace ProjectCeros
{
    public class EventLogWindow : EditorWindow
    {
        private Vector2 _scrollPosition;
        private static List<LogEntry> _eventLogs = new List<LogEntry>();

        [MenuItem("Window/Event Log")]
        public static void ShowWindow()
        {
            GetWindow<EventLogWindow>("Event Log");
        }

        public static void LogEvent(string eventName, GameEvent gameEvent)
        {
            List<GameObject> currentListeners = new List<GameObject>();

            // Capture all current listeners at the moment of event raise.
            if (gameEvent != null)
            {
                foreach (var listener in gameEvent.eventListeners)
                {
                    if (listener != null)
                        currentListeners.Add(listener.gameObject);
                }
            }

            // Add entry to the log.
            _eventLogs.Add(new LogEntry
            {
                time = DateTime.Now,
                eventName = eventName,
                context = gameEvent,
                listeners = currentListeners
            });

            // Manually trigger a repaint to update the log in real-time.
            if (EditorWindow.HasOpenInstances<EventLogWindow>())
            {
                EditorWindow.GetWindow<EventLogWindow>().Repaint();
            }
        }

        private void OnGUI()
        {
            GUILayout.Label("Event Log", EditorStyles.boldLabel);
            if (GUILayout.Button("Clear Log"))
                _eventLogs.Clear();

            _scrollPosition = EditorGUILayout.BeginScrollView(_scrollPosition);
            foreach (var log in _eventLogs)
            {
                EditorGUILayout.BeginVertical("box");
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField(log.time.ToLongTimeString(), GUILayout.Width(80));
                EditorGUILayout.LabelField(log.eventName, GUILayout.ExpandWidth(true));
                if (log.context != null)
                {
                    if (GUILayout.Button("Select", GUILayout.Width(60)))
                        Selection.activeObject = log.context;
                }
                EditorGUILayout.EndHorizontal();
                if (log.listeners != null && log.listeners.Count > 0)
                {
                    foreach (var listener in log.listeners)
                    {
                        EditorGUILayout.BeginHorizontal();
                        EditorGUILayout.LabelField("Listener: " + (listener != null ? listener.name : "<Destroyed>"));
                        if (listener != null && GUILayout.Button("Select", GUILayout.Width(60)))
                        {
                            Selection.activeGameObject = listener;
                        }
                        EditorGUILayout.EndHorizontal();
                    }
                }
                EditorGUILayout.EndVertical();
                GUILayout.Space(10);
            }
            EditorGUILayout.EndScrollView();
        }

        private class LogEntry
        {
            public DateTime time;
            public string eventName;
            public UnityEngine.Object context;
            public List<GameObject> listeners;
        }
    }
}
#endif