using UnityEngine;
using DistritoZero.Player;

namespace DistritoZero.Enemies
{
    public class EnemyAI : MonoBehaviour
    {
        [SerializeField] private float detectionRange = 6f;
        [SerializeField] private float stopRange = 1.8f;
        [SerializeField] private float speed = 2.2f;
        private Transform player;
        private EnemyCombat combat;

        private void Start()
        {
            var p = FindFirstObjectByType<PlayerHealth>();
            player = p != null ? p.transform : null;
            combat = GetComponent<EnemyCombat>();
        }

        private void Update()
        {
            if (player == null) return;
            float d = Vector3.Distance(transform.position, player.position);
            if (d > detectionRange) return;
            transform.LookAt(new Vector3(player.position.x, transform.position.y, player.position.z));
            if (d > stopRange) transform.position += transform.forward * (speed * Time.deltaTime);
            else combat?.TryAttack(player.GetComponent<PlayerHealth>());
        }
    }
}
