/// <summary>
/// Controls the visibility of the continue game button in the main menu.
/// </summary>
///
/// <remarks>
/// 28/05/2025 by Damian Dalinger: Script creation.
/// 10/07/2025 by Damian Dalinger: Added position shifting for New Game button.
/// </remarks>

using UnityEngine;
using UnityEngine.UI;

namespace ProjectCeros
{
    public class MainMenuUIController : MonoBehaviour
    {
        #region Fields

        [SerializeField] private Button _continueButton;

        [Tooltip("Button to start a new game, will shift if continue is hidden.")]
        [SerializeField] private RectTransform _newGameButton;

        [Tooltip("Optional offset applied to New Game button when Continue is hidden.")]
        [SerializeField] private Vector2 _newGameShiftPosition = new(0f, -100f);

        private SaveManager _saveManager;

        #endregion

        #region Lifecycle Methods

        private void Start()
        {
            InitializeContinueButton();
        }

        #endregion

        #region Private Methods

        // Sets the visibility of the Continue button depending on save file availability.
        private void InitializeContinueButton()
        {
            if (_saveManager == null)
                _saveManager = FindFirstObjectByType<SaveManager>();

            bool hasSave = _saveManager != null && _saveManager.SaveFileExists();

            _continueButton.gameObject.SetActive(hasSave);

            if (!hasSave && _newGameButton != null)
            {
                _newGameButton.anchoredPosition = _newGameShiftPosition;
            }
        }

        #endregion
    }
}
