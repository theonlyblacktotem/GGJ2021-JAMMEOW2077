using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InteractableObject : MonoBehaviour
{
    public List<string> whoIsInteractor; 
    public Image button; // Show the player what button he need to press in order to interact with this
    public bool IsInteracting; //

    protected void OnTriggerEnter2D(Collider2D collision)
    {
        if (IsInteracable(collision.gameObject.tag))
        {
            button.gameObject.SetActive(true);
        }
    }

    protected void OnTriggerExit2D(Collider2D collision)
    {
        if (IsInteracable(collision.gameObject.tag))
        {
            button.gameObject.SetActive(false);
        }
    }

    protected bool IsInteracable(string tag)
    {
        return whoIsInteractor.Contains(tag);
    }
}
