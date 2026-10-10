using UnityEngine;
using DistritoZero.Player;

namespace DistritoZero.Enemies
{
    public class EnemyCombat : MonoBehaviour
    {
        [SerializeField] private float damage = 10f;
        [SerializeField] private float cooldown = 1f;
        private float nextAttack;

        public bool TryAttack(PlayerHealth target)
        {
            if (target == null || target.IsDead || Time.time < nextAttack) return false;
            nextAttack = Time.time + cooldown;
            target.TakeDamage(damage);
            return true;
        }
    }
}
