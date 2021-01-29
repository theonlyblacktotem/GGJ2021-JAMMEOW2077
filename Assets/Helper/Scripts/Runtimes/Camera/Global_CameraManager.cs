using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

namespace GGJ2021
{
    [AddComponentMenu("")]
    public sealed class Global_CameraManager : MonoBehaviour
    {
        #region Variable

        #region Variable - Property

        static Global_CameraManager instance
        {
            get
            {
                if (m_hinstance == null
                    && (m_bAppStart && !m_bAppQuit))
                {
                    var gameObject = new GameObject("Global_CameraManager");

                    gameObject.AddComponent<Global_CameraManager>();
                }

                return m_hinstance;
            }
        }

        #endregion

        static Global_CameraManager m_hinstance;
        static bool m_bAppStart;
        static bool m_bAppQuit;

        Camera_CnVirtualController m_hVirtualController;

        #endregion

        #region Base - Mono

        void Awake()
        {
            if (m_hinstance == null)
            {
                m_hinstance = this;
            }
            else if (m_hinstance != this)
            {
                Destroy(this);
                return;
            }


            Application.quitting += OnAppQuit;
        }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        static void OnAppStart()
        {
            m_bAppStart = true;
            m_bAppQuit = false;
        }

        void OnAppQuit()
        {
            Application.quitting -= OnAppQuit;

            m_bAppStart = false;
            m_bAppQuit = true;
        }

        #endregion

        #region Main

        public static void RegisterVirtualController(Camera_CnVirtualController hController)
        {
            if (instance == null || hController == null)
                return;

            m_hinstance.m_hVirtualController = hController;
        }

        public static void ShakeCamera()
        {
            if (instance == null)
                return;

            m_hinstance.MainShakeCamera();
        }

        public static void ShakeCamera(float fShakeDuration)
        {
            if (instance == null)
                return;

            m_hinstance.MainShakeCamera(1, 1, fShakeDuration);
        }

        public static void ShakeCamera(float fAmplitudeGain, float fFrequencyGain)
        {
            if (instance == null)
                return;

            m_hinstance.MainShakeCamera(fAmplitudeGain, fFrequencyGain);
        }

        public static void ShakeCamera(float fAmplitudeGain, float fFrequencyGain, float fShakeDuration)
        {
            if (instance == null)
                return;

            m_hinstance.MainShakeCamera(fAmplitudeGain, fFrequencyGain, fShakeDuration);
        }

        void MainShakeCamera(float fAmplitudeGain = 1, float fFrequencyGain = 1, float fShakeDuration = -1)
        {
            SpawnVirtualCameraIfNull();

            m_hVirtualController.ShakeCamera(fAmplitudeGain, fFrequencyGain, fShakeDuration);
        }

        #endregion

        #region Helper

        void SpawnVirtualCameraIfNull()
        {
            if (m_hVirtualController)
                return;

            var hVirtualCamera = FindObjectOfType<CinemachineVirtualCamera>();

            if (hVirtualCamera == null)
                SpawnVirtualCamera();

            m_hVirtualController = hVirtualCamera.gameObject.AddComponent<Camera_CnVirtualController>();


            #region Method

            void SpawnVirtualCamera()
            {
                Camera hCameraMain = Camera.main;

                var hVirtualGO = new GameObject("CM vcam");
                hVirtualGO.transform.position = hCameraMain.transform.position;
                hVirtualCamera = hVirtualGO.AddComponent<CinemachineVirtualCamera>();
                hVirtualCamera.m_Lens.OrthographicSize = hCameraMain.orthographicSize;
                hCameraMain.gameObject.AddComponent<CinemachineBrain>();
            }

            #endregion
        }



        #endregion

    }
}