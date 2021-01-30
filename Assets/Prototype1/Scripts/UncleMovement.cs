using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UncleMovement : MonoBehaviour
{
    // Start is called before the first frame update
    public Character2DController charactorController;
    public LayerMask whatIsLadder;
    public GameObject[] AllCrate;

    [Header("Number")]
    [Space]
    public float climbSpeed = 20f;
    public float runSpeed = 40f;
    public float dragSpeed = 1;
    [Range(0, 2)] public float rayUpDistance;
    [Range(0, 1)] public float rayFrontDistrance = 0.05f;

    public bool climb = false;
    public bool holdCrate = false;

    float horizontalMove = 0f;
    float verticalMove = 0f;

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
    }

    void FixedUpdate()
    {
        charactorController.Move(horizontalMove * Time.fixedDeltaTime, false, false);

        // Raycast for ladder climbing
        RaycastHit2D topHit = Physics2D.Raycast(transform.position, Vector2.up, rayUpDistance, whatIsLadder);

        if (topHit.collider != null)
        {
            GameObject ladder = topHit.collider.gameObject;
            if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.DownArrow))
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
        Debug.DrawRay(transform.position, Vector2.up * rayUpDistance, Color.red);


        RaycastHit2D hit = Physics2D.Raycast(new Vector2(transform.position.x + 0.15f * transform.localScale.x, transform.position.y), Vector2.right * transform.localScale.x, 0.5f, LayerMask.GetMask("Crate"));
        if (hit)
        {
            if (hit.transform.tag == "Crate")
            {
                if (Input.GetAxisRaw("UncleInteract") == 1.0f)
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
        Debug.DrawRay(new Vector2(transform.position.x + (0.15f * transform.localScale.x), transform.position.y), Vector2.right * transform.localScale.x * 0.5f, Color.green);
    }
}
