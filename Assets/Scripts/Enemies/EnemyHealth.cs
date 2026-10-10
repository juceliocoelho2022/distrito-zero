using UnityEngine;
using DistritoZero.Combat;
using DistritoZero.Core;

namespace DistritoZero.Enemies
{
    public class EnemyHealth : MonoBehaviour, IDamageable
    {
        [SerializeField] private float maxHealth = 75f;
        [SerializeField] private string enemyId = "security-drone-01";
        public float CurrentHealth { get; private set; }
        public bool IsDead { get; private set; }

        private void Awake() => CurrentHealth = maxHealth;

        public void TakeDamage(float amount)
        {
            if (IsDead || amount <= 0f) return;
            CurrentHealth = Mathf.Max(0f, CurrentHealth - amount);
            if (CurrentHealth <= 0f)
            {
                IsDead = true;
                GameEvents.PublishEnemyDefeated(enemyId);
                gameObject.SetActive(false);
            }
        }
    }
}
