/// <summary>
/// Starts a new game by resetting variables and initializing game systems.
/// </summary>

/// <remarks>
/// 13/05/2025 by Damian Dalinger: Script creation.
/// 27/05/2025 by Damian Dalinger: Implemented NewGameStarter as BaseGameStarter child.
/// </remarks>

using System.Collections;
using UnityEngine;

namespace ProjectCeros
{
    public class NewGameStarter : BaseGameStarter
    {
        #region Fields

        [Tooltip("Event raised after a new game has been started.")]
        [SerializeField] private GameEvent _onNewGameStarted;

        [Tooltip("The variable tracking how many listeners the player has.")]
        [SerializeField] private IntVariable _listenerCount;

        [Tooltip("The variable representing the current in-game day.")]
        [SerializeField] private IntVariable _currentDay;

        #endregion

        #region Protected Methods

        // Resets the data from the last play.
        protected override void ResetInitialGameState()
        {
            SaveManager.Instance?.DeleteSave();
            _listenerCount.SetValue(0);
            _currentDay.SetValue(1);
        }

        // Initializes managers.
        protected override IEnumerator ManagerInitialization()
        {
            yield return null;

            var importer = FindFirstObjectByType<NewsImporter>();
            importer?.ImportAll();
            Destroy(importer?.gameObject);

            var reshuffler = FindFirstObjectByType<NewsDatabaseReshuffler>();
            reshuffler?.RefreshAllPools();
        }

        // Raises an event signaling that a new game has started.
        protected override void RaiseFinishedEvent()
        {
            _onNewGameStarted?.Raise();
        }

        #endregion
    }
}
