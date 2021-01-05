using System.Collections;
using System.Collections.Generic;
using Custom.Utility;
using UnityEngine;

namespace Pong
{
    static class Test
    {
        public static IEnumerator TestCo()
        {
            yield return new WaitForSeconds(2);
        }
    }

    public class AIPaddle : Controller
    {
        public Transform m_fakeBackWall = null;
        [Space]
        [Min(0)] public int m_reflectionResolution = 5;

        public LayerMask m_reflectionLayers;

        public bool m_resetPaddleToCentre = true;

        protected override void Start()
        {
            base.Start();

            InvokeRepeating(nameof(SetTarget), 0, 0.25f);
        }

        void SetTarget()
        {
            m_targetPos.y = CalculateTarget();
        }

        private RaycastHit2D GetFirstValidTransform(RaycastHit2D[] rayHits, RaycastHit2D ignoreHit)
        {
            foreach (var hit in rayHits)
            {
                if (ignoreHit.transform == null || !GeneralUtil.AreEqual(hit.transform, ignoreHit.transform))
                {
                    return hit;
                }
            }

            return new RaycastHit2D();
        }

        private float TestReflection(Vector2 pos, Vector2 dir)
        {
            bool _return = false;
            float _targetY = m_targetPos.y;

            Vector2 _rayDir = dir.normalized;
            Vector2 _rayPos = pos;
            RaycastHit2D[] _rayHits = Physics2D.RaycastAll(_rayPos, _rayDir, 10000, m_reflectionLayers);

            RaycastHit2D _currentHit = GetFirstValidTransform(_rayHits, new RaycastHit2D());

            GeneralUtil.SafeDoWhile(
                // Action
                () =>
                {
                    if (_currentHit.transform != null)
                    {
                        Debug.DrawRay((Vector3)_rayPos, (Vector3)_rayDir * 1000, Color.red, 0.1f);

                        _rayDir = Vector2.Reflect(_rayDir, _currentHit.normal);
                        _rayPos = _currentHit.point;

                        Transform _hitTrans = _currentHit.collider.transform; 
                        // Check if the hit object is the back board or the paddle
                        // If it is either of those than move to the hit position
                        if (_hitTrans.Equals(m_fakeBackWall)) 
                        {
                            _return = true;
                            _targetY = _currentHit.point.y;
                        }
                        else if (_hitTrans.Equals(transform))
                        {
                            _return = true;
                        }
                    }
                    else
                    {
                        _return = true;
                    }

                    _rayHits = Physics2D.RaycastAll(_rayPos, _rayDir, 10000, m_reflectionLayers);
                    _currentHit = GetFirstValidTransform(_rayHits, _currentHit);
                },
                // Predicate
                () =>
                {
                    return !_return;
                }, (uint)m_reflectionResolution, false);

            return _targetY;
        }

        protected bool ValidDirection(Vector2 dir)
        {
            return (m_playerID == PlayerID.PLAYER_ONE) ? (dir.x <= 0.05f) : (dir.x >= -0.95f);
        }
        protected float CalculateTarget()
        {
            float _target = m_targetPos.y;

            Vector3 _ballPos = Ball.Instance.transform.position;

            Vector2 _ballVel = Ball.Instance.Velocity;
            bool _validDir = ValidDirection(_ballVel);

            if (_validDir)
            {
                _target = TestReflection(_ballPos, _ballVel);
            }
            else 
            {
                Vector2 _ballDisplayedVel = Ball.Instance.DisplayedVelocity;
                bool _validDisplayedDir = ValidDirection(_ballDisplayedVel);

                if (_validDisplayedDir)
                {
                    _target = TestReflection(_ballPos, _ballDisplayedVel);
                }
                else if (m_resetPaddleToCentre)
                {
                    _target = ((Mathf.Abs(m_verticalBounds.x) + Mathf.Abs(m_verticalBounds.y)) / 2) - Mathf.Abs(m_verticalBounds.x);
                }
            }

            return _target;
        }

        protected override void Move()
        {
           // m_targetPos.y = m_resetPaddleToCentre ? 0 : transform.position.y;
            m_targetPos.y = Mathf.Clamp(m_targetPos.y, m_verticalBounds.x, m_verticalBounds.y);

            Vector3 _target = transform.position;
            _target.y = m_targetPos.y;
            transform.position = Vector3.MoveTowards(transform.position, _target, m_moveSpeed * Time.deltaTime);

            //transform.position = new Vector3(transform.position.x, Mathf.Lerp(transform.position.y, m_targetPos.y, m_moveSpeed * Time.deltaTime), 0);
            //transform.position = Vector3.Lerp(transform.position, _target, m_moveSpeed * Time.deltaTime);
        }
    }
}