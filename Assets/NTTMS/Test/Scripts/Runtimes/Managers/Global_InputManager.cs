using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DSC.Core;
using DSC.Input;
using DSC.Event;
using UnityEngine.InputSystem;
using static UnityEngine.InputSystem.InputAction;
using UnityEngine.SceneManagement;

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
                if (m_hInstance == null && m_bAppStart && !m_bAppQuit)
                    Debug.LogWarning("Don't have Global_InputManager in scene.");

                return m_hInstance;
            }
        }

        protected override GameInputData gameInputData { get { return m_hGameInputData; } set { m_hGameInputData = value; } }

        protected override EventCallback<DSC_InputEventType, GameInputData> inputEvent { get { return m_evtInput; } set { m_evtInput = value; } }

        #endregion

        static Global_InputManager m_hInstance;
        static bool m_bAppStart;
        static bool m_bAppQuit;

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

            Application.quitting += OnAppQuit;

            InitInput(m_nMaxPlayerNumber, m_fSensitivity, m_fGravity);
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

        void OnDestroy()
        {
            if (m_hInstance == null || m_hInstance != this)
                return;
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

        #region Events

        void OnMovement_Player1(CallbackContext hValue)
        {
            SetRawAxis(0, hValue.ReadValue<Vector2>());
        }

        void OnMovement_Player2(CallbackContext hValue)
        {
            SetRawAxis(1, hValue.ReadValue<Vector2>());
        }

        void OnAction_Player1(CallbackContext hValue)
        {
            SetButtonInput(0, (int)InputButtonType.South, hValue.ReadValueAsButton());
        }

        void OnAction_Player2(CallbackContext hValue)
        {
            SetButtonInput(1, (int)InputButtonType.South, hValue.ReadValueAsButton());
        }

        #endregion


        #region Helper

        #endregion
    }
}