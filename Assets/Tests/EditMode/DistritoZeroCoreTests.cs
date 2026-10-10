using NUnit.Framework;
using DistritoZero.Core;
using DistritoZero.Challenges;
using DistritoZero.Missions;

namespace DistritoZero.Tests.EditMode
{
    public class DistritoZeroCoreTests
    {
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
    }
}
