using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NTTMS.Test
{
    [RequireComponent(typeof(BoxCollider2D))]
    public class ClimbObjectController : MonoBehaviour, IInteractable
    {
        #region Variable

        BoxCollider2D m_hCol;

        #endregion

        #region Base - Mono

        void Awake()
        {
            m_hCol = GetComponent<BoxCollider2D>();
        }

        void OnTriggerEnter2D(Collider2D other)
        {
            var hPlayer = other.GetComponent<PlayerController>();
            if (hPlayer)
            {
                hPlayer.climbableController = this;
            }
        }

        void OnTriggerExit2D(Collider2D other)
        {
            var hPlayer = other.GetComponent<PlayerController>();
            if (hPlayer)
            {
                if (hPlayer.climbableController == this && !hPlayer.isClimbing)
                {
                    hPlayer.climbableController = null;
                }
            }
        }

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

        public Vector2 GetCenterPosition()
        {
            Vector2 vResult = (Vector2)transform.position + m_hCol.offset;
            return vResult;
        }

        public Vector2 GetTopPosition()
        {
            Vector2 vResult = (Vector2)transform.position + m_hCol.offset;
            vResult.y += m_hCol.size.y * 0.5f;
            return vResult;
        }

        public bool IsBelowCenter(Vector2 vPosition)
        {
            return vPosition.y < GetCenterPosition().y;
        }

        #endregion
    }
}
