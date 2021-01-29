using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    // Start is called before the first frame update
    public Character2DController CharactorController;
    float horizontalMove = 0f;
    public float runSpeed = 40f;
    public float dragSpeed = 1;
    bool jump = false;
    bool crouch = false;
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
        CharactorController.Move(horizontalMove * Time.fixedDeltaTime  ,crouch ,jump);
        RaycastHit2D hit = Physics2D.Raycast(new Vector2(transform.position.x + 0.3f * transform.localScale.x, transform.position.y ), Vector2.right * transform.localScale.x,0.5f,LayerMask.GetMask("Crate"));
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
        Debug.DrawRay(new Vector2(transform.position.x + (0.3f * transform.localScale.x), transform.position.y), Vector2.right * transform.localScale.x,Color.green);
        jump = false;
    }
    void OnTriggerStay2D(Collider2D col)
    {
        if (col.tag == "Ladder" && Input.GetKey(KeyCode.W))
        {
            GetComponent<Rigidbody2D>().gravityScale = 0;
        }
    }
    void OnTriggerExit2D(Collider2D col)
    {
        if(col.tag == "Ladder")
        {
            GetComponent<Rigidbody2D>().gravityScale = 3;
        }
    }
}
