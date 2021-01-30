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
        CheckCrouchInput();
    }

    private void FixedUpdate()
    {

        ClimbLadder(KeyCode.W, KeyCode.S);
        HoldCrate();
        Move();

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

        if (coro != null && (Input.GetKeyUp(KeyCode.Space) || climb || holdCrate))
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

    void CheckCrouchInput()
    {
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            crouch = true;
        }
        else if (Input.GetKeyUp(KeyCode.LeftControl))
        {
            crouch = false;
        }
    }

    public void SetDealth()
    {
        Debug.Log("Kid die");
    }
    public bool getClimbState()
    {
        return climb;
    }

    IEnumerator SetJumpCoro()
    {
        yield return new WaitForSeconds(0.02f);
        jump = true;
    }
}
