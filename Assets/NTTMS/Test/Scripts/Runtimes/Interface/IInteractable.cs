namespace NTTMS.Test
{
    public interface IInteractable
    {
        bool CanInteraction(PlayerController hPlayer);
        void StartInteraction(PlayerController hPlayer);
        void UpdateInteraction(PlayerController hPlayer);
        void FixedUpdateInteraction(PlayerController hPlayer);
        void EndInteraction(PlayerController hPlayer);
    }
}
