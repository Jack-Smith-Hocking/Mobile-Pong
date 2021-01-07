using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pong
{
    public enum PlayerID
    {
        PLAYER_ONE = 0,
        PLAYER_TWO = 1
    }

    public abstract class PongController : MonoBehaviour
    {
        public PlayerID m_playerID = PlayerID.PLAYER_ONE;
        [Min(0.1f)] public float m_moveSpeed = 5;
        [Space]
        public float m_deadmanZone = 0.1f;

        public Vector2 m_verticalBounds;

        protected Vector2 m_touchBounds;
        protected Vector2 m_targetPos;

        protected KeyCode m_upCode = KeyCode.UpArrow;
        protected KeyCode m_downCode = KeyCode.DownArrow;

        protected virtual void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            
            Vector3 _pos = transform.position;
            _pos.y = m_targetPos.y;
            Gizmos.DrawCube(_pos, Vector3.one * 1);
        }

        protected virtual void Start()
        {
            if (m_playerID == PlayerID.PLAYER_ONE)
            {
                // Left Bound
                m_touchBounds.x = 0;
                // Right Bound
                m_touchBounds.y = 0.5f - (m_deadmanZone / 2);
                
                m_upCode = KeyCode.W;
                m_downCode = KeyCode.S;
            }
            else
            {
                // Left Bound
                m_touchBounds.x = 0.5f + (m_deadmanZone / 2);
                // Right Bound
                m_touchBounds.y = 1.0f;

                m_upCode = KeyCode.UpArrow;
                m_downCode = KeyCode.DownArrow;
            }

            m_targetPos.x = transform.position.y;

            m_verticalBounds.x += transform.position.y;
            m_verticalBounds.y += transform.position.y;
        }

        public virtual void Update()
        {
            Move();

            Vector3 _validPosition = transform.position;
            _validPosition.y = Mathf.Clamp(transform.position.y, m_verticalBounds.x, m_verticalBounds.y);

            transform.position = _validPosition;
        }

        protected abstract void Move();

        protected bool WithinBounds(Vector2 position)
        {
            float _leftBound = (Screen.width * m_touchBounds.x);
            float _rightBound = (Screen.width * m_touchBounds.y);

            return (position.x >= _leftBound) && (position.x <= _rightBound);
        }
    }
}