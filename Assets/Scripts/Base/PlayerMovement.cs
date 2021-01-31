using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    // Start is called before the first frame update
    public Character2DController charactorController;
    public LayerMask whatIsLadder;

    float horizontalMove = 0f;
    float verticalMove = 0f;

    [Header("Number")]
    [Space]
    public float climbSpeed = 20f;
    public float runSpeed = 40f;
    public float dragSpeed = 1;
    [Range(0, 2)] public float rayUpDistance;
    [Range(0, 1)] public float rayFrontDistrance = 0.05f;

    bool jump = false;
    bool crouch = false;
    public bool climb = false;
    public bool holdCrate = false;

    public GameObject[] AllCrate;

    Animator anim;
    private void Awake()
    {
        anim = GetComponent<Animator>();
    }


    void Start()
    {
        //AllCrate = GameObject.FindGameObjectsWithTag("Crate");
    }

    // Update is called once per frame
    void Update()
    {
        AllCrate = GameObject.FindGameObjectsWithTag("Crate");
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

        verticalMove = Input.GetAxisRaw(gameObject.tag + " Vertical") * climbSpeed;
        horizontalMove = Input.GetAxisRaw(gameObject.tag + " Horizontal") * runSpeed * dragSpeed;
        if (Input.GetButtonDown("Jump"))
        {

            jump = true;
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            crouch = true;
        }else if (Input.GetKeyUp(KeyCode.S))
        {
            crouch = false;
        }


    }
    void FixedUpdate(){
        charactorController.Move(horizontalMove * Time.fixedDeltaTime, crouch, jump);

        // Raycast for ladder climbing
        RaycastHit2D topHit = Physics2D.Raycast(transform.position, Vector2.up, rayUpDistance, whatIsLadder);

        if (topHit.collider != null)
        {
            GameObject ladder = topHit.collider.gameObject;
            if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.DownArrow))
            {
                gameObject.layer = LayerMask.NameToLayer("Climb");
                climb = true;
            }
            //if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.DownArrow))
            //{
            //    gameObject.layer = LayerMask.NameToLayer("Climb");
            //    climb = true;
            //}
        }
        else
        {
            gameObject.layer = LayerMask.NameToLayer("Player");
            climb = false;
        }

        charactorController.Climb(verticalMove * Time.fixedDeltaTime, climb);
        RaycastHit2D hit = Physics2D.Raycast(new Vector2(transform.position.x + 0.15f * transform.localScale.x, transform.position.y ), Vector2.right * transform.localScale.x,0.5f,LayerMask.GetMask("Crate"));
        if (hit)
        {
            if(hit.transform.tag == "Crate")
            {
                if (Input.GetKey(KeyCode.Return))
                {
                    holdCrate = true;
                    if ((transform.localScale.x < 0 && horizontalMove < 0) || (transform.localScale.x > 0 && horizontalMove > 0))
                    {
                        //Debug.Log(hit.transform.name);
                        hit.rigidbody.AddForce(new Vector2(transform.localScale.x*20*Time.deltaTime, 0),ForceMode2D.Force);
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
        Debug.DrawRay(new Vector2(transform.position.x + (0.15f * transform.localScale.x), transform.position.y), Vector2.right * transform.localScale.x * 0.5f,Color.green);
        jump = false;
    }


    public void ForceSetBoolTrue(string key)
    {
        anim.SetBool(key, true);
    }

    public void ForceSetTrigger(string key)
    {
        anim.SetTrigger(key);
    }
}
