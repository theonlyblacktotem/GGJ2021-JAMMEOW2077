using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UncleMovement : PlayerController
{
    // Update is called once per frame
    void Update()
    {
        CheckHoldingCrate();
        MoveInput();
    }

    void FixedUpdate()
    {
        HoldCrate();
        Move();
        ClimbLadder(KeyCode.UpArrow, KeyCode.DownArrow);        
    }

    public override void SetDealth()
    {
        Debug.Log("Uncle die");
    }
}
