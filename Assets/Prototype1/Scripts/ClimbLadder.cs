using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClimbLadder : InteractableObject
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            GameObject player = collision.gameObject;
            player.GetComponent<Rigidbody2D>().gravityScale = 0;
            PlayerController pc = player.GetComponent<PlayerController>();
            pc.climbing = true;
            interactable = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            GameObject player = collision.gameObject;
            player.GetComponent<Rigidbody2D>().gravityScale = 1;
            PlayerController pc = player.GetComponent<PlayerController>();
            pc.climbing = false;
            interactable = false;
        }
    }
}
