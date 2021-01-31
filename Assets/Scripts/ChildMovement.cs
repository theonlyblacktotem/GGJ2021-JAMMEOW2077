using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChildMovement : PlayerController
{

    
    
    void Update()
    {
        CheckHoldingCrate();
        MoveInput();

        CheckJumpInput();
        CheckCrouchInput(KeyCode.LeftControl);
    }

    private void FixedUpdate()
    {
        HoldCrate();
        Move();
        ClimbLadder(KeyCode.W, KeyCode.S);



        jump = false;
    }

    public override void SetDealth()
    {
        Debug.Log("Kid die");
    }

    #region Helper


    public bool getClimbState()
    {
        return climb;
    }

    #endregion
}
