using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DSC.Core;

namespace NTTMS.Test
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class CrateController : MonoBehaviour, IInteractable
    {
        #region Variable

        #region Variable - Inspector

        [Min(0)]
        [SerializeField] float m_fPushSpeed = 2.6f;

        [SerializeField] GameObject m_hBlockPlayerPush;

        #endregion

        Rigidbody2D m_hRigid;

        PlayerFlag m_eLockFlag = PlayerFlag.LockMoveLeft | PlayerFlag.LockMoveRight
                                | PlayerFlag.LockJump | PlayerFlag.LockCrouch
                                | PlayerFlag.LockClimb | PlayerFlag.LockFlip;

        #endregion

        #region Base - Mono

        void Awake()
        {
            m_hRigid = GetComponent<Rigidbody2D>();
        }

        #endregion

        #region Interface

        public bool CanInteraction(PlayerController hPlayer)
        {
            if (hPlayer == null)
                return false;
            
            float fDistanceX = Mathf.Abs(hPlayer.transform.position.x - transform.position.x);     
            return fDistanceX < 0.75f;
        }

        public void StartInteraction(PlayerController hPlayer)
        {
            if (hPlayer == null)
                return;

            hPlayer.playerFlag |= m_eLockFlag;
            hPlayer.animatorController.SetPush(true);
            hPlayer.overrideMoveSpeed = m_fPushSpeed;

            gameObject.layer = LayerMask.NameToLayer(LayerName.defaultLayer);
            m_hBlockPlayerPush.SetActive(false);

            bool bPlayerOnRight = transform.position.x - hPlayer.transform.position.x < 0;
            hPlayer.playerFlag &= bPlayerOnRight ? ~PlayerFlag.LockMoveLeft : ~PlayerFlag.LockMoveRight;
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
            Vector2 vInputAxis = hPlayer.inputAxis;
            if (vInputAxis == Vector2.zero)
                return;
        }

        public void EndInteraction(PlayerController hPlayer)
        {
            hPlayer.playerFlag &= ~m_eLockFlag;
            hPlayer.animatorController.SetPush(false);
            hPlayer.overrideMoveSpeed = null;

            gameObject.layer = LayerMask.NameToLayer(LayerName.notPlayerCollision);
            m_hBlockPlayerPush.SetActive(true);
        }

        #endregion
    }
}
