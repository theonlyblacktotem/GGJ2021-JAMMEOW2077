using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DSC.Core;
using DSC.Input;
using UnityEngine.Events;

namespace NTTMS.Test
{
    public class PlayerController : MonoBehaviour
    {
        #region Variable

        #region Variable - Inspector

        [Min(0)]
        [SerializeField] protected float m_fMoveSpeed = 5.2f;
        [Min(0)]
        [SerializeField] protected float m_fCrouchSpeed = 2.6f;
        [Min(0)]
        [SerializeField] protected float m_fJumpForce = 5f;

        [Min(0)]
        [SerializeField] protected float m_fJumpInputHold = 3f;

        [Header("Event")]
        [SerializeField] UnityEvent m_hFlipEvent;

        [Header("Reference")]
        [SerializeField] PlayerAnimatorController m_hAnimController;

        #endregion

        #region Variable - Property

        public bool isGrounded { get { return m_bIsGrounded; } }

        public bool isJumping { get { return m_bIsJumping; } }

        #endregion

        protected Rigidbody2D m_hRigid;
        protected CapsuleCollider2D m_hCol;

        protected bool m_bFacingRight = true;
        protected bool m_bIsGrounded;
        protected bool m_bIsCeiling;
        protected bool m_bIsWallBack;
        protected bool m_bIsWallForward;

        protected bool m_bIsJumping;
        protected float m_fJumpDelayCheck;

        protected float? m_fJumpInputStartTime;


        protected bool m_bCrouching;

        protected float m_fMoveHorizontal;
        protected float m_fGravityScaleOrigin;
        protected Vector2 m_vCurrentVelocity;

        protected Vector2 m_vInputAxis;
        protected RaycastHit2D[] m_arrRayHit = new RaycastHit2D[5];

        #endregion

        #region Base - Mono

        protected virtual void Awake()
        {
            m_hRigid = GetComponent<Rigidbody2D>();
            m_hCol = GetComponent<CapsuleCollider2D>();
            m_fGravityScaleOrigin = m_hRigid.gravityScale;
            m_hRigid.gravityScale = 0;
        }

        protected virtual void OnEnable()
        {
            Global_InputManager.AddInputEventListener((int)InputButtonType.South, ActionButtonDown, ActionButtonHold, ActionButtonUp);
        }

        protected virtual void OnDisable()
        {
            Global_InputManager.RemoveInputEventListener((int)InputButtonType.South, ActionButtonDown, ActionButtonHold, ActionButtonUp);
        }

        protected virtual void Update()
        {
            m_vInputAxis = Global_InputManager.GetRawAxis();

            JumpDelayCheckCounting();
        }

        protected virtual void FixedUpdate()
        {
            AddGravityVelocity();
            CheckResetVelocityY();
            CheckJumpEnd();
            Crouch();
            Move();
            CheckFlipCharacter();
            UpdateAnimation();
        }

        #endregion

        #region Events

        #region Events - Input

        protected virtual void ActionButtonDown()
        {
            //JumpInputStart();
            Jump();
        }

        protected virtual void ActionButtonHold()
        {

        }

        protected virtual void ActionButtonUp()
        {
            //JumpInputEnd();
        }

        #endregion

        public void SetIsGrounded(bool bIsGrounded)
        {
            m_bIsGrounded = bIsGrounded;
            m_hAnimController.SetGrounded(bIsGrounded);

            if (bIsGrounded && m_bIsJumping && m_fJumpDelayCheck <= 0)
            {
                IsJump(false);
            }
        }

        public void SetIsCeiling(bool bIsCeiling)
        {
            m_bIsCeiling = bIsCeiling;
        }

        public void SetIsWallForward(bool bIsWall)
        {
            m_bIsWallForward = bIsWall;
        }

        public void SetIsWallBack(bool bIsWall)
        {
            m_bIsWallBack = bIsWall;
        }


        #endregion

        #region Base - Main

        protected virtual void Move()
        {
            Vector2 vMovePosition = Vector2.zero;
            m_fMoveHorizontal = m_vInputAxis.x;

            // Add velocity value to move.
            vMovePosition += m_vCurrentVelocity * Time.deltaTime;

            float fMoveSpeed = m_bCrouching ? m_fCrouchSpeed : m_fMoveSpeed;
            vMovePosition.x += m_fMoveHorizontal * fMoveSpeed * Time.deltaTime;
            ChangeMoveXIfHasObstacle(ref vMovePosition);

            vMovePosition += m_hRigid.position;
            m_hRigid.MovePosition(vMovePosition);
        }

        protected void JumpInputStart()
        {
            m_fJumpInputStartTime = Time.time;
        }

        protected void JumpInputEnd()
        {
            m_fJumpInputStartTime = null;
        }

        protected void Jump()
        {
            if (!m_bIsGrounded)
                return;

            m_vCurrentVelocity.y += m_fJumpForce;

            IsJump(true);
            m_fJumpDelayCheck = 0.2f;
        }

        protected void JumpDelayCheckCounting()
        {
            if (m_fJumpDelayCheck <= 0)
                return;

            m_fJumpDelayCheck -= Time.deltaTime;
            if (m_fJumpDelayCheck < 0)
                m_fJumpDelayCheck = 0;
        }

        protected void Crouch()
        {
            m_bCrouching = m_bIsGrounded && m_vInputAxis.y < 0;
            m_hAnimController.SetCrouch(m_bCrouching);
        }

        protected void UpdateAnimation()
        {
            m_hAnimController.SetWalk(m_vInputAxis.x != 0);
        }

        #endregion

        #region Helper

        protected void AddGravityVelocity()
        {
            if (m_bIsGrounded)
                return;

            m_vCurrentVelocity += Physics2D.gravity * m_fGravityScaleOrigin * Time.deltaTime;
        }

        protected void CheckResetVelocityY()
        {
            if ((m_bIsGrounded && m_vCurrentVelocity.y < 0)
                || m_bIsCeiling && m_vCurrentVelocity.y > 0)
                m_vCurrentVelocity.y = 0;
        }

        protected void ChangeMoveXIfHasObstacle(ref Vector2 vMove)
        {
            if (vMove.x == 0)
                return;

            bool bMovingForward = (vMove.x > 0) == m_bFacingRight;
            if ((bMovingForward && m_bIsWallForward)
                || (!bMovingForward && m_bIsWallBack))
            {
                vMove.x = 0;
            }
        }

        protected void CheckFlipCharacter()
        {
            if ((m_vInputAxis.x > 0 && !m_bFacingRight)
                || m_vInputAxis.x < 0 && m_bFacingRight)
                FlipCharacter();
        }

        protected void FlipCharacter()
        {
            m_bFacingRight = !m_bFacingRight;
            transform.Rotate(new Vector3(0, 180, 0));
            m_hFlipEvent?.Invoke();
        }

        protected void CheckJumpEnd()
        {
            if (!m_bIsJumping)
                return;

            if (m_vCurrentVelocity.y <= 0)
            {
                IsJump(false);
            }
        }

        protected void IsJump(bool bJump)
        {
            m_bIsJumping = bJump;
            m_hAnimController.SetJump(bJump);
        }

        #endregion
    }
}
