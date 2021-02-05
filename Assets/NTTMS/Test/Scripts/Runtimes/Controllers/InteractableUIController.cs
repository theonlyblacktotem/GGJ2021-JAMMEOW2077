using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace NTTMS.Test
{
    public class InteractableUIController : MonoBehaviour
    {
        #region Variable

        #region Variable - Inspector

        [SerializeField] GameObject m_hChildUI;
        [SerializeField] GameObject m_hUncleUI;

        #endregion

        #endregion

        #region Base - Mono

        void Awake()
        {
            HideAllUI();
        }

        #endregion

        #region Base - Main

        public void ShowUI(PlayerCharacterType eCharacter, bool bShow)
        {
            if (!bShow)
            {
                HideAllUI();
            }
            else
            {
                bool bChildShow = eCharacter == PlayerCharacterType.Child;
                m_hChildUI.SetActive(bChildShow);
                m_hUncleUI.SetActive(!bChildShow);
            }
        }

        #endregion

        #region Helper

        protected void HideAllUI()
        {
            m_hChildUI.SetActive(false);
            m_hUncleUI.SetActive(false);
        }

        #endregion
    }
}
