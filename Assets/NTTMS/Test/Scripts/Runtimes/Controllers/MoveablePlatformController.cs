using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NTTMS.Test
{
    public class MoveablePlatformController : MonoBehaviour
    {
        #region Variable

        #region Variable - Property

        public Vector2 previousMove { get { return m_vPreviousMove; } }

        #endregion

        Vector2 m_vPreviousPosition;
        Vector2 m_vPreviousMove;

        #endregion

        #region Base - Mono

        void Awake()
        {
            m_vPreviousPosition = transform.position;
        }

        void FixedUpdate()
        {
            m_vPreviousMove = (Vector2)transform.position - m_vPreviousPosition;
            m_vPreviousPosition = transform.position;
        }

        #endregion
    }
}
