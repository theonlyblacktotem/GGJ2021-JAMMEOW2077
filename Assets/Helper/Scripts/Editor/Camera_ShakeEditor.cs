using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using GGJ2021;

namespace GGJ2021Editor
{
    [CustomEditor(typeof(Camera_ShakeController))]
    public class Camera_ShakeEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            var controller = (Camera_ShakeController)target;

            ShowShakeConditionByType(controller, serializedObject);

            if (GUI.changed)
            {
                serializedObject.ApplyModifiedProperties();
            }
        }

        void ShowShakeConditionByType(Camera_ShakeController controller, SerializedObject serializedObject)
        {
            SpawnByType conditionType = controller.conditionType;

            float labelWidth = EditorGUIUtility.labelWidth - 0.79f;

            switch (conditionType)
            {
                case SpawnByType.Input:
                    ShakeByInput(controller, serializedObject);
                    break;

                case SpawnByType.Trigger:
                    ShakeByTrigger(controller, serializedObject);
                    break;

                case SpawnByType.Collision:
                    ShakeByCollision(controller, serializedObject);
                    break;

                case SpawnByType.Other:
                    ShakeByOther(controller, serializedObject);
                    break;
            }

        }

        void ShakeByInput(Camera_ShakeController controller, SerializedObject serializedObject)
        {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Input", GUILayout.Width(EditorGUIUtility.labelWidth));
            controller.inputKey = (KeyCode)EditorGUILayout.EnumPopup(controller.inputKey);
            EditorGUILayout.EndHorizontal();
        }

        void ShakeByTrigger(Camera_ShakeController controller, SerializedObject serializedObject)
        {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Trigger", GUILayout.Width(EditorGUIUtility.labelWidth));
            controller.triggerType = (PhysicInteractionType)EditorGUILayout.EnumPopup(controller.triggerType);
            EditorGUILayout.EndHorizontal();
        }

        void ShakeByCollision(Camera_ShakeController controller, SerializedObject serializedObject)
        {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Collision", GUILayout.Width(EditorGUIUtility.labelWidth));
            controller.collisionType = (PhysicInteractionType)EditorGUILayout.EnumPopup(controller.collisionType);
            EditorGUILayout.EndHorizontal();
        }

        void ShakeByOther(Camera_ShakeController controller, SerializedObject serializedObject)
        {
            controller.otherTrigger = (Camera_ShakeCondition)EditorGUILayout.ObjectField("Other", controller.otherTrigger, typeof(Camera_ShakeCondition), false);
        }

    }
}