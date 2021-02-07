using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DSC.Core;
using DSC.Event;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

namespace NTTMS.Test
{
    public sealed class Global_GameplayManager : MonoBehaviour
    {
        #region Variable

        #region Variable - Inspector
#pragma warning disable 0649

#pragma warning restore 0649
        #endregion

        #region Variable - Property

        static Global_GameplayManager instance
        {
            get
            {
                if (m_hInstance == null && m_bAppStart && !m_bAppQuit)
                    Debug.LogWarning("Don't have Global_GameplayManager in scene.");

                return m_hInstance;
            }
        }

        #endregion

        static Global_GameplayManager m_hInstance;
        static bool m_bAppStart;
        static bool m_bAppQuit;

        EventCallback<GameplayEventType> m_hGameplayEvent = new EventCallback<GameplayEventType>();

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

        #region Events

        #region Add/Remove Gameplay Event

        public static void AddGameplayEventListener(GameplayEventType eEvent, UnityAction hAction)
        {
            if (instance == null)
                return;

            m_hInstance.MainAddGameplayEventListener(eEvent, hAction);
        }

        public static void AddGameplayEventListener(GameplayEventType eEvent, UnityAction hAction
            , EventOrder eOrder)
        {
            if (instance == null)
                return;

            m_hInstance.MainAddGameplayEventListener(eEvent, hAction, eOrder);
        }

        void MainAddGameplayEventListener(GameplayEventType eEvent, UnityAction hAction
            , EventOrder eOrder = EventOrder.Normal)
        {
            m_hGameplayEvent.Add(eEvent, hAction, eOrder);
        }

        public static void RemoveGameplayEventListener(GameplayEventType eEvent, UnityAction hAction)
        {
            if (m_hInstance == null)
                return;

            m_hInstance.MainRemoveGameplayEventListener(eEvent, hAction);
        }

        public static void RemoveGameplayEventListener(GameplayEventType eEvent, UnityAction hAction
            , EventOrder eOrder)
        {
            if (m_hInstance == null)
                return;

            m_hInstance.MainRemoveGameplayEventListener(eEvent, hAction, eOrder);
        }

        void MainRemoveGameplayEventListener(GameplayEventType eEvent, UnityAction hAction
            , EventOrder eOrder = EventOrder.Normal)
        {
            m_hGameplayEvent.Remove(eEvent, hAction, eOrder);
        }

        #endregion

        #endregion

        #region Main

        public static void GameOver()
        {
            if (instance == null)
                return;

            m_hInstance.m_hGameplayEvent.Run(GameplayEventType.GameOver);

            m_hInstance.StartCoroutine(m_hInstance.RestartScene());
        }

        IEnumerator RestartScene()
        {
            yield return new WaitForSeconds(1f);
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        #endregion

        #region Helper

        #endregion
    }
}