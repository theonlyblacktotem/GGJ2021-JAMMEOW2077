using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GGJ2021
{
    [AddComponentMenu("GGJ2021/Camera/Camera Shake Controller")]
    public class Camera_ShakeController : MonoBehaviour
    {

        #region Variable

        #region Variable - Inspector

        [Header("Shake")]
        [Min(0)]
        public float duration = 1;
        public float amplitudeGain = 1;
        public float frequencyGain = 1;

        [Header("Condition")]
        public SpawnByType conditionType;

        [HideInInspector] public KeyCode inputKey;

        [HideInInspector] public PhysicInteractionType triggerType;

        [HideInInspector] public PhysicInteractionType collisionType;

        [HideInInspector] public Camera_ShakeCondition otherTrigger;

        #endregion

        #endregion

        #region Base - Mono

        void Update()
        {
            UpdateCheckSpawnType();
        }

        void OnTriggerEnter(Collider other)
        {
            if (conditionType != SpawnByType.Trigger
                || triggerType != PhysicInteractionType.Enter)
                return;

            ShakeCamera();
        }

        void OnTriggerEnter2D(Collider2D other)
        {
            if (conditionType != SpawnByType.Trigger
                || triggerType != PhysicInteractionType.Enter)
                return;

            ShakeCamera();
        }

        void OnTriggerStay(Collider other)
        {
            if (conditionType != SpawnByType.Trigger
                || triggerType != PhysicInteractionType.Stay)
                return;

            ShakeCamera();
        }

        void OnTriggerStay2D(Collider2D other)
        {
            if (conditionType != SpawnByType.Trigger
                || triggerType != PhysicInteractionType.Stay)
                return;

            ShakeCamera();
        }

        void OnTriggerExit(Collider other)
        {
            if (conditionType != SpawnByType.Trigger
                || triggerType != PhysicInteractionType.Exit)
                return;

            ShakeCamera();
        }

        void OnTriggerExit2D(Collider2D other)
        {
            if (conditionType != SpawnByType.Trigger
                || triggerType != PhysicInteractionType.Exit)
                return;

            ShakeCamera();
        }

        void OnCollisionEnter(Collision other)
        {
            if (conditionType != SpawnByType.Collision
                || triggerType != PhysicInteractionType.Enter)
                return;

            ShakeCamera();
        }

        void OnCollisionEnter2D(Collision2D other)
        {
            if (conditionType != SpawnByType.Collision
                || triggerType != PhysicInteractionType.Enter)
                return;

            ShakeCamera();
        }

        void OnCollisionStay(Collision other)
        {
            if (conditionType != SpawnByType.Collision
                || triggerType != PhysicInteractionType.Stay)
                return;

            ShakeCamera();
        }

        void OnCollisionStay2D(Collision2D other)
        {
            if (conditionType != SpawnByType.Collision
                || triggerType != PhysicInteractionType.Stay)
                return;

            ShakeCamera();
        }

        void OnCollisionExit(Collision other)
        {
            if (conditionType != SpawnByType.Collision
                || triggerType != PhysicInteractionType.Exit)
                return;

            ShakeCamera();
        }

        void OnCollisionExit2D(Collision2D other)
        {
            if (conditionType != SpawnByType.Collision
                || triggerType != PhysicInteractionType.Exit)
                return;

            ShakeCamera();
        }

        #endregion

        #region Base - Main

        void UpdateCheckSpawnType()
        {
            switch (conditionType)
            {
                case SpawnByType.Input:
                    if (Input.GetKeyDown(inputKey))
                    {
                        ShakeCamera();
                    }
                    break;

                case SpawnByType.Other:
                    if (otherTrigger != null && otherTrigger.PassedCondition(this))
                    {
                        ShakeCamera();
                    }
                    break;
            }
        }

        /// <summary>
        /// Shake camera.
        /// </summary>
        public void ShakeCamera()
        {
           Global_CameraManager.ShakeCamera(amplitudeGain,frequencyGain,duration);
        }

        #endregion

        #region Helper


        #endregion

    }
}