/// <summary>
/// If the condition is false, raises a GameEvent, which shows the text temporarily. If true, raises a GameEvent.
/// </summary>

/// <remarks>
/// 09/07/2025 by Damian Dalinger: Initial implementation.
/// </remarks>

using UnityEngine;
using UnityEngine.Events;

namespace ProjectCeros
{

    public class CanISleep : MonoBehaviour
    {
        #region Fields

        [Header("Conditions")]
        [Tooltip("Condition that determines whether sleep is allowed. If true, the GameEvent is raised.")]
        [SerializeField] private BoolReference _condition;

        [Tooltip("GameEvent to raise if the condition is true.")]
        [SerializeField] private GameEvent _onConditionTrue;

        [Tooltip("GameEvent to raise if the condition is false.")]
        [SerializeField] private GameEvent _onConditionFalse;

        #endregion

        #region Public Methods

        // Evaluates the condition. If true, raises the GameEvent. If false, it raises the event, which shows a text.
        public void CheckCondition()
        {
            if (_condition.Value)
            {
                _onConditionTrue?.Raise();
            }
            else
            {
                _onConditionFalse?.Raise();
            }
        }

        #endregion
    }
}
