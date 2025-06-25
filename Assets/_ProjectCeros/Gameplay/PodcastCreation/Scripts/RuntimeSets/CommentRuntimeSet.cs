/// <summary>
/// Runtime database that holds all available comments during gameplay.
/// </summary>

/// <remarks>
/// 25/06/2025 by Damian Dalinger: Script creation.
/// </remarks>

using UnityEngine;

namespace ProjectCeros
{
    [CreateAssetMenu(menuName = "Runtime Sets/CommentRuntimeSet")]
    public class CommentRuntimeSet : RuntimeSet<CommentData>
    {

    }
}