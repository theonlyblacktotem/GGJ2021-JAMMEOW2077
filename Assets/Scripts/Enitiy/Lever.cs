using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lever : MonoBehaviour
{

    public GameObject[] platforms;

    PlayerController pullByPlayer;

    private void OnTriggerEnter2D(Collider2D other)
    {
        var playerController = other.GetComponent<PlayerController>();
        if (playerController)
        {
            playerController.AddInputActionDownOverride(StartInteraction);
            playerController.AddInputActionHoldOverride(HoldInteraction);
            playerController.AddInputActionUpOverride(EndInteraction);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        var playerController = collision.GetComponent<PlayerController>();
        if (playerController)
        {
            playerController.RemoveInputActionDownOverride(StartInteraction);
            playerController.RemoveInputActionHoldOverride(HoldInteraction);
            playerController.RemoveInputActionUpOverride(EndInteraction);

            if (playerController == pullByPlayer)
            {
                LockPlayerMove(false);
                pullByPlayer = null;
            }
        }
    }


    private void Update()
    {
        PullAllPlatform(pullByPlayer != null);
    }

    #region Base - Main

    void StartInteraction(PlayerController playerController)
    {
        if (pullByPlayer && pullByPlayer != playerController)
            return;

        pullByPlayer = playerController;
        LockPlayerMove(true);
    }

    void HoldInteraction(PlayerController playerController)
    {

    }

    void EndInteraction(PlayerController playerController)
    {
        if (pullByPlayer != playerController)
            return;

        LockPlayerMove(false);
        pullByPlayer = null;
        playerController.anim.SetTrigger("PullLever");
    }

    #endregion

    #region Helper

    void PullAllPlatform(bool pull)
    {
        foreach (var plat in platforms)
        {
            if (plat == null)
                continue;
            plat.GetComponent<Platform>().MoveUp(pull);
        }

    }

    void LockPlayerMove(bool lockMove)
    {
        if (pullByPlayer)
        {
            pullByPlayer.cantMove = lockMove;
        }
    }

    #endregion
}
