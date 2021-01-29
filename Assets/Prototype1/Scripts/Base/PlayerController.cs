using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public LayerMask whatisGround;
    public float walkSpeed;
    public float climbSpeed;
    public float jumpForce;

    // For Customize Player's Input
    public KeyCode left;
    public KeyCode right;
    public KeyCode up;
    public KeyCode down;
    public KeyCode jump;
    public KeyCode crouch;

    public bool jumpEnable = false;
    public bool crouchEnable = false;
    private bool isClimbing = false;
    private bool isCrouching = false;

    // Field for accessing object's components
    private Rigidbody2D playerRB;
    private BoxCollider2D p_Collider2D;
    void Start()
    {
        playerRB = GetComponent<Rigidbody2D>();
        p_Collider2D = GetComponent<BoxCollider2D>();
    }


    void Update()
    {
        /**
            Must write some code for checking if player is near a ladder
            Maybe use Raycast
        **/
 
        Walk();

        if (jumpEnable)
        {
            Jump();
        }

        if (crouchEnable)
        {
            Crouch();
        }
    }

    private void Walk()
    {
        Vector2 currentPos = transform.position;
        if (Input.GetKey(right))
        {
            currentPos.x += walkSpeed * Time.deltaTime;
            transform.position = currentPos;
        }
        else if (Input.GetKey(left))
        {
            currentPos.x -= walkSpeed * Time.deltaTime;
            transform.position = currentPos;
        }  
    }

    // Use When Player is in Climbing state
    private void Climb()
    {
        if (Input.GetKey(up))
        {
            playerRB.velocity = new Vector2(0, climbSpeed);
        }
        else if (Input.GetKey(down))
        {
            playerRB.velocity = new Vector2(0, -climbSpeed);
        }
        else
        {
            playerRB.velocity = new Vector2(0, 0);
        }
    }

    private void Crouch()
    {
        // do something....
    }

    private void Jump()
    {
        if (Input.GetKeyDown(jump) && IsGround())
        {
            playerRB.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        }
    }

    // Ground checking use Collider
    private bool IsGround()
    {
        Vector2 topLeftPoint = transform.position;
        topLeftPoint.x -= p_Collider2D.bounds.extents.x;
        topLeftPoint.y += p_Collider2D.bounds.extents.y;
        Vector2 bottomRightPoint = transform.position;
        bottomRightPoint.x += p_Collider2D.bounds.extents.x;
        bottomRightPoint.y -= p_Collider2D.bounds.extents.y;

        return Physics2D.OverlapArea(topLeftPoint, bottomRightPoint, whatisGround);
    }
}
