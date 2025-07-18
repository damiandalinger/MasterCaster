/// <summary>
/// Manages and updates the leaderboard of rival podcasters and the player.
/// </summary>

/// <remarks>
/// 02/07/2025 by Damian Dalinger: Script Creation.
/// 18/07/2025 by Unik Kelmendi: added win condition.
/// </remarks>

using UnityEngine;
using System.Collections.Generic;
using System.Linq;

namespace ProjectCeros
{
    public class LeaderboardManager : MonoBehaviour
    {
        #region Fields

        [Header("Podcaster")]
        [Tooltip("List of rival podcasters in the leaderboard.")]
        [SerializeField] private List<Podcaster> _rivalPodcasters;

        [Tooltip("The player as a podcaster ScriptableObject.")]
        [SerializeField] private Podcaster _playerPodcaster;

        [Tooltip("Database of currently active articles.")]
        [SerializeField] private ArticleDatabase _selectedArticles;

        [Header("Player Settings")]
        [Tooltip("Reference to the current listener count of the player.")]
        [SerializeField] private IntReference _playerListeners;

        [Tooltip("Starting rank for the player rank.")]
        [SerializeField] private IntReference _startRank;

        [Tooltip("Exponent used to curve the climb progress.")]
        [SerializeField] private FloatReference _climbExponent;

        [Tooltip("The name of the podcast.")]
        [SerializeField] private StringReference _podcastName;

        [Tooltip("The name of the person doing the podcast.")]
        [SerializeField] private StringReference _personName;

        [Header("Rival Settings")]
        [Tooltip("Multiplier if the subgenre is liked and in the newspaper.")]
        [SerializeField] private FloatReference _increaseMultiplier;

        [Tooltip("Multiplier if the subgenre is disliked and in the newspaper.")]
        [SerializeField] private FloatReference _decreaseMultiplier;

        [Tooltip("Reference to the LeaderboardDataSync script.")]
        [SerializeField] private LeaderboardDataSync _dataSync;

        [Tooltip("If true, player has completed the game.")]
        [SerializeField] private BoolReference _hasWon;

        #endregion

        #region Properties

        // Current rank of the player.
        public int PlayerRank { get; private set; }

        #endregion

        #region Public Methods

        // Initializes runtime values for all podcasters, including the player.
        public void InitializeAllPodcasters()
        {
            foreach (var rival in _rivalPodcasters)
            {
                rival.InitializeRuntime();
            }

            _playerPodcaster.InitializeRuntime();

            PlayerRank = -1;

            UpdateLeaderboard();
        }

        // Updates leaderboard ranks and listener counts based on article effects.
        public void UpdateLeaderboard()
        {
            var articles = _selectedArticles.Items;
            var allPodcasters = CollectAllPodcasters();

            SavePreviousRanks(allPodcasters);
            UpdateRivalListeners(articles);
            UpdatePlayer();

            var sortedPodcasters = SortPodcasters(allPodcasters);
            UpdateRanks(sortedPodcasters);
            _dataSync.ExportToRuntimeSets();
        }

        #endregion

        #region Private Methods

        // Calculates the rank change direction between previous and current rank.
        private int CalculateRankChange(int previous, int current)
        {
            if (previous <= 0) return 2;
            if (previous > current) return 1;
            if (previous < current) return 3;
            return 2;
        }

        // Returns a combined list of rival podcasters and the player.
        private List<Podcaster> CollectAllPodcasters()
        {
            var all = new List<Podcaster>(_rivalPodcasters);
            if (_playerPodcaster != null)
                all.Add(_playerPodcaster);
            return all;
        }

        // Stores the current rank into the previous rank slot for each podcaster.
        private void SavePreviousRanks(List<Podcaster> podcasters)
        {
            foreach (var rival in podcasters)
            {
                if (rival.CurrentRank > 0)
                    rival.PreviousRank = rival.CurrentRank;
            }
        }

        // Assigns the player a top 10 rank based on the current sorted index.
        private void SetPlayerTop10Rank(int index)
        {
            _playerPodcaster.CurrentRank = index + 1;
            _playerPodcaster.RankChange = CalculateRankChange(_playerPodcaster.PreviousRank, _playerPodcaster.CurrentRank);
            PlayerRank = _playerPodcaster.CurrentRank;
        }

        // Simulates a plausible player rank outside the top 10 based on listener proportion.
        private void SimulatePlayerRank()
        {
            int listenersRank10 = _rivalPodcasters
                .OrderByDescending(r => r.CurrentListener)
                .ElementAt(9).CurrentListener;

            float rawProgress = Mathf.Clamp01((float)_playerPodcaster.CurrentListener / listenersRank10);
            float curvedProgress = Mathf.Pow(rawProgress, _climbExponent);

            int simulatedRank = Mathf.Clamp(
                Mathf.RoundToInt(Mathf.Lerp(_startRank, 11, curvedProgress)),
                11, _startRank
            );

            _playerPodcaster.CurrentRank = simulatedRank;
            _playerPodcaster.RankChange = CalculateRankChange(_playerPodcaster.PreviousRank, simulatedRank);
            PlayerRank = simulatedRank;
        }

        // Sorts all podcasters by their current listener count in descending order.
        private List<Podcaster> SortPodcasters(List<Podcaster> podcasters)
        {
            return podcasters.OrderByDescending(r => r.CurrentListener).ToList();
        }

        // Updates the rival podcasters' listener counts based on active articles and genre preferences.
        private void UpdateRivalListeners(List<Article> articles)
        {
            foreach (var rival in _rivalPodcasters)
            {
                bool hasPositive = rival.LikedGenres.Any(g =>
                    articles.Any(a => a.Subgenre == g && a.ValuePositive > 0));
                bool hasNegative = rival.DislikedGenres.Any(g =>
                    articles.Any(a => a.Subgenre == g && a.ValueNegative > 0));

                float multiplier = 1f;
                if (hasPositive && !hasNegative) multiplier = _increaseMultiplier;
                else if (!hasPositive && hasNegative) multiplier = _decreaseMultiplier;

                rival.CurrentListener = Mathf.RoundToInt(rival.CurrentListener * multiplier);
            }
        }

        // Updates the player’s stuff.
        private void UpdatePlayer()
        {
            _playerPodcaster.CurrentListener = _playerListeners.Variable.RuntimeValue;
            _playerPodcaster.DisplayName = _podcastName.Variable.RuntimeValue;
            _playerPodcaster.PersonName = _personName.Variable.RuntimeValue;
        }

        // Updates ranks of all podcasters, giving the player either a real or simulated position.
        private void UpdateRanks(List<Podcaster> sorted)
        {
            bool playerInTop10 = false;

            for (int i = 0; i < sorted.Count; i++)
            {
                var rival = sorted[i];

                if (rival == _playerPodcaster)
                {
                    if (i < 10)
                    {
                        playerInTop10 = true;
                        
                        SetPlayerTop10Rank(i);

                        if (i == 0)
                        {_hasWon.Variable.SetValue(true);}
                        
                    }
                    continue;
                }

                rival.CurrentRank = i + 1;
                rival.RankChange = CalculateRankChange(rival.PreviousRank, rival.CurrentRank);
            }

            if (!playerInTop10 && _playerPodcaster != null)
            {
                SimulatePlayerRank();
            }
        }

        #endregion
    }
}