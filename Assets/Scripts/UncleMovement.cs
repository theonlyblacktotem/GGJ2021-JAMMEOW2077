using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UncleMovement : PlayerController
{

    // Update is called once per frame
    void Update()
    {
        CheckHoldingCrate();
        MoveInput();

        CheckJumpInput();
    }

    void FixedUpdate()
    {
        HoldCrate(KeyCode.Return);
        Move();
        ClimbLadder(KeyCode.UpArrow, KeyCode.DownArrow);

        jump = false;
    }

    public override void SetDealth()
    {
        base.SetDealth();
        Debug.Log("Uncle die");
    }

    public void ForceSetBoolTrue(string key)
    {
        anim.SetBool(key, true);
    }

    public void ForceSetTrigger(string key)
    {
        anim.SetTrigger(key);
    }

}
