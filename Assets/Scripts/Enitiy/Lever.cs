﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lever : MonoBehaviour
{
    public bool isPull;
    public Collider2D whoIsPull;
    private Collider2D[] whoIsStanding;
    public GameObject[] platforms;

    private void Awake()
    {
        whoIsStanding = new Collider2D[2];
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Uncle"))
        {
            whoIsStanding[0] = collision;
        } else if (collision.CompareTag("Child"))
        {
            whoIsStanding[1] = collision;
        }
    }    
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Uncle"))
        {
            if (collision == whoIsPull)
            {
                isPull = false;
                whoIsPull = null;
            }

            whoIsStanding[0] = null;
        } else if (collision.CompareTag("Child"))
        {
            if (collision == whoIsPull)
            {
                isPull = false;
                whoIsPull = null;
            }
            whoIsStanding[1] = null;
        }
        
    }


    private void Update()
    {
        if (!isPull)
        {
            if (Input.GetKeyDown(KeyCode.Return)) {
                if (whoIsStanding[0] != null) {
                    SendMessageUpwards("ForceSetTrigger", "PullLeverU", SendMessageOptions.DontRequireReceiver);
                    whoIsPull = whoIsStanding[0];
                    isPull = true;
                }

            } else if (Input.GetKeyDown(KeyCode.Space)) {
                if (whoIsStanding[1] != null)
                {
                    SendMessageUpwards("ForceSetTrigger", "PullLeverK", SendMessageOptions.DontRequireReceiver);
                    whoIsPull = whoIsStanding[1];
                    isPull = true;
                }
            }
        } else {
            if ( (Input.GetKeyUp(KeyCode.Return) && whoIsPull.CompareTag("Uncle")) ||
                    (Input.GetKeyUp(KeyCode.Space) && whoIsPull.CompareTag("Child")) )
            {
                isPull = false;
                whoIsPull = null;
            }
        }

        foreach(var plat in platforms)
        {
            plat.GetComponent<Platform>().MoveUp(isPull);
        }
        
    }

}
