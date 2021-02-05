namespace NTTMS.Test
{
    public interface IInteractable
    {
        bool CanInteraction(PlayerController hPlayer);
        void StartInteraction(PlayerController hPlayer);
        void UpdateInteraction(PlayerController hPlayer);
        void EndInteraction(PlayerController hPlayer);
        void ShowInteractionUI(PlayerController hPlayer,bool bShow);
    }
}
