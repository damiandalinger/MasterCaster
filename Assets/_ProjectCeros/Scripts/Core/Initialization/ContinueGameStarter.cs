/// <summary>
/// Starts a continued game session by loading the last saved game and raising the appropriate event.
/// </summary>

/// <remarks>
/// 27/05/2025 by Damian Dalinger: Script creation.
/// 12/08/2025 by Damian Dalinger: Changed load logic to a coroutine.
/// </remarks>

using System.Collections;
using UnityEngine;

namespace ProjectCeros
{
    public class ContinueGameStarter : BaseGameStarter
    {
        #region Fields

        [Tooltip("Event raised when a saved game has been successfully continued.")]
        [SerializeField] private GameEvent _onGameContinued;

        #endregion

        #region Protected Methods

        // Attempts to load the saved game from disk.
        protected override IEnumerator ManagerInitialization()
        {
            var leaderboard = FindFirstObjectByType<LeaderboardDataSync>();
            leaderboard.ImportFromRuntimeSets();

            yield return null;
        }

        // Raises an event indicating the game has been continued.
        protected override void RaiseFinishedEvent()
        {
            _onGameContinued?.Raise();
        }

        // Loads the save.
        protected override IEnumerator ResetAndLoadLogic()
        {
            if (SaveManager.Instance != null && SaveManager.Instance.SaveFileExists())
            {
                yield return SaveManager.Instance.Load();
            }
            else
            {
                Debug.LogWarning("[Continue] No save file found.");
            }
        }

        #endregion
    }
}
