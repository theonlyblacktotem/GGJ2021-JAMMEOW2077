using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    #region Variable

    public Character2DController charactorController;
    public LayerMask whatIsWall;
    public KeyCode keyAction;

    [HideInInspector] public GameObject[] AllCrate;

    [Header("Number")]
    [Space]
    public float climbSpeed = 20f;
    public float runSpeed = 40f;
    public float dragSpeed = 1;
    [Range(0, 2)] public float rayUpDistance;
    [Range(0, 1)] public float rayFrontDistrance = 0.05f;

    [HideInInspector] public ClimbableController climbObject;
    [HideInInspector] public bool cantMove;

    protected UnityAction<PlayerController> inputActionDownOverride;
    protected UnityAction<PlayerController> inputActionHoldOverride;
    protected UnityAction<PlayerController> inputActionUpOverride;

    protected float horizontalMove = 0f;
    protected float verticalMove = 0f;

    protected bool climb = false;
    protected bool holdCrate = false;
    protected bool jump = false;
    protected bool crouch = false;

    protected RaycastHit2D[] raycastHit = new RaycastHit2D[5];

    protected Coroutine coro;
    protected WaitForSeconds delayJumpInput = new WaitForSeconds(0.02f);

    public Animator anim;

    #endregion

    #region Base - Mono

    protected virtual void Awake()
    {
        AllCrate = GameObject.FindGameObjectsWithTag(TagName.crate);
        anim = GetComponent<Animator>();
    }

    #endregion

    #region Base - Main

    protected virtual void MoveInput()
    {
        if (cantMove)
        {
            verticalMove = 0;
            horizontalMove = 0;
            return;
        }

        verticalMove = Input.GetAxisRaw(gameObject.tag + " Vertical") * climbSpeed;
        horizontalMove = Input.GetAxisRaw(gameObject.tag + " Horizontal") * runSpeed * dragSpeed;
    }

    protected virtual void Move()
    {
        // Lock move X during climbing.
        if (climb)
            horizontalMove = 0;

        // Fixed stuck wall in air.
        if (!charactorController.grounded && IsFacingWall())
        {
            bool facingRight = charactorController.facingRight;
            if (horizontalMove > 0 && facingRight
                || horizontalMove < 0 && !facingRight)
                horizontalMove = 0;
        }

        // Fixed standing during crouch and has ceiling over head.
        bool nextCrouch = crouch;
        if (charactorController.wasCrouching && charactorController.ceiling)
            nextCrouch = true;

        charactorController.Move(horizontalMove * Time.fixedDeltaTime, nextCrouch, jump);
    }

    protected virtual void CheckCrouchInput(KeyCode keyCrouch)
    {
        if (Input.GetKeyDown(keyCrouch))
        {
            crouch = true;
        }
        else if (Input.GetKeyUp(keyCrouch))
        {
            crouch = false;
        }
    }

    protected virtual void ClimbLadder(KeyCode keyUp, KeyCode keyDown)
    {
        bool inputUp = Input.GetKey(keyUp);
        bool inputDown = Input.GetKey(keyDown);

        if ((climbObject != null && !jump)
            && !(climbObject.IsLowerThenCenter(transform.position)
                && charactorController.grounded && inputDown))
        {
            //GameObject ladder = topHit.collider.gameObject;
            if (inputUp || inputDown)
            {
                gameObject.layer = LayerMask.NameToLayer(LayerName.climb);
                climb = true;


                transform.position = new Vector3(climbObject.transform.position.x, transform.position.y, transform.position.z);
            }
        }
        else
        {
            gameObject.layer = LayerMask.NameToLayer(LayerName.player);
            climb = false;
        }

        charactorController.Climb(verticalMove * Time.fixedDeltaTime, climb);
        Debug.DrawRay(transform.position, Vector2.up * rayUpDistance, Color.red);
    }

    protected virtual void CheckHoldingCrate()
    {
        if (anim) anim.SetBool("Pushing", holdCrate);
        if (holdCrate)
        {
            dragSpeed = 0.4f;
            foreach (GameObject crate in AllCrate)
            {
                crate.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeRotation;
            }
        }
        else
        {
            dragSpeed = 1;
            foreach (GameObject crate in AllCrate)
            {
                //Debug.Log(crate.name);
                crate.GetComponent<Crate>().DeActiveCrate();
                crate.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeRotation | RigidbodyConstraints2D.FreezePositionX;
            }
        }
    }

    protected virtual void HoldCrate()
    {
        if (jump)
            return;

        LayerMask layerCrate = LayerMask.GetMask(LayerName.crate);
        RaycastHit2D hit = RaycastForward(layerCrate);

        // Check lower.
        if (!hit)
            hit = RaycastForward(layerCrate, new Vector2(0, -GetRaycastOffsetY()));


        if (hit /*&& !hit.collider.isTrigger*/)
        {
            //Debug.Log("Crate before!");
            if (hit.transform.CompareTag(TagName.crate))
            {
                if (Input.GetKey(KeyCode.Space))
                {
                    holdCrate = true;
                    if ((transform.localScale.x < 0 && horizontalMove < 0) || (transform.localScale.x > 0 && horizontalMove > 0))
                    {
                        //Debug.Log(hit.transform.name);
                        hit.rigidbody.AddForce(new Vector2(transform.localScale.x * 20 * Time.deltaTime, 0), ForceMode2D.Force);
                        hit.transform.gameObject.GetComponent<Crate>().ActiveCrate();
                    }
                }
                else
                {
                    holdCrate = false;
                }
            }
            else
            {
                holdCrate = false;
            }
        }
        else
        {

            holdCrate = false;
        }

        Debug.DrawRay(new Vector2(transform.position.x + GetRaycastOffsetX(), transform.position.y), GetFacingDirection() * rayFrontDistrance, Color.green);

    }

    public void ForceSetBoolTrue(string key)
    {
        anim.SetBool(key, true);
    }

    public void ForceSetTrigger(string key)
    {
        anim.SetTrigger(key);
    }

    public virtual void SetDealth()
    {
        anim.SetTrigger("Death");
        Instantiate(darknessPrefab);
        StartCoroutine(LosingPhase());
    }

    [SerializeField] GameObject darknessPrefab;
    IEnumerator LosingPhase()
    {
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void AddInputActionDownOverride(UnityAction<PlayerController> action)
    {
        inputActionDownOverride = action;
    }

    public void RemoveInputActionDownOverride(UnityAction<PlayerController> action)
    {
        if (inputActionDownOverride != action)
            return;

        inputActionDownOverride = null;
    }

    public void AddInputActionHoldOverride(UnityAction<PlayerController> action)
    {
        inputActionHoldOverride = action;
    }

    public void RemoveInputActionHoldOverride(UnityAction<PlayerController> action)
    {
        if (inputActionHoldOverride != action)
            return;

        inputActionHoldOverride = null;
    }

    public void AddInputActionUpOverride(UnityAction<PlayerController> action)
    {
        inputActionUpOverride = action;
    }

    public void RemoveInputActionUpOverride(UnityAction<PlayerController> action)
    {
        if (inputActionUpOverride != action)
            return;

        inputActionUpOverride = null;
    }

    protected void CheckJumpInput()
    {
        if (Input.GetKeyDown(keyAction))
            inputActionDownOverride?.Invoke(this);
        else if (Input.GetKey(keyAction))
            inputActionHoldOverride?.Invoke(this);
        else if (Input.GetKeyUp(keyAction))
            inputActionUpOverride?.Invoke(this);

        if (inputActionDownOverride == null)
        {
            if (Input.GetKeyDown(keyAction))
            {
                coro = StartCoroutine(SetJumpCoro());
            }

            if (coro != null && (Input.GetKeyUp(keyAction) || holdCrate))
            {
                StopCoroutine(coro);
            }
        }
    }

    protected IEnumerator SetJumpCoro()
    {
        yield return delayJumpInput;
        jump = true;
    }


    protected bool IsFacingWall()
    {
        int hit = Physics2D.RaycastNonAlloc(new Vector2(transform.position.x + GetRaycastOffsetX(), transform.position.y), GetFacingDirection(), raycastHit, rayFrontDistrance, whatIsWall);

        // Check lower
        if (hit == 0)
            hit = Physics2D.RaycastNonAlloc(new Vector2(transform.position.x + GetRaycastOffsetX(), transform.position.y - GetRaycastOffsetY()), GetFacingDirection(), raycastHit, rayFrontDistrance, whatIsWall);

        if (hit == 0)
            hit = Physics2D.RaycastNonAlloc(new Vector2(transform.position.x + GetRaycastOffsetX(), transform.position.y + GetRaycastOffsetY()), GetFacingDirection(), raycastHit, rayFrontDistrance, whatIsWall);


        return hit > 0;
    }

    protected float GetRaycastOffsetX()
    {
        float result = 0.15f * transform.localScale.x;
        return result;
    }

    protected float GetRaycastOffsetY()
    {
        float result = 0.25f * transform.localScale.y;
        return result;
    }

    protected Vector2 GetFacingDirection()
    {
        return charactorController.facingRight ? Vector2.right : Vector2.left;
    }

    protected RaycastHit2D RaycastForward(LayerMask layer, Vector2 originOffset = default)
    {
        return Physics2D.Raycast(new Vector2(transform.position.x + GetRaycastOffsetX() + originOffset.x, transform.position.y + originOffset.y), GetFacingDirection(), rayFrontDistrance, layer);
    }

    #endregion
}
