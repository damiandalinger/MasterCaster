/// <summary>
/// Controls the visibility of the continue game button in the main menu.
/// </summary>

/// <remarks>
/// 28/05/2025 by Damian Dalinger: Script creation.
/// </remarks>

using UnityEngine;
using UnityEngine.UI;

namespace ProjectCeros
{
    public class MainMenuUIController : MonoBehaviour
    {
        #region Fields

        [SerializeField] private Button _continueButton;
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

            _continueButton.gameObject.SetActive(_saveManager != null && _saveManager.SaveFileExists());
        }

        #endregion
    }
}
