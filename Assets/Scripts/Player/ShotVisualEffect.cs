using UnityEngine;

namespace DistritoZero.Player
{
    public sealed class ShotVisualEffect : MonoBehaviour
    {
        private const float BeamDuration = 0.08f;
        private const float ImpactDuration = 0.12f;
        private float createdAt;
        private float lifetime;

        public static bool IsVisible(float elapsed, float duration)
        {
            return elapsed >= 0f && elapsed <= duration;
        }

        public static bool ShouldShowImpact(bool hitDamageable)
        {
            return hitDamageable;
        }

        public static void Spawn(Vector3 start, Vector3 end, bool hitDamageable)
        {
            CreateBeam(start, end, hitDamageable);

            if (ShouldShowImpact(hitDamageable))
                CreateImpact(end);
        }

        private static void CreateBeam(Vector3 start, Vector3 end, bool hitDamageable)
        {
            var go = new GameObject("ShotBeamVFX");
            var effect = go.AddComponent<ShotVisualEffect>();
            effect.createdAt = Time.time;
            effect.lifetime = BeamDuration;

            var line = go.AddComponent<LineRenderer>();
            line.useWorldSpace = true;
            line.positionCount = 2;
            line.SetPosition(0, start);
            line.SetPosition(1, end);
            line.startWidth = 0.055f;
            line.endWidth = 0.025f;
            line.numCapVertices = 4;

            Color color = hitDamageable
                ? new Color(1f, 0.15f, 0.1f)
                : new Color(0.1f, 0.9f, 1f);

            line.startColor = color;
            line.endColor = color;
            line.material = CreateMaterial(color);
        }

        private static void CreateImpact(Vector3 position)
        {
            var impact = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            impact.name = "ShotImpactVFX";
            impact.transform.position = position;
            impact.transform.localScale = Vector3.one * 0.18f;

            var collider = impact.GetComponent<Collider>();
            if (collider != null) Destroy(collider);

            Color color = new Color(1f, 0.25f, 0.12f);
            var renderer = impact.GetComponent<Renderer>();
            if (renderer != null) renderer.material = CreateMaterial(color);

            var effect = impact.AddComponent<ShotVisualEffect>();
            effect.createdAt = Time.time;
            effect.lifetime = ImpactDuration;
        }

        private static Material CreateMaterial(Color color)
        {
            Shader shader = Shader.Find("Sprites/Default");
            if (shader == null) shader = Shader.Find("Unlit/Color");
            if (shader == null) shader = Shader.Find("Standard");

            var material = new Material(shader);
            material.color = color;
            return material;
        }

        private void Update()
        {
            if (!IsVisible(Time.time - createdAt, lifetime))
                Destroy(gameObject);
        }
    }
}
