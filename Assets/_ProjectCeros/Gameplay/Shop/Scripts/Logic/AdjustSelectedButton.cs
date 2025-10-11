/// <summary>
/// This script shows the correct item once the player clicks on the button that is connected to the
/// item info in the shop tab.
/// </summary>

/// <remarks>
/// 18/06/2025 by Unik Kelmendi: Initial creation.
/// </remarks>

using UnityEngine;
using UnityEngine.UI;


namespace ProjectCeros
{

    public class AdjustSelectedButton : MonoBehaviour
    {

        [SerializeField] private SelectableButton _currentlySelected;

        [SerializeField] private UIButtonSpriteSwitcher _switcher;

        private void Awake()
        {
            _currentlySelected.button = _currentlySelected.buttonObject.GetComponent<Button>();
            _currentlySelected.image = _currentlySelected.buttonObject.GetComponent<Image>();
            _currentlySelected.normalSprite = _currentlySelected.image.sprite;

            TransferSwitchData();
        }



        public void TransferSwitchData()
        {
            _switcher.OnButtonClicked(_currentlySelected);
        }
    }
}