namespace NTTMS.Test
{
    public interface IInteractable
    {
        bool CanInteraction();
        void StartInteraction();
        void UpdateInteraction();
        void EndInteraction();
    }
}
