using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NTTMS.Test
{    
    public enum PlayerLockFlag
    {
        LockMove    =   1 << 0,
        LockJump    =   1 << 1,
        LockClimb   =   1 << 2,
        LockCrouch  =   1 << 3,
        LockFlip    =   1 << 4,
        LockMoveLeft   =   1 << 5,
        LockMoveRight   =   1 << 6
    }
}
