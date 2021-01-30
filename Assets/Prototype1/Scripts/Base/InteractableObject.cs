using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InteractableObject : MonoBehaviour
{
    public bool interactable;
    // Show the player what button he need to press in order to interact with this 
    public GameObject button;
    // Update is called once per frame
    void Update()
    {
        if (interactable)
        {
          // Do somthing......
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            interactable = true;
            button.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            interactable = false;
            button.SetActive(false);
        }
    }
}
