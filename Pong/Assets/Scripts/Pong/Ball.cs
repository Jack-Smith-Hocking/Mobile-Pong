using System.Collections;
using System.Collections.Generic;
using Custom.Utility;
using UnityEngine;
using UnityEngine.UI;

namespace Pong
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class Ball : MonoBehaviour
    {
        public static Ball Instance = null;

        [Header("Indicator Details")]
        public GameObject m_directionIndicator = null;
        public bool m_indicatorAlwaysOn = true;

        [Header("Speed Details")]
        public FloatRange m_speedRange;

        [Header("Change Direction Details")]
        public SpriteRenderer m_ballRenderer;
        public Color m_directionChangeColour = Color.red;
        [Range(0, 100)] public float m_chargePerHit = 0;

        [Header("FX Details")]
        public Transform m_fxParent = null;
        public GameObject m_changeDirectionFX;
        public GameObject m_hitFX = null;

        public Vector2 Velocity { get { return m_rigidBody.velocity; } }
        public Vector2 DisplayedVelocity { get; private set; }

        private Rigidbody2D m_rigidBody;

        private Vector3 m_startPos;
        private Vector3 m_velocity;

        private Color m_startColour;

        private float m_currentCharge = 0;

        private bool m_isReleasing = false;
        private bool m_updateDirectionIndicator = true;
        private bool m_updateVel = false;

        private void Awake()
        {
            Instance = this;
            m_startPos = transform.position;
        }

        public void Start()
        {
            m_rigidBody = GetComponent<Rigidbody2D>();

            AddRandomForce(1, false);

            if (m_ballRenderer != null)
            {
                m_startColour = m_ballRenderer.color;
            }

            if (m_directionIndicator != null && !m_indicatorAlwaysOn)
            {
                m_directionIndicator.SetActive(false);
            }
        }

        private void Update()
        {
            if (m_updateDirectionIndicator && m_directionIndicator != null && m_indicatorAlwaysOn)
            {
                m_directionIndicator.transform.up = (Vector3)m_rigidBody.velocity;
            }

            if (!m_isReleasing)
            {
                DisplayedVelocity = Velocity;
            }
            else if (m_directionIndicator)
            {
                DisplayedVelocity = m_directionIndicator.transform.up;
            }
        }
        private void FixedUpdate()
        {
            if (m_updateVel)
            {
                m_rigidBody.velocity = m_velocity;

                m_updateVel = false;
            }

            float _sqrSpeed = m_rigidBody.velocity.sqrMagnitude;
            float _max = m_speedRange.MaxVal;
            float _min = m_speedRange.MinVal;

            if (_sqrSpeed > (_max * _max))
            {
                float _validSpeed = Mathf.Clamp(_sqrSpeed, _min * _min, _max * _max);
                m_rigidBody.velocity = m_rigidBody.velocity.normalized * Mathf.Sqrt(_validSpeed);
            }
        }

        IEnumerator ChangeDirection(float delay, bool retainSpeed, bool trueDirection = false)
        {
            if (m_directionIndicator != null && !m_indicatorAlwaysOn)
            {
                m_directionIndicator.SetActive(true);
            }

            m_updateDirectionIndicator = trueDirection;
            AddRandomForce(delay, retainSpeed, trueDirection);

            yield return new WaitForSeconds(delay);

            if (m_changeDirectionFX != null)
            {
                Instantiate(m_changeDirectionFX, transform.position, Quaternion.identity, m_fxParent);
            }

            m_rigidBody.velocity *= 1.2f;

            m_updateDirectionIndicator = true;

            if (m_directionIndicator != null && !m_indicatorAlwaysOn)
            {
                m_directionIndicator.SetActive(false);
            }
        }

        public void ResetBall()
        {
            transform.position = m_startPos;
            
            m_velocity = Vector3.zero;
            m_updateVel = true;
        }

        public void AddRandomForce(float delay)
        {
            AddRandomForce(delay, false);
        }
        public void AddRandomForce(float delay, bool retainSpeed = true, bool trueDirection = true)
        {
            StartCoroutine(AddRandomForce(delay, m_speedRange.MaxVal / 2, retainSpeed, trueDirection));
        }
        IEnumerator AddRandomForce(float delay, float multi, bool retainSpeed = false, bool trueDirection = true)
        {
            Vector2 _force;
            do
            {
                _force = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f));
            } while (_force.sqrMagnitude <= 0.05f || ((Mathf.Abs(_force.y) == 1 && _force.x == 0)));

            Vector2 _dir = _force * (trueDirection ? 1 : -1);
            if (m_directionIndicator)
            {
                m_directionIndicator.transform.up = (Vector3)_dir;
            }

            if (m_isReleasing)
            {
                DisplayedVelocity = _dir.normalized;
            }
            else
            {
                DisplayedVelocity = m_rigidBody.velocity;
            }

            yield return new WaitForSeconds(delay);

            if (retainSpeed)
            {
                float _currentSpeed = m_rigidBody.velocity.magnitude;
                if (_currentSpeed <= 0.5f)
                {
                    _currentSpeed = m_speedRange.MinVal;
                }
                _force = _force.normalized * _currentSpeed;
            }
            else
            {
                _force = _force.normalized * multi;
            }

            m_velocity = _force;
            m_updateVel = true;
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (m_hitFX)
            {
                Instantiate(m_hitFX, transform.position, Quaternion.identity, m_fxParent);
            }

            m_currentCharge += m_chargePerHit;

            if (m_ballRenderer)
            {
                m_ballRenderer.color = Color.Lerp(m_startColour, m_directionChangeColour, m_currentCharge / 100);
            }

            if (m_currentCharge >= 100 && !m_isReleasing)
            {
                StartCoroutine(ReleaseCharge());

                m_isReleasing = true;
            }
        }

        IEnumerator ReleaseCharge()
        {
            bool _showTrue = Random.Range(0, 2) == 1;
            StartCoroutine(ChangeDirection(1, true, _showTrue));

            yield return new WaitForSeconds(1);

            m_isReleasing = false;
            m_currentCharge = 0;

            if (m_ballRenderer != null)
            {
                m_ballRenderer.color = m_startColour;
            }
        }
    }
}