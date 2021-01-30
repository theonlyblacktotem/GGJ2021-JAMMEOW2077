using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChildMovement : PlayerController
{

    private Coroutine coro;
    WaitForSeconds delayJumpInput = new WaitForSeconds(0.02f);

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

    void CheckJumpInput()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            coro = StartCoroutine(SetJumpCoro());
        }

        if (coro != null && (Input.GetKeyUp(KeyCode.Space) || holdCrate))
        {
            StopCoroutine(coro);
        }
    }

    IEnumerator SetJumpCoro()
    {
        yield return delayJumpInput;
        jump = true;
    }

    #region Helper


    public bool getClimbState()
    {
        return climb;
    }

    #endregion
}
