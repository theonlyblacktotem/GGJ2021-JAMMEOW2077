using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GGJ2021
{
    public sealed class Global_ParticlePoolingManager : MonoBehaviour
    {
        #region Variable

        #region Variable - Property

        static Global_ParticlePoolingManager instance
        {
            get
            {
                if (m_hinstance == null
                    && (m_bAppStart && !m_bAppQuit))
                {
                    var gameObject = new GameObject("Global_ParticlePooling");

                    gameObject.AddComponent<Global_ParticlePoolingManager>();
                }

                return m_hinstance;
            }
        }

        #endregion

        static Global_ParticlePoolingManager m_hinstance;
        static bool m_bAppStart;
        static bool m_bAppQuit;

        Dictionary<ParticleSystem, Stack<Particle_GameObjectController>> m_dicParticlePooling = new Dictionary<ParticleSystem, Stack<Particle_GameObjectController>>();

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

        public static void RegisterToPooling(Particle_GameObjectController hParticleController)
        {
            if (instance == null || hParticleController == null)
                return;

            hParticleController.transform.SetParent(m_hinstance.transform);

            var dicPooling = m_hinstance.m_dicParticlePooling;
            var hOriginalPrefab = hParticleController.particlePrefab;

            if (dicPooling.ContainsKey(hOriginalPrefab))
            {
                var stkPooling = dicPooling[hOriginalPrefab];
                
                if(stkPooling == null)
                    stkPooling = new Stack<Particle_GameObjectController>();
                
                stkPooling.Push(hParticleController);
            }
            else
            {
                var stkPooling = new Stack<Particle_GameObjectController>();
                stkPooling.Push(hParticleController);
                dicPooling.Add(hOriginalPrefab, stkPooling);
            }
        }

        public static bool TryGetParticle(ParticleSystem hParticlePrefab, out ParticleSystem hParticle)
        {
            hParticle = null;
            if (instance == null  
                || !m_hinstance.m_dicParticlePooling.ContainsKey(hParticlePrefab)
                || m_hinstance.m_dicParticlePooling[hParticlePrefab].Count <= 0)
            {
                return false;
            }

            var hParticleController = m_hinstance.m_dicParticlePooling[hParticlePrefab].Pop();            
            if(hParticleController)
                hParticle = hParticleController.particle;

            return hParticle != null;
        }

        #endregion
    }
}
