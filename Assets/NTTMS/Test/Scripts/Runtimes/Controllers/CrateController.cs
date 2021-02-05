using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DSC.Core;

namespace NTTMS.Test
{
    public class CrateController : MonoBehaviour, IInteractable
    {
        #region Variable

        #region Variable - Inspector

        [SerializeField] InteractableUIController m_hUIController;

        #endregion

        PlayerFlag m_eLockFlag = PlayerFlag.LockMove | PlayerFlag.LockJump
                                | PlayerFlag.LockCrouch | PlayerFlag.LockClimb
                                | PlayerFlag.LockFlip;

        #endregion

        #region Base - Mono


        #endregion

        #region Interface

        public bool CanInteraction(PlayerController hPlayer)
        {
            return true;
        }

        public void StartInteraction(PlayerController hPlayer)
        {
            if (hPlayer == null)
                return;

            hPlayer.EndInteraction();
            return;
            hPlayer.playerFlag |= m_eLockFlag;
            hPlayer.animatorController.SetPush(true);
        }

        public void UpdateInteraction(PlayerController hPlayer)
        {

            if (Global_InputManager.GetButtonDown(hPlayer.playerID, (int)InputButtonType.South))
            {
                hPlayer.EndInteraction();
            }
        }

        public void FixedUpdateInteraction(PlayerController hPlayer)
        {

        }

        public void EndInteraction(PlayerController hPlayer)
        {
            //hPlayer.playerFlag &= ~m_eLockFlag;
            //hPlayer.animatorController.SetPush(false);
        }

        public void ShowInteractionUI(PlayerController hPlayer, bool bShow)
        {
            if (hPlayer == null)
                return;

            m_hUIController.ShowUI(hPlayer.characterType, bShow);
        }

        #endregion
    }
}
