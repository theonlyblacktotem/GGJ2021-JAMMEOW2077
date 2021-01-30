using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace GGJ2021
{
    [AddComponentMenu("GGJ2021/Particle/Particle Spawn Controller")]
    public class Particle_SpawnController : MonoBehaviour
    {
        #region Variable

        #region Variable - Inspector

        [Header("Particle")]
        public ParticleSystem particlePrefab;
        public Transform particleSpawnPosition;

        [Header("Spawn")]
        public SpawnByType spawnByType;

        [HideInInspector] public KeyCode inputKey;

        [HideInInspector] public PhysicInteractionType triggerType;

        [HideInInspector] public PhysicInteractionType collisionType;

        [HideInInspector] public Particle_SpawnCondition otherTrigger;

        #endregion

        #endregion

        #region Base - Mono

        void Update()
        {
            UpdateCheckSpawnType();
        }

        void OnTriggerEnter(Collider other)
        {
            if (spawnByType != SpawnByType.Trigger
                || triggerType != PhysicInteractionType.Enter)
                return;

            SpawnParticle();
        }

        void OnTriggerEnter2D(Collider2D other)
        {
            if (spawnByType != SpawnByType.Trigger
                || triggerType != PhysicInteractionType.Enter)
                return;

            SpawnParticle();
        }

        void OnTriggerStay(Collider other)
        {
            if (spawnByType != SpawnByType.Trigger
                || triggerType != PhysicInteractionType.Stay)
                return;

            SpawnParticle();
        }

        void OnTriggerStay2D(Collider2D other)
        {
            if (spawnByType != SpawnByType.Trigger
                || triggerType != PhysicInteractionType.Stay)
                return;

            SpawnParticle();
        }

        void OnTriggerExit(Collider other)
        {
            if (spawnByType != SpawnByType.Trigger
                || triggerType != PhysicInteractionType.Exit)
                return;

            SpawnParticle();
        }

        void OnTriggerExit2D(Collider2D other)
        {
            if (spawnByType != SpawnByType.Trigger
                || triggerType != PhysicInteractionType.Exit)
                return;

            SpawnParticle();
        }

        void OnCollisionEnter(Collision other)
        {
            if (spawnByType != SpawnByType.Collision
                || triggerType != PhysicInteractionType.Enter)
                return;

            SpawnParticle();
        }

        void OnCollisionEnter2D(Collision2D other)
        {
            if (spawnByType != SpawnByType.Collision
                || triggerType != PhysicInteractionType.Enter)
                return;

            SpawnParticle();
        }

        void OnCollisionStay(Collision other)
        {
            if (spawnByType != SpawnByType.Collision
                || triggerType != PhysicInteractionType.Stay)
                return;

            SpawnParticle();
        }

        void OnCollisionStay2D(Collision2D other)
        {
            if (spawnByType != SpawnByType.Collision
                || triggerType != PhysicInteractionType.Stay)
                return;

            SpawnParticle();
        }

        void OnCollisionExit(Collision other)
        {
            if (spawnByType != SpawnByType.Collision
                || triggerType != PhysicInteractionType.Exit)
                return;

            SpawnParticle();
        }

        void OnCollisionExit2D(Collision2D other)
        {
            if (spawnByType != SpawnByType.Collision
                || triggerType != PhysicInteractionType.Exit)
                return;

            SpawnParticle();
        }

        #endregion

        #region Base - Main

        void UpdateCheckSpawnType()
        {
            switch (spawnByType)
            {
                case SpawnByType.Input:
                    if (Input.GetKeyDown(inputKey))
                    {
                        SpawnParticle();
                    }
                    break;

                case SpawnByType.Other:
                    if (otherTrigger != null && otherTrigger.PassedCondition(this))
                    {
                        SpawnParticle();
                    }
                    break;
            }
        }

        /// <summary>
        /// Spawn particle GameObject in scene.
        /// </summary>
        public void SpawnParticle()
        {
            if (particlePrefab == null)
            {
                Debug.LogError("Don't have Particle Prefab to spawn!!");
                return;
            }

            Vector3 vSpawnPosition = particleSpawnPosition != null ?
                particleSpawnPosition.position : transform.position;
            
            ParticleSystem hParticle;

            InstantiateParticle();
            ChangeStopActionToDisable();
            AddParticleController();


            #region Method

            void InstantiateParticle()
            {
                if (!Global_ParticlePoolingManager.TryGetParticle(particlePrefab, out hParticle))
                    hParticle = Instantiate(particlePrefab, vSpawnPosition, particlePrefab.transform.rotation);
                else
                {
                    hParticle.transform.SetParent(null);
                    hParticle.transform.position = vSpawnPosition;
                    hParticle.transform.rotation = particlePrefab.transform.rotation;
                    hParticle.gameObject.SetActive(true);
                }
            }

            void ChangeStopActionToDisable()
            {
                ParticleSystem.MainModule hMainModule = hParticle.main;
                hMainModule.stopAction = ParticleSystemStopAction.Disable;
            }

            void AddParticleController()
            {
                var hParticleController = hParticle.GetComponent<Particle_GameObjectController>();
                if (hParticleController == null)
                {
                    hParticleController = hParticle.gameObject.AddComponent<Particle_GameObjectController>();
                }

                hParticleController.particlePrefab = particlePrefab;
            }

            #endregion
        }


        #endregion

        #region Helper


        #endregion
    }
}
