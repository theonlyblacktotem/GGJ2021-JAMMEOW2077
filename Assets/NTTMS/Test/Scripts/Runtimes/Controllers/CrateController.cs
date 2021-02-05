using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace NTTMS.Test
{
    public class CrateController : MonoBehaviour, IInteractable
    {
        #region Variable

        #region Variable - Inspector

        [SerializeField] InteractableUIController m_hUIController;

        #endregion

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

        }

        public void UpdateInteraction(PlayerController hPlayer)
        {

        }

        public void EndInteraction(PlayerController hPlayer)
        {

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
