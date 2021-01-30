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
        Move();
        ClimbLadder(KeyCode.UpArrow, KeyCode.DownArrow);
        HoldCrate();
    }

    public override void SetDealth()
    {
        Debug.Log("Uncle die");
    }
}
