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

        [SerializeField] PlayerCharacterType m_eCharacterType;

        [Header("Input")]
        [Min(0)]
        [SerializeField] protected float m_fJumpInputHold = 3f;
        [SerializeField] protected bool m_bAirControl;

        [Header("Event")]
        [SerializeField] UnityEvent m_hFlipEvent;

        [Header("Reference")]
        [SerializeField] PlayerAnimatorController m_hAnimController;
        [SerializeField] GameObject m_hStandCheckGroup;
        [SerializeField] GameObject m_hCrouchCheckGroup;
        [SerializeField] GameObject m_hInteractableCanvas;

        #endregion

        #region Variable - Property

        public PlayerStatusController statusController { get { return m_hStatusController; } }
        public PlayerMoveController moveController { get { return m_hMoveController; } }
        public PlayerAnimatorController animatorController { get { return m_hAnimController; } }

        public PlayerCharacterType characterType { get { return m_eCharacterType; } }

        public int playerID { get { return m_nPlayerID; } }
        public bool isGrounded { get { return m_bIsGrounded; } }

        public bool isJumping { get { return m_bIsJumping; } }

        public bool isClimbing { get { return m_bClimbing; } }

        public Vector2 inputAxis { get { return m_vInputAxis; } }

        public ClimbObjectController climbableController { get { return m_hClimbController; } set { m_hClimbController = value; } }

        public float? overrideMoveSpeed { get { return m_fOverrideMoveSpeed; } set { m_fOverrideMoveSpeed = value; } }

        #endregion

        protected Rigidbody2D m_hRigid;
        protected CapsuleCollider2D m_hCol;

        protected PlayerStatusController m_hStatusController;
        protected PlayerMoveController m_hMoveController;

        protected int m_nPlayerID;

        protected float? m_fOverrideMoveSpeed;

        protected bool m_bFacingRight = true;
        protected bool m_bIsGrounded;
        protected bool m_bIsCeiling;
        protected bool m_bIsWallBack;
        protected bool m_bIsWallForward;

        protected bool m_bIsJumping;
        protected float m_fJumpDelayCheck;


        protected float? m_fActionInputStartTime;
        protected float? m_fJumpInputStartTime;


        protected bool m_bCrouching;

        protected float m_fGravityScaleOrigin;
        protected Vector2 m_vCurrentVelocity;

        protected Vector2 m_vColSizeOrigin;
        protected Vector2 m_vColOffsetOrigin;

        protected Vector2 m_vInputAxis;
        protected RaycastHit2D[] m_arrRayHit = new RaycastHit2D[5];

        protected ClimbObjectController m_hClimbController;

        protected bool m_bClimbing;

        protected IInteractable m_hInteractable;
        protected float? m_fLastFrameGetInteractable;

        protected IInteractable m_hCurrentInteraction;
        protected float? m_fEndInteractionDelay;

        protected float? m_fMaxAirPosition;

        protected bool m_bDead;

        protected Vector3 m_vInteractableCanvasOffset;

        protected IMoveablePlatform m_hMoveablePlatform;

        #endregion

        #region Base - Mono

        protected virtual void Awake()
        {
            m_hRigid = GetComponent<Rigidbody2D>();
            m_hCol = GetComponent<CapsuleCollider2D>();

            m_hStatusController = GetComponent<PlayerStatusController>();
            m_hMoveController = GetComponent<PlayerMoveController>();

            m_fGravityScaleOrigin = m_hRigid.gravityScale;
            m_hRigid.gravityScale = 0;

            m_vColSizeOrigin = m_hCol.size;
            m_vColOffsetOrigin = m_hCol.offset;

            if (m_eCharacterType == PlayerCharacterType.Uncle)
            {
                m_nPlayerID = 1;
            }

            m_vInteractableCanvasOffset = m_hInteractableCanvas.transform.position - transform.position;
            m_hInteractableCanvas.transform.SetParent(null);
        }

        protected virtual void OnEnable()
        {
            Global_InputManager.AddInputEventListener(m_nPlayerID, (int)InputButtonType.South, ActionButtonDown, ActionButtonHold, ActionButtonUp);
        }

        protected virtual void OnDisable()
        {
            Global_InputManager.RemoveInputEventListener(m_nPlayerID, (int)InputButtonType.South, ActionButtonDown, ActionButtonHold, ActionButtonUp);
        }

        protected virtual void Update()
        {
            if (m_bDead)
                return;

            m_vInputAxis = Global_InputManager.GetRawAxis(m_nPlayerID, 0);

            JumpDelayCheckCounting();
            CheckClimbInput();

            UpdateInteraction();
            EndInteractionDelayCounting();
        }

        void LateUpdate()
        {
            InteractableCanvasFollowing();
        }

        protected virtual void FixedUpdate()
        {
            if (m_bDead)
                return;

            CheckDeathFromFalling();
            AddGravityVelocity();
            CheckResetVelocityY();
            CheckJumpEnd();
            Crouch();
            Move();
            CheckFlipCharacter();
            UpdateAnimation();
            ClearLastInteractable();

            FixedUpdateInteraction();
        }

        #endregion

        #region Events

        #region Events - Input

        protected virtual void ActionButtonDown()
        {

            JumpInputStart();
            //Jump();

            m_fActionInputStartTime = Time.time;
        }

        protected virtual void ActionButtonHold()
        {
            JumpInputUpdate();
        }

        protected virtual void ActionButtonUp()
        {
            JumpInputEnd();

            CheckInteractionStart();
            m_fActionInputStartTime = null;
        }

        #endregion

        public void SetGround(GameObject hGrounded)
        {
            if (hGrounded == null)
            {
                UnregisterMoveablePlatform();
                return;
            }

            var hMoveablePlatform = hGrounded.GetComponentInParent<IMoveablePlatform>();
            if (hMoveablePlatform == null)
            {
                UnregisterMoveablePlatform();
                return;
            }

            if (hMoveablePlatform == m_hMoveablePlatform)
                return;

            m_hMoveablePlatform = hMoveablePlatform;
            m_hMoveablePlatform.RegisterStandFollow(m_hMoveController);


            #region Method

            void UnregisterMoveablePlatform()
            {
                if (m_hMoveablePlatform != null)
                {
                    m_hMoveablePlatform.UnregisterStandFollow(m_hMoveController);
                }
            }

            #endregion
        }

        public void SetIsGrounded(bool bIsGrounded)
        {
            m_bIsGrounded = bIsGrounded;

            // Temp
            if (IsUncle())
                bIsGrounded = true;

            m_hAnimController.SetGrounded(bIsGrounded);

            if (bIsGrounded && m_bIsJumping && m_fJumpDelayCheck <= 0)
            {
                IsJump(false);
            }

            if (!bIsGrounded)
            {
                if (m_hMoveablePlatform != null)
                {
                    m_hMoveablePlatform.UnregisterStandFollow(m_hMoveController);
                    m_hMoveablePlatform = null;
                }

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

        public void GetInteractableObject(GameObject hGO)
        {
            if (m_hCurrentInteraction != null)
                return;

            var hInteractable = hGO.GetComponent<IInteractable>();
            if (hInteractable != null)
            {
                if (!hInteractable.CanInteraction(this))
                    return;

                m_hInteractable = hInteractable;
                m_hInteractableCanvas.SetActive(true);

                m_fLastFrameGetInteractable = Time.time;
            }
        }

        public void GetHalfGroundInFront(RaycastHit2D hHit)
        {
            if (!m_bIsGrounded || m_bClimbing)
                return;

            if (IsMoveForward() && !m_bIsWallForward)
            {
                Vector2 vPosition = transform.position;
                float fBottomDistance = m_hCol.GetPositionBottom().y;
                float fUpPosition = vPosition.y - fBottomDistance - hHit.distance;
                vPosition.y += fUpPosition;
                transform.position = vPosition;
            }
        }

        #endregion

        #region Base - Main

        protected virtual void Move()
        {
            if (m_bIsGrounded && !m_bClimbing && m_fJumpInputStartTime.HasValue)
                return;

            float fDeltaTime = Time.deltaTime;
            Vector2 vMovePosition = Vector2.zero;
            float m_fMoveHorizontal = m_vInputAxis.x;
            float m_fMoveVertical = m_vInputAxis.y;

            if (m_bClimbing)
                m_fMoveHorizontal = 0;
            else
                m_fMoveVertical = 0;

            if (HasLockFlag(PlayerLockFlag.LockMove))
                m_fMoveHorizontal = 0;
            if (HasLockFlag(PlayerLockFlag.LockClimb))
                m_fMoveVertical = 0;

            if ((m_fMoveHorizontal < 0
                && HasLockFlag(PlayerLockFlag.LockMoveLeft))
                || (m_fMoveHorizontal > 0
                && HasLockFlag(PlayerLockFlag.LockMoveRight)))
                m_fMoveHorizontal = 0;

            //if (!m_bAirControl && !m_bIsGrounded && !m_bCurrentJump)
            //    m_fMoveHorizontal = 0;

            // Add velocity value to move.
            vMovePosition += m_vCurrentVelocity * fDeltaTime;

            float fMoveSpeed = m_hStatusController.moveSpeed;

            if (!m_bIsGrounded)
                fMoveSpeed = m_hStatusController.airMoveSpeed;
            else if (m_bCrouching)
                fMoveSpeed = m_hStatusController.crouchSpeed;

            if (m_fOverrideMoveSpeed.HasValue)
                fMoveSpeed = m_fOverrideMoveSpeed.Value;

            vMovePosition.x += m_fMoveHorizontal * fMoveSpeed * fDeltaTime;
            vMovePosition.y += m_fMoveVertical * m_hStatusController.climbSpeed * fDeltaTime;
            ChangeMoveXIfHasObstacle(ref vMovePosition);

            m_hMoveController.Move(vMovePosition);
            //vMovePosition += m_hRigid.position;
            //m_hRigid.MovePosition(vMovePosition);
        }

        protected virtual void JumpInputStart()
        {
            if (!m_bIsGrounded || m_bClimbing || m_bCrouching)
                return;

            m_fJumpInputStartTime = Time.time;

            if (!IsUncle())
                m_hAnimController.SetJumpReady(true);
        }

        protected virtual void JumpInputEnd()
        {
            m_fJumpInputStartTime = null;
            m_hAnimController.SetJumpReady(false);
        }

        protected virtual void JumpInputUpdate()
        {
            if (!m_fJumpInputStartTime.HasValue)
                return;

            if (!m_bIsGrounded)
            {
                JumpInputEnd();
                return;
            }

            if (Time.time >= m_fJumpInputStartTime + m_fJumpInputHold)
            {
                Jump();
                JumpInputEnd();
            }
        }

        protected virtual void Jump()
        {
            if (IsUncle())
                return;

            if (!m_bIsGrounded || m_bIsCeiling || HasLockFlag(PlayerLockFlag.LockJump))
                return;

            m_vCurrentVelocity.y += m_hStatusController.jumpForce;

            IsJump(true);
            m_fJumpDelayCheck = 0.2f;
        }

        protected virtual void JumpDelayCheckCounting()
        {
            if (m_fJumpDelayCheck <= 0)
                return;

            m_fJumpDelayCheck -= Time.deltaTime;
            if (m_fJumpDelayCheck < 0)
                m_fJumpDelayCheck = 0;
        }

        protected virtual void Crouch()
        {
            if (IsUncle())
                return;

            bool bCrouching = m_bIsGrounded && m_vInputAxis.y < 0;
            if (m_bClimbing || m_fJumpInputStartTime.HasValue
                || HasLockFlag(PlayerLockFlag.LockCrouch))
                bCrouching = false;

            if ((m_bCrouching == bCrouching) || (m_bCrouching && m_bIsCeiling))
                return;

            m_bCrouching = bCrouching;
            m_hAnimController.SetCrouch(m_bCrouching);

            if (m_bCrouching)
            {
                Vector2 vSize = m_vColSizeOrigin;
                vSize.y *= 0.6f;
                m_hCol.size = vSize;

                Vector2 vOffset = m_vColOffsetOrigin;
                vOffset.y += (vSize.y - m_vColSizeOrigin.y) * 0.5f;
                m_hCol.offset = vOffset;

                m_hStandCheckGroup.SetActive(false);
                m_hCrouchCheckGroup.SetActive(true);
            }
            else
            {
                m_hCol.size = m_vColSizeOrigin;
                m_hCol.offset = m_vColOffsetOrigin;

                m_hStandCheckGroup.SetActive(true);
                m_hCrouchCheckGroup.SetActive(false);
            }
        }

        protected virtual void CheckClimbInput()
        {
            if (climbableController == null)
                return;

            Vector2 vClimbCenter = climbableController.GetCenterPosition();
            float xDistance = Mathf.Abs(vClimbCenter.x - transform.position.x);
            if (xDistance > 0.2f)
                return;

            bool bClimbing = m_bClimbing;
            bool bBelowCenter = climbableController.IsBelowCenter(transform.position);
            float fPlayerBottomPosition = m_hCol.GetPositionBottom().y;
            float fTopPosition = climbableController.GetTopPosition().y;
            bool bOverTop = fPlayerBottomPosition >= fTopPosition - 0.1f;

            if ((m_vInputAxis.y > 0 && !bOverTop)
                || (m_vInputAxis.y < 0 && !bBelowCenter && bOverTop))
            {
                bClimbing = true;
            }
            else if ((m_vInputAxis.y < 0 && bBelowCenter && m_bIsGrounded)
                || (!bBelowCenter && fPlayerBottomPosition >= fTopPosition - 0.1f))
            {
                bClimbing = false;
            }

            if (m_vInputAxis.y == 0 && m_vInputAxis.x != 0)
            {
                bClimbing = false;
            }

            if (bClimbing != m_bClimbing)
            {
                if (bClimbing)
                    ClimbStart();
                else
                    ClimbEnd();
            }
        }

        protected virtual void ClimbStart()
        {
            m_bClimbing = true;
            m_hAnimController.SetClimb(true);

            Vector2 vPosition = transform.position;
            vPosition.x = climbableController.GetCenterPosition().x;
            transform.position = vPosition;
            m_hMoveController.StopMove();

            gameObject.layer = LayerMask.NameToLayer(LayerName.climb);
        }

        protected virtual void ClimbEnd()
        {
            m_bClimbing = false;
            m_hAnimController.SetClimb(false);

            gameObject.layer = LayerMask.NameToLayer(LayerName.player);
        }

        protected void UpdateAnimation()
        {
            bool bWalk = m_vInputAxis.x != 0;
            if (m_fJumpInputStartTime.HasValue
                || HasLockFlag(PlayerLockFlag.LockMove))
                bWalk = false;

            m_hAnimController.SetWalk(bWalk);
        }

        protected void CheckInteractionStart()
        {
            if (m_hInteractable == null || m_fEndInteractionDelay.HasValue)
                return;

            if (m_fActionInputStartTime.HasValue && Time.time <= m_fActionInputStartTime + 0.2f)
            {
                m_hCurrentInteraction = m_hInteractable;
                m_hCurrentInteraction.StartInteraction(this);
            }
        }

        protected void UpdateInteraction()
        {
            if (m_hCurrentInteraction == null)
                return;

            m_hCurrentInteraction.UpdateInteraction(this);
        }

        protected void FixedUpdateInteraction()
        {
            if (m_hCurrentInteraction == null)
                return;

            m_hCurrentInteraction.FixedUpdateInteraction(this);
        }

        public void EndInteraction()
        {
            if (m_hCurrentInteraction == null)
                return;

            m_hCurrentInteraction.EndInteraction(this);
            m_hCurrentInteraction = null;

            m_fEndInteractionDelay = Time.time + 0.2f;
        }

        protected void EndInteractionDelayCounting()
        {
            if (!m_fEndInteractionDelay.HasValue)
                return;

            if (Time.time >= m_fEndInteractionDelay)
            {
                m_fEndInteractionDelay = null;
            }
        }

        protected void CheckDeathFromFalling()
        {
            if (IsUncle())
                return;

            if (m_bIsGrounded)
            {
                if (m_fMaxAirPosition.HasValue)
                {
                    float fFallDistance = m_fMaxAirPosition.Value - transform.position.y;
                    m_fMaxAirPosition = null;

                    if (fFallDistance >= 3f)
                    {
                        Death();
                    }
                }
                return;
            }

            float fPositionY = transform.position.y;
            if (!m_fMaxAirPosition.HasValue
                || (m_fMaxAirPosition < fPositionY))
                m_fMaxAirPosition = fPositionY;
        }

        protected virtual void Death()
        {
            m_bDead = true;
            m_hAnimController.TriggerHurt();
            Global_GameplayManager.GameOver();
        }

        protected void InteractableCanvasFollowing()
        {
            m_hInteractableCanvas.transform.position = transform.position + m_vInteractableCanvasOffset;
        }

        #endregion

        #region Helper

        protected void AddGravityVelocity()
        {
            if (m_bIsGrounded || m_bClimbing)
                return;

            m_vCurrentVelocity += Physics2D.gravity * m_fGravityScaleOrigin * Time.deltaTime;
        }

        protected void CheckResetVelocityY()
        {
            if ((m_bIsGrounded && m_vCurrentVelocity.y < 0)
                || (m_bIsCeiling && m_vCurrentVelocity.y > 0)
                || m_bClimbing)
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
            if (HasLockFlag(PlayerLockFlag.LockFlip))
                return;

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

        protected bool IsUncle()
        {
            return m_eCharacterType == PlayerCharacterType.Uncle;
        }

        protected void ClearLastInteractable()
        {
            if (m_hInteractable == null || !m_fLastFrameGetInteractable.HasValue)
                return;

            if (Time.time <= m_fLastFrameGetInteractable + 0.04f)
                return;

            m_fLastFrameGetInteractable = null;
            m_hInteractableCanvas.SetActive(false);
            m_hInteractable = null;
        }

        protected bool IsMoveForward()
        {
            bool bResult = false;
            if (m_vInputAxis.x != 0)
            {
                bResult = (m_vInputAxis.x > 0 && m_bFacingRight)
                            || (m_vInputAxis.x < 0 && !m_bFacingRight);
            }

            return bResult;
        }

        protected bool HasLockFlag(PlayerLockFlag eLockFlag)
        {
            return FlagUtility.HasFlagUnsafe(m_hStatusController.lockFlag, eLockFlag);
        }

        #endregion
    }
}
