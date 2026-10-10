using UnityEngine;

namespace DistritoZero.Challenges
{
    public class ChallengeManager : MonoBehaviour
    {
        public static ChallengeManager Instance { get; private set; }
        public bool IsVisible { get; private set; }
        public string Feedback { get; private set; } = string.Empty;
        private ChallengeSession session;

        private void Awake() => Instance = this;

        public void StartDefaultChallenge()
        {
            session = new ChallengeSession(0);
            IsVisible = true;
            Feedback = "";
        }

        public bool SubmitAnswer(int answerIndex)
        {
            if (session == null) return false;
            bool correct = session.Submit(answerIndex);
            Feedback = correct ? "Correto! Energia restaurada." : "Resposta incorreta. Tente novamente.";
            if (correct) IsVisible = false;
            return correct;
        }
    }
}
