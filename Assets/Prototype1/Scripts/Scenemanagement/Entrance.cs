using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entrance : MonoBehaviour
{
    public enum EntryCheck
    {
        Start,
        Moveon,
        Comeback
    };

    public EntryCheck entryChecking;
    public string entrancePW;

    void Start()
    {
        /*if (Player1.instance.scenePW == entrancePW)
        {
            Player1.instance.transform.position = transform.position;
        }
        else
        {
            Debug.Log("Wrong Entry");
        }*/
        
        if ((Player2.instance.scenePW == entrancePW) && (Player2.instance.whichWay == entryChecking))
        {
           Player2.instance.transform.position = transform.position;
        }
        else
        {
            Debug.Log("Wrong Entry");
        }
    }
}

