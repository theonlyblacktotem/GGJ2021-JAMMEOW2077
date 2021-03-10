using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChildMovement : PlayerController
{

    void Update()
    {
        CheckHoldingCrate();
        MoveInput();

        CheckJumpInput();
        CheckCrouchInput(KeyCode.S);
    }

    private void FixedUpdate()
    {
        HoldCrate(KeyCode.Space);
        Move();
        ClimbLadder(KeyCode.W, KeyCode.S);



        jump = false;
    }

    public override void SetDealth()
    {
        base.SetDealth();
        Debug.Log("Kid die");
    }

    #region Helper


    public bool getClimbState()
    {
        return climb;
    }

    #endregion
}
