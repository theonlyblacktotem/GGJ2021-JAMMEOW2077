using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InteractableObject : MonoBehaviour
{
    public bool interactable;
    // Show the player what button he need to press in order to interact with this 
    public Image button;

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerMovement p_Movement = collision.gameObject.GetComponent<PlayerMovement>();
            if (p_Movement.climb || p_Movement.holdCrate)
            {
                button.gameObject.SetActive(false);
            }
            else
            {
                button.gameObject.SetActive(true);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            button.gameObject.SetActive(false);
        }
    }
}
