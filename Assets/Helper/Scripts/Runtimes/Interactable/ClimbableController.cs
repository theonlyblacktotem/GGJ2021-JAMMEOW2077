using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("GGJ2021/Object/ClimbableController")]
public class ClimbableController : MonoBehaviour
{
    #region Variable

    #region Variable - Inspector

    [Min(0)]
    public float maxCenterXDistance = 0.2f;

    #endregion

    BoxCollider2D boxCollider;

    #endregion

    #region Base - Mono

    void Awake()
    {
        boxCollider = GetComponent<BoxCollider2D>();
    }

    void OnTriggerStay2D(Collider2D other)
    {
        var playerController = other.GetComponent<PlayerController>();
        if (playerController)
        {
            if (NearCenterX(other.transform.position))
            {
                SetPlayerCanClimb(playerController, true);
            }
            else
            {
                SetPlayerCanClimb(playerController, false);
            }
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        var playerController = other.GetComponent<PlayerController>();
        if (playerController)
        {
            SetPlayerCanClimb(playerController, false);
        }
    }

    #endregion

    #region Base - Main

    public bool IsLowerThenCenter(Vector2 position)
    {
        bool result = false;

        if (boxCollider)
        {
            Vector2 centerPosition = transform.position;
            Vector2 offset = boxCollider.offset;
            offset.x *= transform.localScale.x;
            offset.y *= transform.localScale.y;
            centerPosition += offset;
            
            result = position.y < centerPosition.y;
        }

        return result;
    }

    #endregion

    #region Helper

    bool NearCenterX(Vector2 position)
    {
        return (Mathf.Abs(transform.position.x) - Mathf.Abs(position.x)
                    <= maxCenterXDistance);
    }

    void SetPlayerCanClimb(PlayerController playerController, bool canClimb)
    {
        playerController.climbObject = canClimb ? this : null;
    }

    #endregion
}
