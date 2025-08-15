/// <summary>
/// Activates the right chair animation depending on the active ids.
/// </summary>
/// <remarks>
/// 15/08/2025 by Damian Dalinger: Script creation.
/// </remarks>

using UnityEngine;

namespace ProjectCeros
{
    public class ChairAnimationController : MonoBehaviour
    {
        #region Fields

        [Tooltip("Runtime Set with active item ids.")]
        [SerializeField] private IntRuntimeSet _activeItems;

        [Header("Chair01")]
        [Tooltip("Id of the first chair.")]

        [SerializeField] private int _idA;
        [Tooltip("GameObject of the first chair animation.")]
        [SerializeField] private GameObject _gameobjectA;

        [Header("Chair02")]
        [Tooltip("Id of the second chair.")]

        [SerializeField] private int _idB;
        [Tooltip("GameObject of the secind chair animation.")]
        [SerializeField] private GameObject _gameobjectB;

        #endregion

        #region Lifecycle Methods

        private void OnEnable()
        {
            bool hasSet = _activeItems != null && _activeItems.Items != null;

            bool matchA = hasSet && _activeItems.Items.Contains(_idA);
            bool matchB = !matchA && hasSet && _activeItems.Items.Contains(_idB); 

            if (_gameobjectA) _gameobjectA.SetActive(matchA);
            if (_gameobjectB) _gameobjectB.SetActive(matchB);

            if (!matchA && !matchB)
            {
                if (_gameobjectA) _gameobjectA.SetActive(false);
                if (_gameobjectB) _gameobjectB.SetActive(false);
            }
        }

        private void OnDisable()
        {
            if (_gameobjectA) _gameobjectA.SetActive(false);
            if (_gameobjectB) _gameobjectB.SetActive(false);
        }

        #endregion
    }
}
