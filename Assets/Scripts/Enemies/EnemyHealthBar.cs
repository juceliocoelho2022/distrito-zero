using UnityEngine;

namespace DistritoZero.Enemies
{
    public class EnemyHealthBar : MonoBehaviour
    {
        [SerializeField] private Vector3 worldOffset = new Vector3(0f, 1.35f, 0f);
        [SerializeField] private float barWidth = 84f;
        [SerializeField] private float barHeight = 9f;
        [SerializeField] private float popupDuration = 0.35f;

        private EnemyHealth health;
        private float previousHealth;
        private float lastDamage;
        private float popupShownAt = -1f;

        private void Awake()
        {
            health = GetComponent<EnemyHealth>();
            if (health != null)
                previousHealth = health.CurrentHealth;
        }

        private void Update()
        {
            if (health == null) return;

            if (health.CurrentHealth < previousHealth)
            {
                lastDamage = previousHealth - health.CurrentHealth;
                popupShownAt = Time.time;
            }

            previousHealth = health.CurrentHealth;
        }

        private void OnGUI()
        {
            if (health == null || health.IsDead) return;

            Camera camera = Camera.main;
            if (camera == null) return;

            Vector3 screen = camera.WorldToScreenPoint(transform.position + worldOffset);
            if (screen.z <= 0f) return;

            float x = screen.x - barWidth * 0.5f;
            float y = Screen.height - screen.y;
            Rect background = new Rect(x, y, barWidth, barHeight);
            Rect fill = new Rect(x + 1f, y + 1f, (barWidth - 2f) * health.HealthRatio, barHeight - 2f);

            Color previousColor = GUI.color;

            GUI.color = new Color(0.05f, 0.05f, 0.07f, 0.9f);
            GUI.DrawTexture(background, Texture2D.whiteTexture);

            GUI.color = HealthColor(health.HealthRatio);
            GUI.DrawTexture(fill, Texture2D.whiteTexture);

            GUI.color = Color.white;
            GUIStyle labelStyle = new GUIStyle(GUI.skin.label)
            {
                alignment = TextAnchor.MiddleCenter,
                fontSize = 11,
                fontStyle = FontStyle.Bold
            };
            GUI.Label(new Rect(x - 8f, y - 20f, barWidth + 16f, 18f), $"DRONE {health.CurrentHealth:0}/{health.MaxHealth:0}", labelStyle);

            if (lastDamage > 0f && IsDamagePopupVisible(Time.time, popupShownAt, popupDuration))
            {
                GUI.color = new Color(1f, 0.3f, 0.2f);
                GUIStyle popupStyle = new GUIStyle(GUI.skin.label)
                {
                    alignment = TextAnchor.MiddleCenter,
                    fontSize = 18,
                    fontStyle = FontStyle.Bold
                };
                GUI.Label(new Rect(x, y - 44f, barWidth, 24f), $"-{Mathf.RoundToInt(lastDamage)}", popupStyle);
            }

            GUI.color = previousColor;
        }

        public static bool IsDamagePopupVisible(float currentTime, float shownAt, float duration)
        {
            return duration > 0f && shownAt >= 0f && currentTime - shownAt <= duration;
        }

        private static Color HealthColor(float ratio)
        {
            if (ratio > 0.6f) return new Color(0.1f, 1f, 0.35f);
            if (ratio > 0.3f) return new Color(1f, 0.8f, 0.15f);
            return new Color(1f, 0.2f, 0.15f);
        }
    }
}
