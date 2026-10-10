using UnityEngine;

namespace DistritoZero.Challenges
{
    [CreateAssetMenu(menuName = "Distrito Zero/Challenge Data")]
    public class ChallengeData : ScriptableObject
    {
        [TextArea] public string question;
        public string[] answers;
        public int correctAnswerIndex;
    }
}
