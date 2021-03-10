using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NTTMS.Test
{
    public interface IMoveable
    {
        void Move(Vector2 vMove);
        void StopMove();
        void Warp(Vector2 vWarpPosition);
    }
}
