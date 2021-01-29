using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GGJ2021
{
    public abstract class Camera_ShakeCondition : ScriptableObject
    {
        public abstract bool PassedCondition(Camera_ShakeController hController);
    }
}