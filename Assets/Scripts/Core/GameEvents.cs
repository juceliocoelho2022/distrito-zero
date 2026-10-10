using System;

namespace DistritoZero.Core
{
    public static class GameEvents
    {
        public static event Action ChallengeCompleted;
        public static event Action<string> EnemyDefeated;
        public static event Action PlayerDied;
        public static event Action ChapterCompleted;
        public static event Action<int> XpGained;

        public static void PublishChallengeCompleted() => ChallengeCompleted?.Invoke();
        public static void PublishEnemyDefeated(string enemyId) => EnemyDefeated?.Invoke(enemyId);
        public static void PublishPlayerDied() => PlayerDied?.Invoke();
        public static void PublishChapterCompleted() => ChapterCompleted?.Invoke();
        public static void PublishXpGained(int amount) => XpGained?.Invoke(amount);

        public static void ResetAll()
        {
            ChallengeCompleted = null;
            EnemyDefeated = null;
            PlayerDied = null;
            ChapterCompleted = null;
            XpGained = null;
        }
    }
}
