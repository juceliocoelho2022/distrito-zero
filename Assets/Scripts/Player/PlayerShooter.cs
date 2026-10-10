using System;
using UnityEngine;
using DistritoZero.Combat;

namespace DistritoZero.Player
{
    public enum ShotFeedbackState
    {
        None,
        Fired,
        Hit
    }

    public class PlayerShooter : MonoBehaviour
    {
        [SerializeField] private float shotDamage = 30f;
        [SerializeField] private float shotRange = 45f;
        [SerializeField] private float shotCooldown = 0.4f;

        private ShotCooldown cooldown;
        public bool LastShotHit { get; private set; }
        public ShotFeedbackState LastFeedbackState { get; private set; } = ShotFeedbackState.None;
        public float FeedbackUntil { get; private set; }

        private void Awake()
        {
            cooldown = new ShotCooldown(shotCooldown);
        }

        private void Update()
        {
            if (Cursor.lockState != CursorLockMode.Locked) return;
            if (Input.GetMouseButtonDown(0)) TryShoot();
        }

        public bool TryShoot()
        {
            if (cooldown == null) cooldown = new ShotCooldown(shotCooldown);
            if (!cooldown.TryConsume(Time.time)) return false;

            LastShotHit = ShootFromScreenCenter();
            LastFeedbackState = ResolveFeedbackState(LastShotHit);
            FeedbackUntil = Time.time + 0.12f;
            return true;
        }

        public static ShotFeedbackState ResolveFeedbackState(bool hit)
        {
            return hit ? ShotFeedbackState.Hit : ShotFeedbackState.Fired;
        }

        private bool ShootFromScreenCenter()
        {
            Camera camera = Camera.main;
            if (camera == null) return false;

            Ray ray = camera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
            RaycastHit[] hits = Physics.RaycastAll(ray, shotRange, ~0, QueryTriggerInteraction.Ignore);
            Array.Sort(hits, (a, b) => a.distance.CompareTo(b.distance));

            foreach (RaycastHit hit in hits)
            {
                var playerHealth = hit.collider.GetComponentInParent<PlayerHealth>();
                if (playerHealth != null) continue;

                IDamageable damageable = FindDamageable(hit.collider);
                if (damageable != null)
                    return ApplyDamage(damageable, shotDamage);

                return false;
            }

            return false;
        }

        private static IDamageable FindDamageable(Collider collider)
        {
            foreach (var behaviour in collider.GetComponentsInParent<MonoBehaviour>())
            {
                if (behaviour is IDamageable damageable)
                    return damageable;
            }
            return null;
        }

        public static bool ApplyDamage(IDamageable target, float damage)
        {
            if (target == null || target.IsDead || damage <= 0f) return false;
            target.TakeDamage(damage);
            return true;
        }
    }
}
