using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GGJ2021
{
    public abstract class Particle_SpawnTrigger : ScriptableObject
    {
        public abstract bool IsTrigger(Particle_SpawnController hSpawnController);
    }
}
