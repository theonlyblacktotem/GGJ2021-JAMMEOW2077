using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using GGJ2021;

namespace GGJ2021Editor
{
    [CustomEditor(typeof(Particle_SpawnController))]
    public class Particle_SpawnEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            var controller = (Particle_SpawnController)target;

            ShowSpawnByTypeEvent(controller, serializedObject);

            if (GUI.changed)
            {
                serializedObject.ApplyModifiedProperties();
            }
        }

        void ShowSpawnByTypeEvent(Particle_SpawnController controller, SerializedObject serializedObject)
        {
            SpawnByType spawnByType = controller.spawnByType;

            float labelWidth = EditorGUIUtility.labelWidth - 0.79f;

            switch (spawnByType)
            {
                case SpawnByType.Input:
                    SpawnTypeInput(controller, serializedObject);
                    break;

                case SpawnByType.Trigger:
                    SpawnTypeTrigger(controller, serializedObject);
                    break;

                case SpawnByType.Collision:
                    SpawnTypeCollision(controller, serializedObject);
                    break;

                case SpawnByType.Other:
                    SpawnTypeOther(controller, serializedObject);
                    break;
            }

        }

        void SpawnTypeInput(Particle_SpawnController controller, SerializedObject serializedObject)
        {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Input", GUILayout.Width(EditorGUIUtility.labelWidth));
            controller.inputKey = (KeyCode)EditorGUILayout.EnumPopup(controller.inputKey);
            EditorGUILayout.EndHorizontal();
        }

        void SpawnTypeTrigger(Particle_SpawnController controller, SerializedObject serializedObject)
        {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Trigger", GUILayout.Width(EditorGUIUtility.labelWidth));
            controller.triggerType = (PhysicInteractionType)EditorGUILayout.EnumPopup(controller.triggerType);
            EditorGUILayout.EndHorizontal();
        }

        void SpawnTypeCollision(Particle_SpawnController controller, SerializedObject serializedObject)
        {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Collision", GUILayout.Width(EditorGUIUtility.labelWidth));
            controller.collisionType = (PhysicInteractionType)EditorGUILayout.EnumPopup(controller.collisionType);
            EditorGUILayout.EndHorizontal();
        }

        void SpawnTypeOther(Particle_SpawnController controller, SerializedObject serializedObject)
        {
            controller.otherTrigger = (Particle_SpawnCondition)EditorGUILayout.ObjectField("Other",controller.otherTrigger, typeof(Particle_SpawnCondition),false);
        }
    }
}
