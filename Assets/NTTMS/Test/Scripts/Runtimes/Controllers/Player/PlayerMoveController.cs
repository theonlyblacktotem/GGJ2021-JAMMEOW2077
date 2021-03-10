using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NTTMS.Test
{
    public class PlayerMoveController : MonoBehaviour, IMoveable
    {
        #region Variable

        #region Variable - Inspector



        #endregion

        #region Variable - Property

        #endregion

        protected Rigidbody2D m_hRigid;

        protected Vector2 m_vMove;

        protected bool m_bStopMove;

        #endregion

        #region Mono

        protected virtual void Awake()
        {
            m_hRigid = GetComponent<Rigidbody2D>();
        }

        protected virtual void FixedUpdate()
        {
            UpdateMove();
        }

        protected virtual void LateUpdate()
        {
            m_bStopMove = false;
        }

        #endregion

        #region Interface

        public virtual void Move(Vector2 vMove)
        {
            if (m_bStopMove)
                return;
            
            m_vMove += vMove;
        }

        public virtual void StopMove()
        {
            m_bStopMove = true;
            m_vMove = Vector2.zero;
        }


        public virtual void Warp(Vector2 vWarpPosition)
        {

        }

        #endregion

        #region Main

        protected virtual void UpdateMove()
        {
            if (m_vMove == Vector2.zero)
                return;

            m_hRigid.MovePosition(m_hRigid.position + m_vMove);
            m_vMove = Vector2.zero;
        }

        #endregion
    }
}
