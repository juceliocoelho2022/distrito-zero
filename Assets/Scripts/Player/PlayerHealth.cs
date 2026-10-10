using UnityEngine;
using DistritoZero.Combat;
using DistritoZero.Core;

namespace DistritoZero.Player
{
    public class PlayerHealth : MonoBehaviour, IDamageable
    {
        [SerializeField] private float maxHealth = 100f;
        public float CurrentHealth { get; private set; }
        public bool IsDead { get; private set; }
        public float MaxHealth => maxHealth;

        private void Awake() => ResetHealth();

        public void TakeDamage(float amount)
        {
            if (IsDead || amount <= 0f) return;
            CurrentHealth = Mathf.Max(0f, CurrentHealth - amount);
            if (CurrentHealth <= 0f)
            {
                IsDead = true;
                GameEvents.PublishPlayerDied();
            }
        }

        public void ResetHealth()
        {
            CurrentHealth = maxHealth;
            IsDead = false;
        }
    }
}
