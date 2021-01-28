using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableObject : MonoBehaviour
{
    // Start is called before the first frame update
    public bool interactable;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        interactable = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        interactable = false;
    }
}
