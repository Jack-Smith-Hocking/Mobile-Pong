using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Custom.Utility.Mobile;

namespace Pong
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class PlayerPaddle : PongController
    {
        public bool m_useMobileControls = false;

        private Rigidbody2D m_rigidbody = null;

        private float m_yDelta = 0;
        private float m_recordedY = 0;

        protected override void Start()
        {
            base.Start();

            m_rigidbody = GetComponent<Rigidbody2D>();
        }

        private void FixedUpdate()
        {
            m_rigidbody.MovePosition(m_targetPos);
        }
        protected override void Move()
        {
            if (Application.isMobilePlatform || m_useMobileControls)
            {
                foreach (var pos in TouchInputWrapper.GetAllInputPositions())
                {
                    if (WithinBounds(pos))
                    {
                        float _screenPerc = pos.y / Screen.height;
                        float _boundSize = (Mathf.Abs(m_verticalBounds.x) + Mathf.Abs(m_verticalBounds.y));
                        float _worldPerc = _boundSize * _screenPerc;
                       
                        m_targetPos.y = _worldPerc - (_boundSize / 2);

                        break;
                    }
                }

                if (!TouchInputWrapper.MainInputPresent)
                {
                    m_targetPos.y = transform.position.y;
                }

                m_targetPos.y = Mathf.Lerp(transform.position.y, m_targetPos.y, m_moveSpeed * Time.fixedDeltaTime);
            }
            else
            {
                if (Input.GetKey(m_upCode))
                {
                    m_yDelta += m_moveSpeed * Time.deltaTime;
                    m_recordedY = transform.position.y;
                }
                else if (Input.GetKey(m_downCode))
                {
                    m_yDelta -= m_moveSpeed * Time.deltaTime;
                    m_recordedY = transform.position.y;
                }
                else
                {
                    m_yDelta = 0;
                    m_recordedY = transform.position.y;
                }

                //m_targetPos = new Vector2(transform.position.x, Mathf.Lerp(transform.position.y, m_recordedY + m_yDelta, m_moveSpeed * Time.deltaTime));
                m_targetPos = new Vector3(transform.position.x, m_recordedY + m_yDelta, transform.position.z);
            }
        }
    }
}