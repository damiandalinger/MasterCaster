/// <summary>
/// Imports comment data from JSON files into runtime sets at startup.
/// </summary>

/// <remarks>
/// 25/06/2025 by Damian Dalinger: Script Creation.
/// </remarks>

using System.Collections.Generic;
using UnityEngine;

namespace ProjectCeros
{

    public class CommentImporter : MonoBehaviour
    {
        #region Fields

        [Tooltip("Match the jsons with the right RuntimeSet.")]
        [SerializeField] private List<CommentImportSource> _importSources;

        #endregion

        #region Lifecycle Methods
        private void Start()
        {
            ImportComments();
            Destroy(gameObject);
        }

        #endregion

        #region Private Methods

        // Reads each configured JSON source and fills the corresponding runtime set.
        private void ImportComments()
        {
            foreach (var source in _importSources)
            {
                if (source.JsonFile == null || source.TargetSet == null)
                    continue;

                var parsedComments = JsonUtilityWrapper.FromJsonArray<CommentData>(source.JsonFile.text);

                source.TargetSet.Clear();
                foreach (var comment in parsedComments)
                {
                    source.TargetSet.Add(comment);
                }
            }
        }

        #endregion
    }
}
