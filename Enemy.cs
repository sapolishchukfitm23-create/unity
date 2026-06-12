using UnityEngine;

namespace ClassicPlatformer
{
    // Updated for Lab 4: drives the walk/death animation from code.
    public class Enemy : MonoBehaviour
    {
        [Header("Patrol")]
        [SerializeField] private float _patrolSpeed = 2f;
        [SerializeField] private Transform _leftPoint;
        [SerializeField] private Transform _rightPoint;

        [Header("Visual")]
        [SerializeField] private SpriteRenderer _spriteRenderer;
        [SerializeField] private Animator _animator;

        [Header("Damage")]
        [SerializeField] private int _damage = 1;

        [Header("Death")]
        [SerializeField] private float _deathAnimationDuration = 0.4f;

        private int _direction = 1;
        private bool _isDead;

        private static readonly int SpeedFloat = Animator.StringToHash("Speed");
        private static readonly int DieTrigger = Animator.StringToHash("Die");

        private void Update()
        {
            if (_isDead)
                return;

            Patrol();
            CheckPatrolBounds();

            if (_animator != null)
                _animator.SetFloat(SpeedFloat, _patrolSpeed);
        }

        private void Patrol()
        {
            transform.Translate(Vector2.right * _direction * _patrolSpeed * Time.deltaTime);

            if (_spriteRenderer != null)
                _spriteRenderer.flipX = _direction < 0;
        }

        private void CheckPatrolBounds()
        {
            if (_leftPoint != null && transform.position.x <= _leftPoint.position.x)
                _direction = 1;
            else if (_rightPoint != null && transform.position.x >= _rightPoint.position.x)
                _direction = -1;
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (_isDead)
                return;

            var player = collision.gameObject.GetComponent<Player>();
            if (player == null) return;

            if (collision.contacts[0].normal.y < -0.5f)
            {
                Die();
            }
            else
            {
                player.TakeDamage(_damage);
            }
        }

        private void Die()
        {
            _isDead = true;

            foreach (var col in GetComponents<Collider2D>())
                col.enabled = false;

            if (_animator != null)
            {
                _animator.SetTrigger(DieTrigger);
                Destroy(gameObject, _deathAnimationDuration);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        private void OnDrawGizmosSelected()
        {
            if (_leftPoint != null && _rightPoint != null)
            {
                Gizmos.color = Color.yellow;
                Gizmos.DrawLine(_leftPoint.position, _rightPoint.position);
            }
        }
    }
}
