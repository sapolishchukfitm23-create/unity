using UnityEngine;

namespace ClassicPlatformer
{
    // Mechanic 1: bounce pad. Uses the COLLISION event (OnCollisionEnter2D),
    // not a trigger - the pad is a solid object the player lands on.
    public class JumpPad : MonoBehaviour
    {
        [Header("Bounce")]
        [SerializeField] private float _bounceForce = 22f;

        [Header("Visual")]
        [SerializeField] private Animator _animator;

        private static readonly int BounceTrigger = Animator.StringToHash("Bounce");

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (!collision.gameObject.TryGetComponent(out Player player))
                return;

            // Bounce only if the player landed on top of the pad.
            if (collision.contacts[0].normal.y < -0.5f)
            {
                player.Bounce(_bounceForce);

                if (_animator != null)
                    _animator.SetTrigger(BounceTrigger);
            }
        }
    }
}
