using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

namespace GGJ2021
{
    [AddComponentMenu("")]
    [RequireComponent(typeof(CinemachineVirtualCamera))]
    public class Camera_CnVirtualController : MonoBehaviour
    {
        #region Variable

        #region Variable - Property

        public CinemachineVirtualCamera virtualCamera { get { return m_hVirtualCamera; } }
        public CinemachineBasicMultiChannelPerlin cameraNoise { get { return m_hCameraNoise; } }

        #endregion

        CinemachineVirtualCamera m_hVirtualCamera;
        CinemachineBasicMultiChannelPerlin m_hCameraNoise;

        NoiseSettings m_h6DShake;

        float? m_fShakeDuration;

        #endregion

        #region Base - Mono

        void Awake()
        {
            m_hVirtualCamera = GetComponent<CinemachineVirtualCamera>();
            m_hCameraNoise = m_hVirtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
            if (m_hCameraNoise == null)
                m_hCameraNoise = m_hVirtualCamera.AddCinemachineComponent<CinemachineBasicMultiChannelPerlin>();

            Global_CameraManager.RegisterVirtualController(this);
        }

        void Update()
        {
            if (m_fShakeDuration.HasValue)
            {
                CountingShakeDuration();
            }
        }

        #endregion

        #region Main

        public void ShakeCamera(float fAmplitudeGain, float fFrequencyGain, float fShakeDuration)
        {
            Load6DShakeIfNull();

            m_hCameraNoise.m_NoiseProfile = m_h6DShake;
            SetShakeValue(fAmplitudeGain, fFrequencyGain);

            if (fShakeDuration >= 0)
            {
                m_fShakeDuration = fShakeDuration;
            }
        }

        public void StopShakeCamera()
        {
            SetShakeValue(0, 0);
        }

        #endregion

        #region Helper

        void Load6DShakeIfNull()
        {
            if (m_h6DShake == null)
            {
                m_h6DShake = Resources.Load<NoiseSettings>("Noise_6DShake");
            }
        }

        void SetShakeValue(float fAmplitudeGain, float fFrequencyGain)
        {
            m_hCameraNoise.m_AmplitudeGain = fAmplitudeGain;
            m_hCameraNoise.m_FrequencyGain = fFrequencyGain;
        }

        void CountingShakeDuration()
        {
            m_fShakeDuration -= Time.deltaTime;

            if (m_fShakeDuration <= 0)
            {
                m_fShakeDuration = null;
                StopShakeCamera();
            }
        }

        #endregion
    }
}