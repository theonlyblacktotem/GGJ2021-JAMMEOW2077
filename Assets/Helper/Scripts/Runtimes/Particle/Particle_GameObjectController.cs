using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GGJ2021
{
    [RequireComponent(typeof(ParticleSystem))]
    public class Particle_GameObjectController : MonoBehaviour
    {
        #region Variable

        public ParticleSystem particle { get; protected set; }
        public ParticleSystem particlePrefab { get; set; }

        #endregion

        #region Base - Mono

        void Awake()
        {
            particle = GetComponent<ParticleSystem>();
        }

        void OnDisable()
        {
            Invoke("DelayRegister", 0.1f);
        }

        #endregion

        #region Base - Main

        void DelayRegister()
        {
            Global_ParticlePoolingManager.RegisterToPooling(this);
        }

        #endregion
    }
}
