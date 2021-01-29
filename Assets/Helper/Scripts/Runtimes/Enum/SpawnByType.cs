using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GGJ2021
{
    public enum SpawnByType
    {
        Input,
        Trigger,
        Collision,
        Other = 1 << 31
    }
}
