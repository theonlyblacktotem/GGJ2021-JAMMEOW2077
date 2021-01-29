using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player2 : PlayerController
{
    public static Player2 instance;

    public KeyCode jump;
    public KeyCode crouch;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        playerRB = GetComponent<Rigidbody2D>();
        p_Collider2D = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        Walk();

        if (Input.GetKeyDown(jump) && base.IsGround())
        {
            Jump();
        }
    }

    private void Crouch()
    {
        // do something....
    }

    private void Jump()
    {
        playerRB.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
    }
}
