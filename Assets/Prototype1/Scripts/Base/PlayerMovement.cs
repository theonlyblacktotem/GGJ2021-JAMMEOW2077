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

    public float climbSpeed = 20f;
    public float runSpeed = 40f;
    public float dragSpeed = 1;
    public float rayUpDistance;
    public float rayFrontDistrance = 0.05f;

    bool jump = false;
    bool crouch = false;
    bool climb = false;
    public bool holdCrate = false;
    public GameObject[] AllCrate;
    void Awake()
    {
        
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
                Debug.Log(crate.name);
                crate.GetComponent<Crate>().DeActiveCrate();
                crate.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeRotation | RigidbodyConstraints2D.FreezePositionX;
            }
        }

        verticalMove = Input.GetAxisRaw("Vertical") * climbSpeed;
        horizontalMove = Input.GetAxisRaw("Horizontal") * runSpeed * dragSpeed;
        if (Input.GetButtonDown("Jump"))
        {

            jump = true;
        }
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            crouch = true;
        }else if (Input.GetKeyUp(KeyCode.LeftControl))
        {
            crouch = false;
        }


    }
    void FixedUpdate(){
        charactorController.Move(horizontalMove * Time.fixedDeltaTime  ,crouch ,jump);
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
}
