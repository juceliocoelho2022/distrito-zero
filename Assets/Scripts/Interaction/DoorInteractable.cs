using UnityEngine;
using DistritoZero.Core;

namespace DistritoZero.Interaction
{
    public class DoorInteractable : MonoBehaviour, IInteractable
    {
        public bool IsUnlocked { get; private set; }
        public bool IsOpen { get; private set; }
        public bool CanInteract => IsUnlocked && !IsOpen;
        public string Prompt => IsUnlocked ? "[E] Abrir porta" : "Porta sem energia";

        private void OnEnable() => GameEvents.ChallengeCompleted += Unlock;
        private void OnDisable() => GameEvents.ChallengeCompleted -= Unlock;

        public void Unlock() => IsUnlocked = true;
        public void Interact()
        {
            if (!CanInteract) return;
            IsOpen = true;
            transform.position += Vector3.up * 3.5f;
        }
    }
}
