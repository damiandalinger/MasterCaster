/// <summary>
/// Syncs leaderboard data (e.g., rank, listeners) between Podcaster objects and persistent runtime sets.
/// Called after leaderboard updates and during load cycles.
/// </summary>

/// <remarks>
/// 08/07/2025 by Damian Dalinger: Script Creation.
/// </remarks>

using UnityEngine;
using System.Collections.Generic;

namespace ProjectCeros
{
    public class LeaderboardDataSync : MonoBehaviour
    {
        #region Fields

        [Tooltip("References to the podcasters.")]
        [SerializeField] private List<Podcaster> _podcasters;

        [Header("Runtime Sets")]
        [Tooltip("The current rank of all podcasters.")]
        [SerializeField] private IntRuntimeSet _currentRank;

        [Tooltip("The previous rank of all podcasters.")]
        [SerializeField] private IntRuntimeSet _previousRank;

        [Tooltip("The current listeners of all podcasters.")]
        [SerializeField] private IntRuntimeSet _listenerCount;

        [Tooltip("The sprite for the rank change.")]
        [SerializeField] private IntRuntimeSet _rankChange;

        #endregion

        #region Public Methods

        // Exports the current values of all podcasters into saveable RuntimeSets.
        public void ExportToRuntimeSets()
        {
            _currentRank.Clear();
            _previousRank.Clear();
            _listenerCount.Clear();
            _rankChange.Clear();


            for (int i = 0; i < _podcasters.Count; i++)
            {
                _currentRank.AddWithDuplicates(_podcasters[i].CurrentRank);
                _previousRank.AddWithDuplicates(_podcasters[i].PreviousRank);
                _listenerCount.AddWithDuplicates(_podcasters[i].CurrentListener);
                _rankChange.AddWithDuplicates(_podcasters[i].RankChange);
            }
        }

        // Imports the values from the RuntimeSets into the podcaster scriptable objects.
        public void ImportFromRuntimeSets()
        {

            for (int i = 0; i < _podcasters.Count; i++)
            {
                _podcasters[i].CurrentRank = _currentRank.Items[i];
                _podcasters[i].PreviousRank = _previousRank.Items[i];
                _podcasters[i].CurrentListener = _listenerCount.Items[i];
                _podcasters[i].RankChange = _rankChange.Items[i];
            }
        }

        #endregion
    }
}