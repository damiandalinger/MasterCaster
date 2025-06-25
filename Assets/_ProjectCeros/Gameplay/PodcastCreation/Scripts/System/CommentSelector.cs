/// <summary>
/// Selects context-specific comments based on game state (guests, sponsors, rating, etc.).
/// </summary>

/// <remarks>
/// 25/06/2025 by Damian Dalinger: Script Creation.
/// </remarks>

using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace ProjectCeros
{
    public class CommentSelector : MonoBehaviour
    {
        #region Serialized Fields

        [Header("Comment Pools")]

        [Tooltip("Pool of guest-related comments.")]
        [SerializeField] private CommentRuntimeSet _guestComments;

        [Tooltip("Pool of sponsor-related comments.")]
        [SerializeField] private CommentRuntimeSet _sponsorComments;

        [Tooltip("Pool of equipment-related comments.")]
        [SerializeField] private CommentRuntimeSet _equipmentComments;

        [Tooltip("Pool of spin-related comments.")]
        [SerializeField] private CommentRuntimeSet _spinComments;

        [Tooltip("Pool of general/random comments.")]
        [SerializeField] private CommentRuntimeSet _randomComments;

        [Header("Dynamic Input")]
        [Tooltip("The current in-game day.")]
        [SerializeField] private IntVariable _currentDay;

        [Tooltip("The selected spin ID for this session.")]
        [SerializeField] private IntVariable _selectedSpin;

        [Tooltip("Current star rating used for determining sentiment.")]
        [SerializeField] private FloatVariable _starRating;

        [Tooltip("List of currently active guest IDs.")]
        [SerializeField] private List<int> _guests;

        [Tooltip("List of currently active sponsor IDs.")]
        [SerializeField] private List<int> _sponsors;

        [Tooltip("List of currently active equipment IDs.")]
        [SerializeField] private List<int> _equipment;

        [Header("Output")]
        [Tooltip("The runtime set where selected comment strings will be stored.")]
        [SerializeField] private StringRuntimeSet _selectedComments;

        #endregion

        #region Public Methods

        // Generates a set of comments based on the current game state and writes them to the output set.
        public void GenerateComments()
        {
            int totalCount = GetCommentCountForDay(_currentDay.RuntimeValue);

            List<CommentData> combinedPool = new();
            combinedPool.AddRange(GetCommentsByIDs(_guests));
            combinedPool.AddRange(GetCommentsByIDs(_sponsors));
            combinedPool.AddRange(GetCommentsByIDs(_equipment));
            combinedPool.AddRange(GetSpinComments(_selectedSpin.RuntimeValue));
            combinedPool.AddRange(GetRandomCommentsByRating(_starRating.RuntimeValue));

            var selected = SelectRandomComments(combinedPool, totalCount);
            ApplyToOutput(selected);
        }

        #endregion

        #region Private Methods

        // Clears the output set and adds the selected comments.
        private void ApplyToOutput(List<string> comments)
        {
            _selectedComments.Clear();
            foreach (var comment in comments)
            {
                _selectedComments.Add(comment);
            }
        }

        // Returns the number of comments to generate based on the current day.
        private int GetCommentCountForDay(int day)
        {
            return day switch
            {
                <= 1 => Random.Range(1, 3),
                2 => Random.Range(2, 4),
                _ => Random.Range(3, 5)
            };
        }

        // Retrieves comments from the equipment pool that match any of the given IDs.
        private List<CommentData> GetCommentsByIDs(List<int> ids)
        {
            return _equipmentComments.Items
                .Where(comment => ids.Contains(comment.ID))
                .ToList();
        }

        // Retrieves all comments related to the currently selected spin.
        private List<CommentData> GetSpinComments(int spinID)
        {
            return _spinComments.Items
                .Where(comment => comment.ID == spinID)
                .ToList();
        }

        // Selects random comments based on the current star rating.
        // Includes always-neutral (ID 3) and context-specific sentiment (ID 1 or 2).
        private List<CommentData> GetRandomCommentsByRating(float rating)
        {
            return _randomComments.Items.Where(c =>
                c.ID == 3 ||
                (rating >= 4f && c.ID == 1) ||
                (rating <= 1.9f && c.ID == 2) ||
                (rating > 1.9f && rating < 4f && c.ID == 1)
            ).ToList();
        }

        // Randomly selects a number of unique comments from the given pool.
        private List<string> SelectRandomComments(List<CommentData> pool, int count)
        {
            return pool
                .OrderBy(_ => Random.value)
                .Take(count)
                .Select(c => c.Comment)
                .ToList();
        }
        
        #endregion
    }
}
