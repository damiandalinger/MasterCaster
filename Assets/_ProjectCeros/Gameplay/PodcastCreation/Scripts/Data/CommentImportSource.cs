/// <summary>
/// Defines a source for importing comment data from a JSON file into a runtime set.
/// </summary>

/// <remarks>
/// 25/06/2025 by Damian Dalinger: Script creation.
/// </remarks>

using UnityEngine;

namespace ProjectCeros
{

    [System.Serializable]
    public class CommentImportSource
    {
        [Tooltip("The JSON file containing the comment data.")]
        public TextAsset JsonFile;

        [Tooltip("The target runtime set where parsed comments will be stored.")]
        public CommentRuntimeSet TargetSet;
    }
}