/// <summary>
/// Reusable 2D button component using SpriteRenderer and Collider2D.
/// Changes sprite on hover and triggers UnityEvents on click.
/// </summary>

/// <remarks>
/// 09/07/2025 by Damian Dalinger: Initial implementation.
/// </remarks>

using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace ProjectCeros
{
    [RequireComponent(typeof(SpriteRenderer))]
    [RequireComponent(typeof(Collider2D))]
    public class SpriteButton : MonoBehaviour
    {
        #region Fields

        [Tooltip("Sprite to use when mouse hovers. Leave empty to disable hover effect.")]
        [SerializeField] private Sprite _hoverSprite;

        [Tooltip("Cooldown time (in seconds) between accepted clicks.")]
        [SerializeField] private float _clickCooldown = 0.2f;

        [Tooltip("Event invoked when the sprite is clicked.")]
        [SerializeField] private UnityEvent _onClick;

        private SpriteRenderer _spriteRenderer;
        private Sprite _normalSprite;
        private bool _isHovering;
        private bool _clickLocked;

        #endregion

        #region Lifecycle Methods

        private void Awake()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
            _normalSprite = _spriteRenderer.sprite;
        }

        #endregion

        #region Private Methods

        // Sets the hover sprite if assigned.
        private void OnMouseEnter()
        {
            if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject())
                return;

            _isHovering = true;

            if (_hoverSprite != null)
                _spriteRenderer.sprite = _hoverSprite;
        }

        // Restores the normal sprite.
        private void OnMouseExit()
        {
            _isHovering = false;
            _spriteRenderer.sprite = _normalSprite;
        }

        // Invokes the assigned UnityEvent.
        private void OnMouseDown()
        {
            if (_clickLocked)
                return;

            // Block input if pointer is over a UI element
            if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject())
                return;

            _clickLocked = true;
            _onClick?.Invoke();
            Invoke(nameof(UnlockClick), _clickCooldown);
        }

        // Restores the sprite based on current hover state.
        private void OnMouseUpAsButton()
        {
            _spriteRenderer.sprite = (_isHovering && _hoverSprite != null)
                ? _hoverSprite
                : _normalSprite;
        }

        // Unlocks the click lock after cooldown.
        private void UnlockClick()
        {
            _clickLocked = false;
        }

        #endregion
    }
}
