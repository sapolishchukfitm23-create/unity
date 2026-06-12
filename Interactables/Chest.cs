using UnityEngine;

namespace ClassicPlatformer
{
    // Mechanic 3: chest opened with the E key. The player finds it with
    // Physics2D.Raycast (see Player.TryInteract), not with a trigger.
    [RequireComponent(typeof(SpriteRenderer))]
    public class Chest : MonoBehaviour, IRaycastInteractable
    {
        [Header("Loot")]
        [SerializeField] private int _coinsInside = 5;

        [Header("Visual")]
        [SerializeField] private Sprite _openedSprite;
        [SerializeField] private GameObject _openEffect;
        [SerializeField] private Animator _animator;

        private SpriteRenderer _spriteRenderer;
        private bool _isOpened;

        private static readonly int OpenTrigger = Animator.StringToHash("Open");

        private void Awake()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
        }

        public void InteractByRay(Player player)
        {
            if (_isOpened)
                return;

            _isOpened = true;
            GameManager.Instance.AddCoins(_coinsInside);

            if (_openedSprite != null)
                _spriteRenderer.sprite = _openedSprite;

            if (_animator != null)
                _animator.SetTrigger(OpenTrigger);

            if (_openEffect != null)
                Instantiate(_openEffect, transform.position, Quaternion.identity);
        }
    }
}
