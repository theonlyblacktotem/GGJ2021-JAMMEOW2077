using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace NTTMS.Test
{
    [RequireComponent(typeof(Image))]
    [RequireComponent(typeof(Animation))]
    public class BlackScreenController : MonoBehaviour
    {
        #region Variable

        #region Variable - Inspector

        [Header("Animation Name")]
        [SerializeField] string m_sFadeIn = "BlackScreen_FadeIn";

        #endregion

        Image m_hImage;
        Animation m_hAnim;

        #endregion

        #region Base - Mono

        void Awake()
        {
            m_hImage = GetComponent<Image>();
            m_hAnim = GetComponent<Animation>();
        }

        void OnEnable()
        {
            Global_GameplayManager.AddGameplayEventListener(GameplayEventType.GameOver, OnGameOver);
        }

        void OnDisable()
        {
            Global_GameplayManager.RemoveGameplayEventListener(GameplayEventType.GameOver, OnGameOver);
        }

        #endregion

        #region Events

        void OnGameOver()
        {
            m_hAnim.Play(m_sFadeIn);
        }

        #endregion
    }
}
