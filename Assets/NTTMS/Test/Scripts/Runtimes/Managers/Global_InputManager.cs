using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DSC.Input;
using DSC.Event;

namespace NTTMS.Test
{
    public sealed class Global_InputManager : DSC_Input
    {
        #region Variable

        #region Variable - Inspector
#pragma warning disable 0649

        [Header("Axis")]
        [SerializeField] float m_fSensitivity = 3f;
        [SerializeField] float m_fGravity = 3f;

        [Header("Other")]
        [SerializeField] int m_nMaxPlayerNumber = 1;

#pragma warning restore 0649
        #endregion

        #region Variable - Property

        static Global_InputManager instance
        {
            get
            {
                if (m_hInstance == null)
                    Debug.LogWarning("Don't have Global_InputManager in scene.");

                return m_hInstance;
            }
        }

        protected override GameInputData gameInputData { get { return m_hGameInputData; } set { m_hGameInputData = value; } }

        protected override EventCallback<DSC_InputEventType, GameInputData> inputEvent { get { return m_evtInput; } set { m_evtInput = value; } }

        #endregion

        static Global_InputManager m_hInstance;

        GameInputData m_hGameInputData;
        EventCallback<DSC_InputEventType, GameInputData> m_evtInput = new EventCallback<DSC_InputEventType, GameInputData>();

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
                Destroy(gameObject);
                return;
            }

            InitInput(m_nMaxPlayerNumber, m_fSensitivity, m_fGravity);
        }

        private void Update()
        {
            UpdateInput();
        }

        private void LateUpdate()
        {
            LateUpdateInput();
        }

        #endregion
    }
}