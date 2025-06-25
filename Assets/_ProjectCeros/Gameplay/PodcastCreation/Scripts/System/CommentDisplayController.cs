/// <summary>
/// Handles the display of comments in the UI using CommentBoxes.
/// </summary>

/// <remarks>
/// 25/06/2025 by Damian Dalinger: Script Creation.
/// </remarks>

using UnityEngine;
using System.Collections.Generic;

namespace ProjectCeros
{
    public class CommentDisplayController : MonoBehaviour
    {
        #region Fields

        [Tooltip("The comments consisting of two text and one image object.")]
        [SerializeField] private List<CommentBox> _commentBoxes;

        [Header("Data Sources")]
        [Tooltip("The StringRuntimeSet with the selected comments.")]
        [SerializeField] private StringRuntimeSet _selectedComments;

        [Tooltip("The RuntimeSet with the names of the commentators.")]
        [SerializeField] private CommentRuntimeSet _commentNames;

        [Tooltip("The sprites of the profile icons.")]
        [SerializeField] private Sprite[] _profileIcons;

        private bool _wasEnabled = false;

        #endregion

        #region Lifecycle Methods

        private void OnEnable()
        {
            if (!_wasEnabled)
            {
                var selector = GetComponent<CommentSelector>();
                selector.GenerateComments();
                ShowComments();
                _wasEnabled = true;
            }
        }

        #endregion

        #region Public Methods

        // Displays selected comments with randomized user names and icons.
        public void ShowComments()
        {
            var nameCandidates = new List<CommentData>(_commentNames.Items);
            var comments = _selectedComments.Items;

            for (int i = 0; i < _commentBoxes.Count; i++)
            {
                if (i < comments.Count)
                {
                    string comment = comments[i];
                    string name = GetRandomName(ref nameCandidates);
                    Sprite icon = _profileIcons.GetRandom();

                    _commentBoxes[i].SetContent(name, comment, icon);
                }
                else
                {
                    _commentBoxes[i].Clear();
                }
            }
        }

        #endregion

        // Retrieves a unique random name from the name candidate list.
        private string GetRandomName(ref List<CommentData> candidates)
        {
            if (candidates == null || candidates.Count == 0)
                return "???";

            int index = Random.Range(0, candidates.Count);
            string result = candidates[index].Comment;
            candidates.RemoveAt(index);
            return result;
        }

        #region Private Methods

        #endregion
    }
}