using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UncleMovement : PlayerController
{
    Animator anim;

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        CheckHoldingCrate();
        MoveInput();
    }

    void FixedUpdate()
    {
        HoldCrate(KeyCode.Return);
        Move();
        ClimbLadder(KeyCode.UpArrow, KeyCode.DownArrow);        
    }

    public override void SetDealth()
    {
        anim.SetTrigger("Death");
        Instantiate(darknessPrefab);
        StartCoroutine(LosingPhase());
        Debug.Log("Uncle die");
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

}
