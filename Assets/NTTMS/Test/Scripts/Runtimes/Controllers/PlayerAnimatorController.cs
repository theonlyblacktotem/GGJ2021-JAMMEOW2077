using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NTTMS.Test
{
    [RequireComponent(typeof(Animator))]
    public class PlayerAnimatorController : MonoBehaviour
    {
        #region Variable

        protected Animator m_hAnim;

        #endregion

        #region Base - Mono

        void Awake()
        {
            m_hAnim = GetComponent<Animator>();
        }

        #endregion

        #region Base - Main

        public void SetWalk(bool bWalk)
        {
            m_hAnim.SetBool("Walking", bWalk);
        }

        public void SetJump(bool bJump)
        {
            m_hAnim.SetBool("Jumping", bJump);
        }

        public void SetGrounded(bool bGrounded)
        {
            m_hAnim.SetBool("Grounded", bGrounded);
        }

        public void SetCrouch(bool bCrouch)
        {
            m_hAnim.SetBool("Crouching", bCrouch);
        }

        public void SetClimb(bool bClimb)
        {
            m_hAnim.SetBool("Climbing", bClimb);
        }

        #endregion
    }
}
