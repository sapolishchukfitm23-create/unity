using System;
using TMPro;
using UnityEngine;

namespace ClassicPlatformer
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class Player : MonoBehaviour
    {
        [Header("Movement")]
        [SerializeField] private float _moveSpeed = 7f;
        [SerializeField] private float _climbSpeed = 3.5f;
        [SerializeField] private float _jumpForce = 14f;

        [Header("Ground Detection")]
        [SerializeField] private Transform _groundCheck;
        [SerializeField] private float _groundCheckRadius = 0.2f;
        [SerializeField] private LayerMask _groundLayer;

        [Header("Interaction")]
        [SerializeField] private float _interactionDistance = 1.5f;
        [SerializeField] private LayerMask _interactionLayer;

        [Header("Visual")]
        [SerializeField] private SpriteRenderer _spriteRenderer;

        [Header("Health")]
        [SerializeField] private int _maxHealth = 3;

        [Header("Invincibility")]
        [SerializeField] private float _invincibilityDuration = 1.5f;

        [Header("UI")]
        [SerializeField] private TextMeshProUGUI _healthText;

        private int _currentHealth;
        private float _invincibilityTimer;
        private bool _isInvincible;

        public int CurrentHealth => _currentHealth;
        public int MaxHealth => _maxHealth;
        public bool IsGrounded => _isGrounded;
        public Vector2 Velocity => _rb.linearVelocity;

        public event Action<int, int> HealthChanged;

        private Rigidbody2D _rb;
        private float _horizontalInput;
        private float _verticalMovement;
        private bool _isGrounded;
        private bool _verticalMovementEnabled;
        private Vector3 _respawnPoint;

        private void Awake()
        {
            _currentHealth = _maxHealth;
            _rb = GetComponent<Rigidbody2D>();
            _respawnPoint = transform.position;
            UpdateUI();
        }

        private void Update()
        {
            if (GameManager.Instance != null && GameManager.Instance.State != GameState.Playing)
            {
                _horizontalInput = 0f;
                _verticalMovement = 0f;
                return;
            }

            _horizontalInput = Input.GetAxisRaw("Horizontal");
            _verticalMovement = Input.GetAxisRaw("Vertical");

            _isGrounded = Physics2D.OverlapCircle(_groundCheck.position, _groundCheckRadius, _groundLayer);

            if (Input.GetButtonDown("Jump") && _isGrounded)
            {
                Jump();
            }

            if (Input.GetKeyDown(KeyCode.E))
            {
                TryInteract();
            }

            if (_isInvincible)
            {
                _invincibilityTimer -= Time.deltaTime;
                if (_invincibilityTimer <= 0f)
                    _isInvincible = false;
            }
        }

        private void FixedUpdate()
        {
            float velocityY = _verticalMovementEnabled ? _verticalMovement * _climbSpeed : _rb.linearVelocity.y;
            _rb.linearVelocity = new Vector2(_horizontalInput * _moveSpeed, velocityY);

            if (_spriteRenderer != null && _horizontalInput != 0)
                _spriteRenderer.flipX = _horizontalInput < 0;
        }

        public void EnableVerticalMovement(bool enabled)
        {
            _rb.bodyType = enabled ? RigidbodyType2D.Kinematic : RigidbodyType2D.Dynamic;
            _verticalMovementEnabled = enabled;
        }

        public void SetRespawnPoint(Vector3 position)
        {
            _respawnPoint = position;
        }

        public void Respawn()
        {
            _rb.linearVelocity = Vector2.zero;
            transform.position = _respawnPoint;
        }

        public void Bounce(float force)
        {
            _rb.linearVelocity = new Vector2(_rb.linearVelocity.x, force);
        }

        private void Jump()
        {
            _rb.linearVelocity = new Vector2(_rb.linearVelocity.x, _jumpForce);
        }

        private void TryInteract()
        {
            Vector2 direction = _spriteRenderer != null && _spriteRenderer.flipX ? Vector2.left : Vector2.right;
            RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, _interactionDistance, _interactionLayer);

            Debug.DrawRay(transform.position, direction * _interactionDistance, Color.cyan, 0.5f);

            if (hit.collider != null && hit.collider.TryGetComponent(out IRaycastInteractable interactable))
            {
                interactable.InteractByRay(this);
            }
        }

        public void TakeDamage(int damage = 1)
        {
            if (_isInvincible || _currentHealth <= 0) return;

            _currentHealth -= damage;
            _currentHealth = Mathf.Max(_currentHealth, 0);
            HealthChanged?.Invoke(_currentHealth, _maxHealth);
            UpdateUI();

            if (_currentHealth <= 0)
            {
                if (GameManager.Instance != null)
                    GameManager.Instance.LoseGame();

                gameObject.SetActive(false);
            }
            else
            {
                _isInvincible = true;
                _invincibilityTimer = _invincibilityDuration;
            }
        }

        public void Heal(int amount = 1)
        {
            if (_currentHealth <= 0) return;

            _currentHealth += amount;
            _currentHealth = Mathf.Min(_currentHealth, _maxHealth);
            HealthChanged?.Invoke(_currentHealth, _maxHealth);
            UpdateUI();
        }

        private void UpdateUI()
        {
            if (_healthText != null)
                _healthText.text = $"HP: {_currentHealth}/{_maxHealth}";
        }

        private void OnDrawGizmosSelected()
        {
            if (_groundCheck != null)
            {
                Gizmos.color = _isGrounded ? Color.green : Color.red;
                Gizmos.DrawWireSphere(_groundCheck.position, _groundCheckRadius);
            }
        }
    }
}
