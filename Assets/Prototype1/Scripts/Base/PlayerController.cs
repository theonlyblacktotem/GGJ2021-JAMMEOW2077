using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    public float walkSpeed;
    public float climbSpeed;

    // For Customize Player's Input
    public KeyCode left;
    public KeyCode right;
    public KeyCode up;
    public KeyCode down;
    public KeyCode interact;

    [SerializeField] private bool isClimbing = false;

    // Field for accessing object's components
    private Rigidbody2D playerRB;
    void Start()
    {
        playerRB = GetComponent<Rigidbody2D>();
    }


    void Update()
    {
        /**
            Must write some code for checking if player is near a ladder
            Maybe use Raycast
        **/
        if (isClimbing)
        {
            Climb();
        }
        else
        {
            Walk();
        }
    }

    private void Walk()
    {
        if (Input.GetKey(left))
        {
            playerRB.velocity = new Vector2(-walkSpeed, playerRB.velocity.y);
        } 
        else if (Input.GetKey(right))
        {
            playerRB.velocity = new Vector2(walkSpeed, playerRB.velocity.y);
        }
        else
        {
            playerRB.velocity = new Vector2(0, playerRB.velocity.y);
        }
    }

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
}
