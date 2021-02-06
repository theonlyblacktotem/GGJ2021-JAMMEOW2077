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

    public void ForceSetBoolTrue(string key)
    {
        m_hAnim.SetBool(key, true);
    }

    public void ForceSetTrigger(string key)
    {
        m_hAnim.SetTrigger(key);
    }

    #region Helper


    public bool getClimbState()
    {
        return climb;
    }

    #endregion
}
