using UnityEngine;

namespace ClassicPlatformer
{
    // Mechanic 2: pressure plate. Uses Physics2D.OverlapBox to check
    // every physics step whether something heavy stands on the plate.
    public class PressurePlate : MonoBehaviour
    {
        [Header("Detection")]
        [SerializeField] private Vector2 _detectionSize = new Vector2(1f, 0.3f);
        [SerializeField] private Vector2 _detectionOffset = new Vector2(0f, 0.25f);
        [SerializeField] private LayerMask _activatorLayer;

        [Header("Target")]
        [SerializeField] private Doors _doors;

        [Header("Visual")]
        [SerializeField] private SpriteRenderer _spriteRenderer;
        [SerializeField] private Sprite _pressedSprite;
        [SerializeField] private Animator _animator;

        private Sprite _defaultSprite;
        private bool _isPressed;

        private static readonly int IsPressedBool = Animator.StringToHash("IsPressed");

        public bool IsPressed => _isPressed;

        private void Awake()
        {
            if (_spriteRenderer != null)
                _defaultSprite = _spriteRenderer.sprite;
        }

        private void FixedUpdate()
        {
            Vector2 center = (Vector2)transform.position + _detectionOffset;
            Collider2D activator = Physics2D.OverlapBox(center, _detectionSize, 0f, _activatorLayer);

            bool pressedNow = activator != null;
            if (pressedNow == _isPressed)
                return;

            _isPressed = pressedNow;

            if (_isPressed && _doors != null)
                _doors.Open();

            UpdateVisual();
        }

        private void UpdateVisual()
        {
            if (_spriteRenderer != null && _pressedSprite != null)
                _spriteRenderer.sprite = _isPressed ? _pressedSprite : _defaultSprite;

            if (_animator != null)
                _animator.SetBool(IsPressedBool, _isPressed);
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = _isPressed ? Color.green : Color.yellow;
            Gizmos.DrawWireCube((Vector2)transform.position + _detectionOffset, _detectionSize);
        }
    }
}
