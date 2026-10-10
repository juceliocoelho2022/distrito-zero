using UnityEngine;
using DistritoZero.Player;

namespace DistritoZero.Enemies
{
    public class EnemyAI : MonoBehaviour
    {
        [SerializeField] private float detectionRange = 6f;
        [SerializeField] private float loseTargetRange = 9f;
        [SerializeField] private float attackRange = 1.8f;
        [SerializeField] private float speed = 2.2f;
        [SerializeField] private float patrolRadius = 2.5f;
        [SerializeField] private float waypointTolerance = 0.25f;

        private Transform player;
        private PlayerHealth playerHealth;
        private EnemyCombat combat;
        private EnemyStateMachine brain;
        private Vector3 patrolA;
        private Vector3 patrolB;
        private Vector3 patrolTarget;

        public EnemyState CurrentState => brain != null ? brain.CurrentState : EnemyState.Patrol;

        private void Start()
        {
            playerHealth = FindFirstObjectByType<PlayerHealth>();
            player = playerHealth != null ? playerHealth.transform : null;
            combat = GetComponent<EnemyCombat>();
            brain = new EnemyStateMachine(detectionRange, loseTargetRange, attackRange);

            patrolA = transform.position + Vector3.left * patrolRadius;
            patrolB = transform.position + Vector3.right * patrolRadius;
            patrolTarget = patrolB;
        }

        private void Update()
        {
            float distance = player != null
                ? Vector3.Distance(transform.position, player.position)
                : float.PositiveInfinity;

            bool trackingTarget = ShouldTrackTarget(distance);
            bool reachedRoute = Vector3.Distance(transform.position, patrolA) <= waypointTolerance ||
                                Vector3.Distance(transform.position, patrolB) <= waypointTolerance;

            EnemyState state = brain.Tick(trackingTarget, distance, reachedRoute);

            switch (state)
            {
                case EnemyState.Patrol:
                    Patrol();
                    break;
                case EnemyState.Alert:
                    FacePlayer();
                    break;
                case EnemyState.Chase:
                    ChasePlayer();
                    break;
                case EnemyState.Attack:
                    AttackPlayer();
                    break;
                case EnemyState.Return:
                    ReturnToPatrol();
                    break;
            }
        }

        private bool ShouldTrackTarget(float distance)
        {
            if (player == null || playerHealth == null || playerHealth.IsDead)
                return false;

            if (CurrentState == EnemyState.Patrol || CurrentState == EnemyState.Return)
                return distance <= detectionRange;

            return distance < loseTargetRange;
        }

        private void Patrol()
        {
            if (Vector3.Distance(transform.position, patrolTarget) <= waypointTolerance)
                patrolTarget = patrolTarget == patrolA ? patrolB : patrolA;

            MoveTowards(patrolTarget);
        }

        private void ChasePlayer()
        {
            if (player == null) return;
            FacePlayer();
            MoveTowards(player.position);
        }

        private void AttackPlayer()
        {
            if (player == null || playerHealth == null) return;
            FacePlayer();
            combat?.TryAttack(playerHealth);
        }

        private void ReturnToPatrol()
        {
            Vector3 target = Vector3.Distance(transform.position, patrolA) <= Vector3.Distance(transform.position, patrolB)
                ? patrolA
                : patrolB;

            patrolTarget = target;
            MoveTowards(target);
        }

        private void MoveTowards(Vector3 target)
        {
            Vector3 flatTarget = new Vector3(target.x, transform.position.y, target.z);
            Vector3 direction = flatTarget - transform.position;
            if (direction.sqrMagnitude <= 0.001f) return;

            transform.rotation = Quaternion.LookRotation(direction.normalized, Vector3.up);
            transform.position += transform.forward * (speed * Time.deltaTime);
        }

        private void FacePlayer()
        {
            if (player == null) return;
            Vector3 flatPlayer = new Vector3(player.position.x, transform.position.y, player.position.z);
            Vector3 direction = flatPlayer - transform.position;
            if (direction.sqrMagnitude > 0.001f)
                transform.rotation = Quaternion.LookRotation(direction.normalized, Vector3.up);
        }
    }
}
