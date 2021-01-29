using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    public Entrance.EntryCheck whichWay = Entrance.EntryCheck.Start;
    public LayerMask whatisGround;
    public float walkSpeed;
    public float climbSpeed;
    public float jumpForce;
    public string scenePW;

    // For Customize Player's Input
    public KeyCode left;
    public KeyCode right;
    public KeyCode up;
    public KeyCode down;

    // Field for accessing object's components
    protected Rigidbody2D playerRB;
    protected BoxCollider2D p_Collider2D;

    protected void Walk()
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
    protected void Climb()
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

    // Ground checking use Collider
    protected bool IsGround()
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
