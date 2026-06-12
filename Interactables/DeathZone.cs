using UnityEngine;

namespace ClassicPlatformer
{
    // Mechanic 5 (extra): death zone below the level. A trigger that either
    // returns the player to the last checkpoint (with damage) or kills them.
    public class DeathZone : BaseInteractable
    {
        [Header("Damage")]
        [SerializeField] private int _damage = 1;
        [SerializeField] private bool _instantKill;

        public override void Interact(Player player)
        {
            if (_instantKill)
            {
                player.TakeDamage(999);
                return;
            }

            player.TakeDamage(_damage);
            player.Respawn();
        }
    }
}
