using UnityEngine;
using DistritoZero.Combat;

namespace DistritoZero.Player
{
    public class PlayerCombat : MonoBehaviour
    {
        [SerializeField] private float attackDamage = 25f;
        [SerializeField] private float attackRange = 2.2f;
        [SerializeField] private float attackCooldown = 0.45f;
        private float nextAttackTime;

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.F)) TryAttackNearby();
        }

        public bool TryAttack(IDamageable target)
        {
            if (target == null || target.IsDead || Time.time < nextAttackTime) return false;
            nextAttackTime = Time.time + attackCooldown;
            target.TakeDamage(attackDamage);
            return true;
        }

        private void TryAttackNearby()
        {
            foreach (var hit in Physics.OverlapSphere(transform.position, attackRange))
            {
                IDamageable target = null;
                foreach (var behaviour in hit.GetComponentsInParent<MonoBehaviour>())
                {
                    if (behaviour is IDamageable damageable) { target = damageable; break; }
                }
                if (target != null && target is not PlayerHealth && TryAttack(target)) break;
            }
        }
    }
}
