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
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (holdCrate)
        {
            dragSpeed = 0.4f;
        }
        else
        {
            dragSpeed = 1;
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
        // Raycast for ladder climbing
        RaycastHit2D topHit = Physics2D.Raycast(transform.position, Vector2.up, rayUpDistance, whatIsLadder);

        if (topHit.collider != null)
        {
            if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.S))
            {
                gameObject.layer = LayerMask.NameToLayer("Climb");
                climb = true;
            }
        }
        else
        {
            gameObject.layer = LayerMask.NameToLayer("Player");
            climb = false;
        }

        charactorController.Climb(verticalMove * Time.fixedDeltaTime, climb);

        charactorController.Move(horizontalMove * Time.fixedDeltaTime, crouch, jump);
        RaycastHit2D hit = Physics2D.Raycast(new Vector2(transform.position.x + 0.3f * transform.localScale.x, transform.position.y ), Vector2.right * transform.localScale.x, rayFrontDistrance, LayerMask.GetMask("Crate"));
        if (hit)
        {
            if(hit.transform.tag == "Crate")
            {
                if (Input.GetKey(KeyCode.Return))
                {
                    holdCrate = true;
                    hit.transform.position = new Vector2(hit.transform.position.x + (transform.position.x + 0.8f * transform.localScale.x - hit.transform.position.x), hit.transform.position.y);
                }
                else
                {
                    holdCrate = false;
                }
            }
        }
        else
        {
            holdCrate = false;
        }
        Debug.DrawRay(transform.position, Vector2.up * rayUpDistance, Color.green);
        Debug.DrawRay(new Vector2(transform.position.x + (0.3f * transform.localScale.x), transform.position.y), Vector2.right * transform.localScale.x,Color.red);
        jump = false;
    }
}
