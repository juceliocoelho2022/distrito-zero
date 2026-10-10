using NUnit.Framework;
using DistritoZero.Core;
using DistritoZero.Challenges;
using DistritoZero.Missions;
using DistritoZero.Player;
using DistritoZero.Combat;
using DistritoZero.Enemies;

namespace DistritoZero.Tests.EditMode
{
    public class DistritoZeroCoreTests
    {
        private sealed class FakeDamageable : IDamageable
        {
            public bool IsDead { get; private set; }
            public float DamageTaken { get; private set; }

            public void TakeDamage(float amount)
            {
                if (IsDead || amount <= 0f) return;
                DamageTaken += amount;
            }
        }

        [SetUp]
        public void Setup() => GameEvents.ResetAll();

        [Test]
        public void Challenge_WrongAnswer_AllowsRetry_AndDoesNotPublishCompletion()
        {
            var published = 0;
            GameEvents.ChallengeCompleted += () => published++;
            var challenge = new ChallengeSession(1);

            Assert.IsFalse(challenge.Submit(0));
            Assert.IsFalse(challenge.IsCompleted);
            Assert.AreEqual(0, published);

            Assert.IsTrue(challenge.Submit(1));
            Assert.IsTrue(challenge.IsCompleted);
            Assert.AreEqual(1, published);
        }

        [Test]
        public void Challenge_Completion_IsPublishedOnlyOnce()
        {
            var published = 0;
            GameEvents.ChallengeCompleted += () => published++;
            var challenge = new ChallengeSession(0);

            Assert.IsTrue(challenge.Submit(0));
            Assert.IsTrue(challenge.Submit(0));
            Assert.AreEqual(1, published);
        }

        [Test]
        public void Mission_Ignores_OutOfOrderEvents()
        {
            var mission = new MissionStateMachine();
            Assert.AreEqual(MissionStep.FindTerminal, mission.CurrentStep);
            mission.Advance(MissionEvent.EnemyDefeated);
            Assert.AreEqual(MissionStep.FindTerminal, mission.CurrentStep);
        }

        [TestCase(-120f, -70f)]
        [TestCase(120f, 70f)]
        [TestCase(25f, 25f)]
        public void Camera_ClampPitch_RespectsConfiguredLimits(float input, float expected)
        {
            Assert.AreEqual(expected, ThirdPersonCamera.ClampPitch(input, -70f, 70f));
        }

        [Test]
        public void Shooting_ValidTarget_ReceivesConfiguredDamage()
        {
            var target = new FakeDamageable();

            Assert.IsTrue(PlayerShooter.ApplyDamage(target, 30f));
            Assert.AreEqual(30f, target.DamageTaken);
        }

        [Test]
        public void Shooting_Cooldown_BlocksUntilNextAllowedTime()
        {
            var gate = new ShotCooldown(0.4f);

            Assert.IsTrue(gate.TryConsume(1.0f));
            Assert.IsFalse(gate.TryConsume(1.2f));
            Assert.IsTrue(gate.TryConsume(1.4f));
        }

        [Test]
        public void EnemyAI_PlayerDetected_TransitionsFromPatrolToAlertThenChase()
        {
            var brain = new EnemyStateMachine(6f, 9f, 1.8f);

            Assert.AreEqual(EnemyState.Patrol, brain.CurrentState);
            Assert.AreEqual(EnemyState.Alert, brain.Tick(true, 5f, false));
            Assert.AreEqual(EnemyState.Chase, brain.Tick(true, 5f, false));
        }

        [Test]
        public void EnemyAI_ClosePlayer_TransitionsToAttack_AndBackToChase()
        {
            var brain = new EnemyStateMachine(6f, 9f, 1.8f);

            brain.Tick(true, 5f, false);
            brain.Tick(true, 5f, false);
            Assert.AreEqual(EnemyState.Attack, brain.Tick(true, 1.5f, false));
            Assert.AreEqual(EnemyState.Chase, brain.Tick(true, 3f, false));
        }

        [Test]
        public void EnemyAI_LostPlayer_ReturnsToPatrolRoute()
        {
            var brain = new EnemyStateMachine(6f, 9f, 1.8f);

            brain.Tick(true, 5f, false);
            brain.Tick(true, 5f, false);
            Assert.AreEqual(EnemyState.Return, brain.Tick(true, 10f, false));
            Assert.AreEqual(EnemyState.Patrol, brain.Tick(false, float.PositiveInfinity, true));
        }
    }
}
