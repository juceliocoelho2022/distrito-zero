namespace DistritoZero.Interaction
{
    public interface IInteractable
    {
        bool CanInteract { get; }
        string Prompt { get; }
        void Interact();
    }
}
