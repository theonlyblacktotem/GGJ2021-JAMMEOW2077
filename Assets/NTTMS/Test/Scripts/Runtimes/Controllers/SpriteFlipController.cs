using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NTTMS.Test
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class SpriteFlipController : MonoBehaviour
    {
        #region Variable

        SpriteRenderer m_hSprite;

        #endregion

        #region Base - Mono

        void Awake()
        {
            m_hSprite = GetComponent<SpriteRenderer>();
        }

        #endregion

        #region Base - Main

        public void FlipX()
        {
            m_hSprite.flipX = !m_hSprite.flipX;
        }

        public void FlipY()
        {
            m_hSprite.flipY = !m_hSprite.flipY;
        }

        #endregion
    }
}
