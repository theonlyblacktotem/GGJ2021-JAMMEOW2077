using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using DSC.Core;

namespace NTTMS.Test
{
    public sealed class Global_TimeManager : DSC_Time
    {
        #region Variable

        #region Variable - Inspector
#pragma warning disable 0649

        [Min(0)]
        [SerializeField] float m_fTimeScale = 1;

#pragma warning restore 0649
        #endregion

        #region Variable - Property

        static Global_TimeManager instance
        {
            get
            {
                if (m_hInstance == null && m_bAppStart && !m_bAppQuit)
                    Debug.LogWarning("Don't have Global_TimeManager in scene.");

                return m_hInstance;
            }
        }

        protected override float customTime
        {
            get
            {
                return Time.unscaledTime;
            }
        }
        protected override float customTimeScale
        {
            get
            {
                return m_fTimeScale;
            }
        }

        protected override float customDeltaTime
        {
            get
            {
                return Time.unscaledDeltaTime * m_fTimeScale;
            }
        }

        protected override float customFixedDeltaTime
        {
            get
            {
                return Time.fixedUnscaledDeltaTime * m_fTimeScale;
            }
        }

        protected override UnityAction<float> onTimeScaleChangeAction
        {
            get
            {
                if (instance == null)
                    return null;

                return m_hInstance.m_hTimeScaleChange;
            }

            set
            {
                if (instance == null)
                    return;

                m_hInstance.m_hTimeScaleChange = value;
            }
        }

        protected override UnityAction<float> onUnityTimeScaleChangeAction
        {
            get
            {
                if (instance == null)
                    return null;

                return m_hInstance.m_hUnityTimeScaleChange;
            }

            set
            {
                if (instance == null)
                    return;

                m_hInstance.m_hUnityTimeScaleChange = value;
            }
        }

        #endregion

        static Global_TimeManager m_hInstance;
        static bool m_bAppStart;
        static bool m_bAppQuit;

        float m_fPreviousTimeScale;
 
        float? m_fResetTimeBackTime;

        float m_fOriginalFixedTimeDeltaTime;
        float? m_fResetUnityTimeBackTime;

        UnityAction<float> m_hTimeScaleChange;
        UnityAction<float> m_hUnityTimeScaleChange;

        #endregion

        #region Base - Mono

        private void Awake()
        {
            if (m_hInstance == null)
            {
                m_hBaseInstance = this;
                m_hInstance = this;
            }
            else if (m_hInstance != this)
            {
                Destroy(this);
                return;
            }

            Application.quitting += OnAppQuit;

            m_fPreviousTimeScale = m_fTimeScale;
            m_fOriginalFixedTimeDeltaTime = Time.fixedDeltaTime;
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

        private void Update()
        {
            float fTime = Time.unscaledTime;

            if(m_fTimeScale != m_fPreviousTimeScale)
            {
                m_fPreviousTimeScale = m_fTimeScale;
                m_hTimeScaleChange?.Invoke(m_fTimeScale);
            }

            if(m_fResetTimeBackTime != null)
            {
                if(fTime >= m_fResetTimeBackTime)
                {
                    MainChangeTimeScale(1);
                    m_fResetTimeBackTime = null;
                }
            }

            if(m_fResetUnityTimeBackTime != null)
            {
                if(fTime >= m_fResetUnityTimeBackTime)
                {
                    MainChangeUnityTimeScale(1);
                    m_fResetUnityTimeBackTime = null;
                }
            }
        }

        #endregion

        #region Main

        protected override void MainChangeUnityTimeScale(float fTimeScale,float fResetTime = 0)
        {
            if (fTimeScale < 0)
                fTimeScale = 0;

            Time.timeScale = fTimeScale;

            if (fTimeScale != 0)
            {
                Time.fixedDeltaTime = Time.timeScale * m_fOriginalFixedTimeDeltaTime;
            }
            else
            {
                Time.fixedDeltaTime = m_fOriginalFixedTimeDeltaTime;
            }

            m_hUnityTimeScaleChange?.Invoke(fTimeScale);

            if(fResetTime > 0)
            {
                m_fResetUnityTimeBackTime = Time.unscaledTime + fResetTime;
            }
        }

        protected override void MainChangeTimeScale(float fTimeScale, float fResetTime = 0)
        {
            if (fTimeScale < 0)
                fTimeScale = 0;

            m_fTimeScale = fTimeScale;
            m_fPreviousTimeScale = fTimeScale;

            m_hTimeScaleChange?.Invoke(fTimeScale);

            if (fResetTime > 0)
            {
                m_fResetTimeBackTime = Time.unscaledTime + fResetTime;
            }
        }

        #endregion

        #region Helper

        #endregion
    }
}