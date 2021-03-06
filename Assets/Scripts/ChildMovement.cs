﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChildMovement : PlayerController
{
    Animator anim;

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

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
        anim.SetTrigger("Death");
        Instantiate(darknessPrefab);
        StartCoroutine(LosingPhase());
        Debug.Log("Kid die");
    }
    [SerializeField] GameObject darknessPrefab;
    IEnumerator LosingPhase()
    {
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void ForceSetBoolTrue(string key)
    {
        anim.SetBool(key, true);
    }

    public void ForceSetTrigger(string key)
    {
        anim.SetTrigger(key);
    }

    #region Helper


    public bool getClimbState()
    {
        return climb;
    }

    #endregion
}
