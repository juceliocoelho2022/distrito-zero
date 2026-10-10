namespace DistritoZero.Missions
{
    public sealed class MissionStateMachine
    {
        public MissionStep CurrentStep { get; private set; } = MissionStep.FindTerminal;

        public bool Advance(MissionEvent evt)
        {
            MissionStep next = CurrentStep;
            switch (CurrentStep)
            {
                case MissionStep.FindTerminal when evt == MissionEvent.TerminalActivated: next = MissionStep.SolveChallenge; break;
                case MissionStep.SolveChallenge when evt == MissionEvent.ChallengeSolved: next = MissionStep.DefeatDrone; break;
                case MissionStep.DefeatDrone when evt == MissionEvent.EnemyDefeated: next = MissionStep.ReachExit; break;
                case MissionStep.ReachExit when evt == MissionEvent.ExitReached: next = MissionStep.Complete; break;
            }
            if (next == CurrentStep) return false;
            CurrentStep = next;
            return true;
        }
    }
}
