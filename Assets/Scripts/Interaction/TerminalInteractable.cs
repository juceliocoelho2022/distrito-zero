using UnityEngine;
using DistritoZero.Challenges;
using DistritoZero.Missions;

namespace DistritoZero.Interaction
{
    public class TerminalInteractable : MonoBehaviour, IInteractable
    {
        private bool activated;
        public bool CanInteract => !activated && ChallengeManager.Instance != null;
        public string Prompt => "[E] Acessar terminal";

        public void Interact()
        {
            if (!CanInteract) return;
            activated = true;
            MissionManager.Instance?.TerminalActivated();
            ChallengeManager.Instance.StartDefaultChallenge();
        }
    }
}
