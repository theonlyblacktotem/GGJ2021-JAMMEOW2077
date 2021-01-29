using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player1 : PlayerController
{
    public static Player1 instance;

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

    void Update()
    {
        Walk();
    }
}
