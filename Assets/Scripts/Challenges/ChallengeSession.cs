using DistritoZero.Core;

namespace DistritoZero.Challenges
{
    public sealed class ChallengeSession
    {
        private readonly int correctIndex;
        public bool IsCompleted { get; private set; }
        public ChallengeSession(int correctIndex) => this.correctIndex = correctIndex;

        public bool Submit(int answerIndex)
        {
            if (IsCompleted) return answerIndex == correctIndex;
            if (answerIndex != correctIndex) return false;
            IsCompleted = true;
            GameEvents.PublishChallengeCompleted();
            return true;
        }
    }
}
