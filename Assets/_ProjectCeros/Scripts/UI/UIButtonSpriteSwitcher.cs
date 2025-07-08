/// <summary>
/// Allows one selectable button at a time. Automatically finds Button and Image components,
/// and uses the button's initial sprite as the default (normal) state.
/// </summary>

/// <remarks>
/// 25/06/2025 by Damian Dalinger: Initial implementation.
/// 08/07/2025 by Unik Kelmendi: extruded nested class.
/// </remarks>

using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

namespace ProjectCeros
{
    public class UIButtonSpriteSwitcher : MonoBehaviour
    {

        #region Fields

        [Tooltip("List of buttons that can be toggled. Only one can be selected at a time.")]
        [SerializeField] private List<SelectableButton> _buttons = new();

        private SelectableButton _currentlySelected;

        #endregion

        #region Lifecycle Methods
        private void Awake()
        {
            foreach (var entry in _buttons)
            {
                entry.button = entry.buttonObject.GetComponent<Button>();
                entry.image = entry.buttonObject.GetComponent<Image>();
                entry.normalSprite = entry.image.sprite;
                entry.button.onClick.AddListener(() => OnButtonClicked(entry));
            }
        }

        #endregion

        #region Public Methods

        // Deselects all buttons and reverts them to their default (normal) sprite.
        public void ResetSelection()
        {
            foreach (var btn in _buttons)
            {
                if (btn.image != null && btn.normalSprite != null)
                    btn.image.sprite = btn.normalSprite;
            }

            _currentlySelected = null;
        }

        #endregion

        #region Private Methods
        
        // Resets the selection and updates the sprite of the clicked button.
        public void OnButtonClicked(SelectableButton clicked)
        {
            ResetSelection();
            clicked.image.sprite = clicked.selectedSprite;
            _currentlySelected = clicked;
        }

        #endregion
    }
}
