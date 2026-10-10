using UnityEngine;

namespace DistritoZero.Interaction
{
    public class InteractionDetector : MonoBehaviour
    {
        [SerializeField] private float radius = 2.2f;
        public string CurrentPrompt { get; private set; } = string.Empty;

        private void Update()
        {
            IInteractable closest = null;
            float best = float.MaxValue;
            foreach (var hit in Physics.OverlapSphere(transform.position, radius))
            {
                IInteractable item = null;
                foreach (var behaviour in hit.GetComponentsInParent<MonoBehaviour>())
                {
                    if (behaviour is IInteractable interactable) { item = interactable; break; }
                }
                if (item == null || !item.CanInteract) continue;
                float d = Vector3.SqrMagnitude(hit.transform.position - transform.position);
                if (d < best) { best = d; closest = item; }
            }
            CurrentPrompt = closest?.Prompt ?? string.Empty;
            if (closest != null && Input.GetKeyDown(KeyCode.E)) closest.Interact();
        }
    }
}
