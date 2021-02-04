using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DSC.Core;

namespace NTTMS.Test
{
    public sealed class Global_PoolingManager : DSC_Pooling
    {
        #region Variable

        #region Variable - Inspector
#pragma warning disable 0649

#pragma warning restore 0649
        #endregion

        #region Variable - Property

        static Global_PoolingManager instance
        {
            get
            {
                if (m_hInstance == null && m_bAppStart && !m_bAppQuit)
                    Debug.LogWarning("Don't have Global_PoolingManager in scene.");

                return m_hInstance;
            }
        }

        #endregion

        static Global_PoolingManager m_hInstance;
        static bool m_bAppStart;
        static bool m_bAppQuit;

        #endregion

        #region Base - Mono

        private void Awake()
        {
            if (m_hInstance == null)
            {
                m_hInstance = this;
            }
            else if (m_hInstance != this)
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

        #region Helper

        #endregion
    }
}