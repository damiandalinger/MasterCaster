using UnityEngine;
using UnityEngine.UI;

namespace ProjectCeros
{
    public class MainMenuUIController : MonoBehaviour
    {
        [SerializeField] private Button _continueButton;
        private SaveManager _saveManager;


        private void Start()
        {
            if (_saveManager == null)
                _saveManager = FindFirstObjectByType<SaveManager>(); // Fallback

            _continueButton.gameObject.SetActive(_saveManager != null && _saveManager.SaveFileExists());
        }
    }
}
