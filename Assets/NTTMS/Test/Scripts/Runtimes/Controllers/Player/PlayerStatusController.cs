using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NTTMS.Test
{
    public class PlayerStatusController : MonoBehaviour
    {
        #region Variable

        #region Variable - Inspector

        [Header("Status")]
        [Min(0)]
        [SerializeField] protected float m_fMoveSpeed = 5.2f;
        [Min(0)]
        [SerializeField] protected float m_fAirMoveSpeed = 3f;
        [Min(0)]
        [SerializeField] protected float m_fCrouchSpeed = 2.6f;
        [Min(0)]
        [SerializeField] protected float m_fClimbSpeed = 2.6f;
        [Min(0)]
        [SerializeField] protected float m_fJumpForce = 5.5f;

        #endregion

        #region Variable - Property

        public float moveSpeed { get { return m_fMoveSpeed; } set { m_fMoveSpeed = value; } }
        public float airMoveSpeed { get { return m_fAirMoveSpeed; } set { m_fAirMoveSpeed = value; } }
        public float crouchSpeed { get { return m_fCrouchSpeed; } set { m_fCrouchSpeed = value; } }
        public float climbSpeed { get { return m_fClimbSpeed; } set { m_fClimbSpeed = value; } }
        public float jumpForce { get { return m_fJumpForce; } set { m_fJumpForce = value; } }

        public PlayerLockFlag lockFlag { get { return m_eLockFlag; } set { m_eLockFlag = value; } }

        #endregion

        protected PlayerLockFlag m_eLockFlag;

        #endregion
    }
}
