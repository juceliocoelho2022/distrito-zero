using UnityEngine;
using DistritoZero.Core;

namespace DistritoZero.Progression
{
    public class PlayerExperience : MonoBehaviour
    {
        public int CurrentXp { get; private set; }
        private void OnEnable() => GameEvents.EnemyDefeated += OnEnemy;
        private void OnDisable() => GameEvents.EnemyDefeated -= OnEnemy;
        private void OnEnemy(string _) => AddXp(100);
        public void AddXp(int amount)
        {
            if (amount <= 0) return;
            CurrentXp += amount;
            GameEvents.PublishXpGained(amount);
        }
    }
}
