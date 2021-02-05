using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NTTMS.Test
{    
    public enum PlayerFlag
    {
        LockMove    =   1 << 0,
        LockJump    =   1 << 1,
        LockClimb   =   1 << 2,
        LockCrouch  =   1 << 3,
        LockFlip    =   1 << 4
    }
}
