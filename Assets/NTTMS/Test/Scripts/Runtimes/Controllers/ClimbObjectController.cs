using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NTTMS.Test
{
    public class ClimbObjectController : MonoBehaviour, IInteractable
    {
        #region Variable



        #endregion

        #region Interface

        public bool CanInteraction()
        {
            return false;
        }

        public void StartInteraction()
        {

        }

        public void UpdateInteraction()
        {

        }

        public void EndInteraction()
        {

        }


        #endregion

        #region Base - Main

        

        #endregion
    }
}
