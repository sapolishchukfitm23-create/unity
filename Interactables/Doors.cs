using UnityEngine;

namespace ClassicPlatformer
{
    // Updated: entering the open door now WINS the game (game state system)
    // instead of just reloading the scene.
    [RequireComponent(typeof(SpriteRenderer))]
    public class Doors : BaseInteractable
    {
        [SerializeField] private Sprite _openDoors;
        [SerializeField] private Animator _animator;

        private SpriteRenderer _spriteRenderer;
        private bool _isOpen;

        private static readonly int OpenTrigger = Animator.StringToHash("Open");

        private void Awake()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
        }

        protected override void OnTriggerEnter2D(Collider2D other)
        {
            if (!_isOpen)
                return;

            base.OnTriggerEnter2D(other);
        }

        public void Open()
        {
            if (_isOpen)
                return;

            _isOpen = true;

            if (_openDoors != null)
                _spriteRenderer.sprite = _openDoors;

            if (_animator != null)
                _animator.SetTrigger(OpenTrigger);
        }

        public override void Interact(Player player)
        {
            GameManager.Instance.WinGame();
        }
    }
}
