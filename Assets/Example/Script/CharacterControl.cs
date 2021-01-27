using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Example
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class CharacterControl : RaycastCtrl
    {
        public enum characterSide { purple1, green2, yellow3, red4, neutral5 };
        public characterSide e_charSide;

        public Rigidbody2D rb2d;
        //This maybe remove in the future
        public SpriteRenderer spRenderer;

        public Vector3 tr_OriginTrans;
        public Vector3 tr_OriginTrans_play;

        [Header("Player Status")]
        [Range(1, 50)] [SerializeField] public float speed = 20;
        [Range(1, 200)] [SerializeField] float jump = 800;
        [Range(1, 5)] [SerializeField] private int numberjump = 1; //for double jump

        private int i_countJump = 0;
        public GameObject go_hitBox;
        public bool isMove = true;
        public bool isGuard = false;
        public bool isGuardAll = false;
        public bool b_isAttack = false;
        public bool isGround = true;

        public bool b_isSpellingPhase;

        public Transform tr_grabPosition;

        public Animator animator;

        [Header("Hit Box")]
        public Rect boxCastSize = new Rect(0, -0.65f, 0.3f, 0.14f);
        public Rect boxCastSizeEx = new Rect(0, -0.65f, 1.43f, 0.14f);
        public Rect boxCastSizeFront = new Rect(.47f, -.04f, .48f, .59f);
        RaycastHit2D[] ra2_smashGroundAll;

        RaycastHit2D ra2_hitFront;

        public CollisionInfo collisions;

        [Header("Item")]
        public float[] itemDuration = new float[7];
        public bool[] gotItem = new bool[7];

        [Header("Platform Control")]
        public float moveDirection = 1; // 1 = move right | -1 = move left
        float horizontal = 1;

        public bool isJump;
        public LayerMask collisMask = 8 | 9;
        public LayerMask collisMaskFront_Spell;
        RaycastHit2D[] ra2d_checkHitOnJump;

        [Header("Debug Thing")]
        public Sprite[] placehold_character;
        public Color charColor;

        public override void Awake()
        {
            base.Start();

            collider = GetComponent<BoxCollider2D>();
            rb2d = GetComponent<Rigidbody2D>();
            spRenderer = GetComponent<SpriteRenderer>();

            charColor = spRenderer.color;
            collisions.faceDir = 1;

            tr_OriginTrans = transform.localPosition;
            //spRenderer.sprite = placehold_character[(int)e_charSide];
        }

        void Update()
        {
            if (isMove)
            {
                if (Input.GetKeyDown(KeyCode.Space)) CheckJump();

                if (isMove)
                {
                    if (b_isSpellingPhase)
                    {
                        SP_ControlChecker();
                    }
                    ActionPlayer();
                }
            }
        }

        void FixedUpdate()
        {
            //Jump Checker
            collisions.Reset();
            UpdateRaycastOrigins();
            //float directionY = Mathf.Sign(rb2d.velocity.y);
            float rayLength = Mathf.Abs(0.01f);

            for (int i = 0; i < verticalRayCount; i++)
            {
                Vector2 rayOrigin = raycastOrigins.bottomLeft;
                rayOrigin += Vector2.right * (verticalRaySpacing * i);
                RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.down, rayLength, collisionMaskForJump);

                Color colorHitJump = hit ? Color.green : Color.red;
                Debug.DrawRay(rayOrigin, Vector2.down, colorHitJump);

                if (hit)
                {
                    //print("hit");
                    isJump = false;
                    isGround = true;

                    break; // use continue; to pass for next iteration but now we use break; instead.
                }
                else
                {
                    isGround = false;
                }
            }

            //Move Horizontal
            if (Input.GetKey(KeyCode.A))
            {
                transform.localScale = new Vector2(-1, 1);
                horizontal = -1 * moveDirection;
                boxCastSizeFront.position = new Vector2(-.47f, boxCastSizeFront.position.y);
            }
            else if (Input.GetKey(KeyCode.D))
            {
                transform.localScale = new Vector2(1, 1);
                horizontal = 1 * moveDirection;
                boxCastSizeFront.position = new Vector2(.47f, boxCastSizeFront.position.y);
            }
            else
            {
                horizontal = 0;
            }


            if (isMove)
            {
                Movement();
            }
        }

        private void OnDisable()
        {
            isMove = true;
            isGuard = false;
            isGuardAll = false;
            isGround = true;
            _b_isAttack = false;
            //Attack(false);
        }

        void CheckJump()
        {
            if (!isJump && isGround)
            {
                //AudioManager.instance.Play("SFX_Jump");
                isJump = true;
                rb2d.AddForce(Vector2.up * jump);
                isGround = false;
                i_countJump = 1;
            }
            else if (numberjump > i_countJump)
            {
                rb2d.Sleep();
                isGround = false;
                rb2d.AddForce(Vector2.up * jump / 1.5f);
                i_countJump++;
            }
        }

        void Movement()
        {
            GetComponent<BoxCollider2D>().sharedMaterial.friction = (isGround) ? 1 : 0;

            if (horizontal * rb2d.velocity.x < 5)
            {
                //rb2d.AddForce(Vector2.right * horizontal * speed);
                rb2d.velocity = new Vector2(horizontal * speed, rb2d.velocity.y);
            }

            //print("speed : " + speed + "| rb2d speed x : " + rb2d.velocity.x);
        }

        public void Knock(Vector3 vector)
        {
            //if (isStreng)          //Invincible can't be knock
            //    return;
            if (isGuard)
            {
                //rb2d.velocity = new Vector2(vector.x / Mathf.Abs(vector.x), 0);
            }
            else
                KnockBack(vector);
        }

        public void KnockBack(Vector3 vector)
        {
            if (isGuardAll) return;
            rb2d.velocity = (vector * 6);
            StartCoroutine(CancelMovement(1));
            _b_isAttack = false;
            StopCoroutine(Attack());
        }

        IEnumerator CancelMovement(float sec)
        {
            isMove = false;
            isGuardAll = true;
            yield return new WaitForSeconds(sec);
            isMove = true;
            yield return new WaitForSeconds(0.2f);
            isGuardAll = false;
        }
        private void ActionPlayer()
        {
            if (!_b_isAttack)
            {
                if (Input.GetKeyDown(KeyCode.Z) && !isGuard && !b_isSpellingPhase)
                {
                    print("Attack");
                    StartCoroutine(Attack());
                }
            }
        }

        bool _b_isAttack = false;
        private IEnumerator Attack()
        {
            _b_isAttack = true;
            //animPlayer.SetTrigger("punch");
            yield return new WaitForSeconds(0.15f);
            CreateHitBox();
            yield return new WaitForSeconds(0.1f);
            _b_isAttack = false;
        }

        private void CreateHitBox()
        {
            if (!b_isSpellingPhase)
            {
                print("POW!");

                ra2_smashGroundAll = Physics2D.BoxCastAll(transform.position + (Vector3)boxCastSizeEx.position, boxCastSizeEx.size, transform.eulerAngles.z, Vector2.down, 0);
                foreach (RaycastHit2D ra2_allhit in ra2_smashGroundAll)
                {
                    print("What target hit : " + ra2_allhit.transform.gameObject);
                    if (ra2_allhit && ra2_allhit.collider.tag == "block_neutral")
                    {

                    }
                }
            }
        }


        private void OnTriggerEnter2D(Collider2D col)
        {

        }

        private void OnTriggerExit2D(Collider2D collision)
        {

        }

        #region ********* Spelling Phase Script **********


        void SP_ControlChecker()
        {
            //print("passed");
            ra2_hitFront = Physics2D.BoxCast(transform.position + (Vector3)boxCastSizeFront.position, boxCastSizeFront.size, transform.eulerAngles.z, Vector2.down, 0, collisMaskFront_Spell);

            if (ra2_hitFront && ra2_hitFront.collider.tag == "left_boundary")
            {

            }

        }

        #endregion

        private void OnDrawGizmos()
        {
            //Show BoxCast
                var fw = transform.TransformDirection(boxCastSize.position);
                Gizmos.color = Color.magenta;
                Gizmos.matrix = Matrix4x4.TRS(transform.position + fw, transform.rotation, Vector3.one);
                Gizmos.DrawWireCube(Vector2.zero, boxCastSize.size);
            
            var fwFront = transform.TransformDirection(boxCastSizeFront.position);
            Gizmos.color = Color.cyan;
            Gizmos.matrix = Matrix4x4.TRS(transform.position + fwFront, transform.rotation, Vector3.one);
            Gizmos.DrawWireCube(Vector2.zero, boxCastSizeFront.size);
        }
    }


    public struct CollisionInfo //Use to create Jump Raycast on each bound in Box Collider 2D
    {
        public bool above, below;
        public bool left, right;

        public bool climbingSlope;
        public bool descendingSlope;
        public bool slidingDownMaxSlope;

        public float slopeAngle, slopeAngleOld;
        public Vector2 slopeNormal;
        public Vector3 velocityOld;
        public int faceDir;
        public bool fallingThroughPlatform;

        public void Reset()
        {
            above = below = false;
            left = right = false;
            climbingSlope = false;
            descendingSlope = false;
            slidingDownMaxSlope = false;
            slopeNormal = Vector2.zero;

            slopeAngleOld = slopeAngle;
            slopeAngle = 0;
        }
    }
}

