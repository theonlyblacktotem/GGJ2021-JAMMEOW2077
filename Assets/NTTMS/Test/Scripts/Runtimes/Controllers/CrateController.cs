using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DSC.Core;

namespace NTTMS.Test
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class CrateController : MonoBehaviour, IInteractable, IMoveablePlatform
    {
        #region Variable

        #region Variable - Inspector

        [Min(0)]
        [SerializeField] float m_fPushSpeed = 2.6f;

        [SerializeField] GameObject m_hBlockPlayerPush;

        #endregion

        Rigidbody2D m_hRigid;

        PlayerLockFlag m_eLockFlag = PlayerLockFlag.LockMoveLeft | PlayerLockFlag.LockMoveRight
                                | PlayerLockFlag.LockJump | PlayerLockFlag.LockCrouch
                                | PlayerLockFlag.LockClimb | PlayerLockFlag.LockFlip;

        PlayerController m_hCurrentPlayer;

        List<IMoveable> m_lstStandFollow = new List<IMoveable>();

        Vector3 m_vLastPosition;
        Vector3 m_vLastMove;

        #endregion

        #region Mono

        void Awake()
        {
            m_hRigid = GetComponent<Rigidbody2D>();

            m_vLastPosition = transform.position;
        }

        void FixedUpdate()
        {
            m_vLastMove = transform.position - m_vLastPosition;
            m_vLastPosition = transform.position;            
            UpdateStandFollow();
        }

        #endregion

        #region Interface

        public bool CanInteraction(PlayerController hPlayer)
        {
            if (hPlayer == null || m_hCurrentPlayer)
                return false;

            float fDistanceX = Mathf.Abs(hPlayer.transform.position.x - transform.position.x);
            return fDistanceX < 0.75f;
        }

        public void StartInteraction(PlayerController hPlayer)
        {
            if (hPlayer == null || (m_hCurrentPlayer != null && m_hCurrentPlayer != hPlayer))
                return;

            m_hCurrentPlayer = hPlayer;

            hPlayer.statusController.lockFlag |= m_eLockFlag;
            hPlayer.animatorController.SetPush(true);
            hPlayer.overrideMoveSpeed = m_fPushSpeed;

            hPlayer.gameObject.layer = LayerMask.NameToLayer(LayerName.playerPush);
            //gameObject.layer = LayerMask.NameToLayer(LayerName.defaultLayer);
            //m_hBlockPlayerPush.SetActive(false);

            bool bPlayerOnRight = transform.position.x - hPlayer.transform.position.x < 0;
            hPlayer.statusController.lockFlag &= bPlayerOnRight ? ~PlayerLockFlag.LockMoveLeft : ~PlayerLockFlag.LockMoveRight;
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
            m_hCurrentPlayer = null;

            hPlayer.statusController.lockFlag &= ~m_eLockFlag;
            hPlayer.animatorController.SetPush(false);
            hPlayer.overrideMoveSpeed = null;

            hPlayer.gameObject.layer = LayerMask.NameToLayer(LayerName.player);
            //gameObject.layer = LayerMask.NameToLayer(LayerName.notPlayerCollision);
            //m_hBlockPlayerPush.SetActive(true);
        }

        public void RegisterStandFollow(IMoveable hMoveable)
        {
            if (hMoveable == null)
                return;

            m_lstStandFollow.Add(hMoveable);
        }

        public void UnregisterStandFollow(IMoveable hMoveable)
        {
            m_lstStandFollow.Remove(hMoveable);
        }

        #endregion

        #region Main

        void UpdateStandFollow()
        {
            if (m_lstStandFollow.Count <= 0)
                return;

            foreach (var hFollow in m_lstStandFollow)
            {
                if (hFollow == null)
                    continue;

                hFollow.Move(m_vLastMove);
            }
        }

        #endregion
    }
}
