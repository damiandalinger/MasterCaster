/// <summary>
/// Simple UI panel for a single tutorial step: shows headline/body and raises callbacks when the user confirms ("Understood").
/// </summary>

/// <remarks>
/// 11/08/2025 by Damian Dalinger: Script creation.
/// </remarks>

using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

namespace ProjectCeros
{
    public class TutorialPanel : MonoBehaviour
    {
        #region Fields

        [Header("UI References")]

        [Tooltip("Text element used to display the step's title.")]
        [SerializeField] private TMP_Text _headline;

        [Tooltip("Text element used to display the step's description.")]
        [SerializeField] private TMP_Text _body;

        [Tooltip("Button the player clicks to continue this step.")]
        [SerializeField] private Button _understood;

        private Action _onUnderstood;

        #endregion

        #region Lifecycle Methods

        private void Awake()
        {
            if (_understood != null)
                _understood.onClick.AddListener(OnUnderstoodClicked);
        }

        #endregion

        #region Public Methods

        // Binds headline and body text and sets the completion callback.
        public void Bind(string headline, string body, Action onUnderstood)
        {
            if (_headline) _headline.text = headline;
            if (_body) _body.text = body;
            _onUnderstood = onUnderstood;
            gameObject.SetActive(true);
        }

        #endregion

        #region Private Methods

        // Invoked when the "Understood" button is clicked; forwards to the manager callback.
        private void OnUnderstoodClicked()
        {
            _onUnderstood?.Invoke();
        }

        #endregion
    }
}