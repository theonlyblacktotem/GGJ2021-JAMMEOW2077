using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GGJ2021
{
    public abstract class Particle_SpawnCondition : ScriptableObject
    {
        public abstract bool PassedCondition(Particle_SpawnController hSpawnController);
    }
}
