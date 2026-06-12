using UnityEngine;

namespace ClassicPlatformer
{
    // Mechanic 4 (extra): checkpoint. Classic trigger (OnTriggerEnter2D
    // via BaseInteractable) - saves the respawn point for the player.
    public class Checkpoint : BaseInteractable
    {
        [Header("Visual")]
        [SerializeField] private SpriteRenderer _spriteRenderer;
        [SerializeField] private Sprite _activatedSprite;
        [SerializeField] private Animator _animator;

        private bool _isActivated;

        private static readonly int ActivatedBool = Animator.StringToHash("IsActivated");

        public override void Interact(Player player)
        {
            if (_isActivated)
                return;

            _isActivated = true;
            player.SetRespawnPoint(transform.position);

            if (_spriteRenderer != null && _activatedSprite != null)
                _spriteRenderer.sprite = _activatedSprite;

            if (_animator != null)
                _animator.SetBool(ActivatedBool, true);
        }
    }
}
