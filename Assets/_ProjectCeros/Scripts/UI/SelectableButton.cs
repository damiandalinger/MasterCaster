using UnityEngine;
using UnityEngine.UI;

namespace ProjectCeros
{

    [System.Serializable]
    public class SelectableButton
    {
        [Tooltip("The GameObject that contains both a Button and Image component.")]
        public GameObject buttonObject;

        [Tooltip("The sprite used when this button is selected.")]
        public Sprite selectedSprite;

        [HideInInspector] public Button button;
        [HideInInspector] public Image image;
        [HideInInspector] public Sprite normalSprite;
    }
}
