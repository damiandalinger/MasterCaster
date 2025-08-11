/// <summary>
/// A single TutorialStep, stores what text to display and the id.
/// </summary>

/// <remarks>
/// 11/08/2025 by Damian Dalinger: Script creation.
/// </remarks>

using UnityEngine;

namespace ProjectCeros
{
    [CreateAssetMenu(menuName = "Other/Tutorial Step")]
    public class TutorialStep : ScriptableObject
    {
        [Tooltip("Unique non-negative integer ID for this step. Must be unique within the tutorial sequence.")]
        public int Id;

        [TextArea(1, 1), Tooltip("Short headline/title displayed at the top of the panel.")]
        public string Headline;

        [TextArea(4, 12), Tooltip("Long-form description/instructions shown in the panel body.")]
        public string Text;

        [Tooltip("Runtime flag indicating whether the step was completed. Will be reset by the TutorialManager at scene start.")]
        public bool IsComplete = false;
    }
}