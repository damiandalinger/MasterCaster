/// <summary>
/// Runtime database that holds all available articles during gameplay.
/// </summary>

/// <remarks>
/// 29/04/2025 by Damian Dalinger: Initial creation.
/// </remarks>

using UnityEngine;

namespace ProjectCeros
{
    [CreateAssetMenu(menuName = "Runtime Sets/ArticleDatabase")]
    public class ArticleDatabase : RuntimeSet<Article>
    {
        // Currently inherits all functionality from RuntimeSet.
        // Future methods like filtering or randomization by category can be added here.
    }
}