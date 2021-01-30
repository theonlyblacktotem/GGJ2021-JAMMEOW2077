using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChildMovement : PlayerController
{

    private Coroutine coro;

    void Update()
    {
        CheckHoldingCrate();
        MoveInput();

        if (Input.GetKeyDown(KeyCode.Space))
        {
            coro = StartCoroutine(SetJumpCoro());
        }

        if (coro != null && (Input.GetKeyUp(KeyCode.Space) || climb || holdCrate))
        {
            StopCoroutine(coro);
        }

        CheckCrouchInput();
    }

    private void FixedUpdate()
    {
        Move();
        ClimbLadder(KeyCode.W, KeyCode.S);
        HoldCrate();

        jump = false;
    }

    public override void SetDealth()
    {
        Debug.Log("Kid die");
    }

    IEnumerator SetJumpCoro()
    {
        yield return new WaitForSeconds(0.30f);
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

    #endregion
}

