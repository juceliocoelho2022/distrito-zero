namespace DistritoZero.Enemies
{
    public enum EnemyState
    {
        Patrol,
        Alert,
        Chase,
        Attack,
        Return
    }

    public sealed class EnemyStateMachine
    {
        private readonly float detectionRange;
        private readonly float loseTargetRange;
        private readonly float attackRange;

        public EnemyState CurrentState { get; private set; } = EnemyState.Patrol;

        public EnemyStateMachine(float detectionRange, float loseTargetRange, float attackRange)
        {
            this.detectionRange = detectionRange;
            this.loseTargetRange = loseTargetRange;
            this.attackRange = attackRange;
        }

        public EnemyState Tick(bool hasTarget, float distanceToTarget, bool reachedPatrolRoute)
        {
            if (!hasTarget)
            {
                if (CurrentState == EnemyState.Return && reachedPatrolRoute)
                    return CurrentState = EnemyState.Patrol;

                if (CurrentState != EnemyState.Patrol && CurrentState != EnemyState.Return)
                    return CurrentState = EnemyState.Return;

                return CurrentState;
            }

            if (CurrentState == EnemyState.Return && distanceToTarget <= detectionRange)
                return CurrentState = EnemyState.Alert;

            if (distanceToTarget >= loseTargetRange && CurrentState != EnemyState.Patrol)
                return CurrentState = EnemyState.Return;

            if (CurrentState == EnemyState.Patrol)
            {
                if (distanceToTarget <= detectionRange)
                    return CurrentState = EnemyState.Alert;

                return CurrentState;
            }

            if (CurrentState == EnemyState.Alert)
            {
                if (distanceToTarget <= attackRange)
                    return CurrentState = EnemyState.Attack;

                return CurrentState = EnemyState.Chase;
            }

            if (distanceToTarget <= attackRange)
                return CurrentState = EnemyState.Attack;

            return CurrentState = EnemyState.Chase;
        }
    }
}
