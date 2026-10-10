using UnityEngine;
using DistritoZero.Core;

namespace DistritoZero.Missions
{
    public class MissionManager : MonoBehaviour
    {
        public static MissionManager Instance { get; private set; }
        public MissionStateMachine State { get; private set; }
        public MissionStep CurrentStep => State.CurrentStep;

        private void Awake() { Instance = this; State = new MissionStateMachine(); }
        private void OnEnable()
        {
            GameEvents.ChallengeCompleted += OnChallenge;
            GameEvents.EnemyDefeated += OnEnemy;
        }
        private void OnDisable()
        {
            GameEvents.ChallengeCompleted -= OnChallenge;
            GameEvents.EnemyDefeated -= OnEnemy;
        }

        public void TerminalActivated() => State.Advance(MissionEvent.TerminalActivated);
        private void OnChallenge() => State.Advance(MissionEvent.ChallengeSolved);
        private void OnEnemy(string _) => State.Advance(MissionEvent.EnemyDefeated);
        public void ExitReached()
        {
            if (State.Advance(MissionEvent.ExitReached)) GameEvents.PublishChapterCompleted();
        }

        public string ObjectiveText => CurrentStep switch
        {
            MissionStep.FindTerminal => "Encontre o terminal de rede.",
            MissionStep.SolveChallenge => "Resolva o desafio de lógica.",
            MissionStep.DefeatDrone => "Neutralize o drone de segurança. [F]",
            MissionStep.ReachExit => "Vá até a saída iluminada.",
            _ => "Capítulo concluído!"
        };
    }
}
