using NUnit.Framework;
using UnityEngine;
using DistritoZero.Core;
using DistritoZero.Player;
using DistritoZero.Enemies;

namespace DistritoZero.Tests.EditMode
{
    public class DistritoZeroHealthTests
    {
        [SetUp]
        public void Setup() => GameEvents.ResetAll();

        [Test]
        public void PlayerDeath_IsPublishedOnce_AndResetRestoresHealth()
        {
            var go = new GameObject("player-test");
            var health = go.AddComponent<PlayerHealth>();
            var deaths = 0;
            GameEvents.PlayerDied += () => deaths++;

            health.TakeDamage(1000f);
            health.TakeDamage(10f);
            Assert.IsTrue(health.IsDead);
            Assert.AreEqual(1, deaths);

            health.ResetHealth();
            Assert.IsFalse(health.IsDead);
            Assert.Greater(health.CurrentHealth, 0f);
            Object.DestroyImmediate(go);
        }

        [Test]
        public void EnemyDeath_IsPublishedOnce()
        {
            var go = new GameObject("enemy-test");
            var health = go.AddComponent<EnemyHealth>();
            var deaths = 0;
            GameEvents.EnemyDefeated += _ => deaths++;

            health.TakeDamage(1000f);
            health.TakeDamage(10f);
            Assert.AreEqual(1, deaths);
            Object.DestroyImmediate(go);
        }
    }
}
