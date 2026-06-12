using UnityEngine;

namespace ClassicPlatformer
{
    // Drives the player's Animator from code based on the player's state:
    // running (Speed), jumping/falling (IsGrounded, VelocityY), taking damage.
    [RequireComponent(typeof(Animator))]
    public class PlayerAnimator : MonoBehaviour
    {
        [SerializeField] private Player _player;

        private Animator _animator;

        private static readonly int SpeedFloat = Animator.StringToHash("Speed");
        private static readonly int IsGroundedBool = Animator.StringToHash("IsGrounded");
        private static readonly int VelocityYFloat = Animator.StringToHash("VelocityY");
        private static readonly int HurtTrigger = Animator.StringToHash("Hurt");

        private int _lastHealth;

        private void Awake()
        {
            _animator = GetComponent<Animator>();

            if (_player == null)
                _player = GetComponent<Player>();

            _lastHealth = _player.CurrentHealth;
            _player.HealthChanged += OnHealthChanged;
        }

        private void OnDestroy()
        {
            if (_player != null)
                _player.HealthChanged -= OnHealthChanged;
        }

        private void Update()
        {
            _animator.SetFloat(SpeedFloat, Mathf.Abs(_player.Velocity.x));
            _animator.SetBool(IsGroundedBool, _player.IsGrounded);
            _animator.SetFloat(VelocityYFloat, _player.Velocity.y);
        }

        private void OnHealthChanged(int current, int max)
        {
            if (current < _lastHealth)
                _animator.SetTrigger(HurtTrigger);

            _lastHealth = current;
        }
    }
}
