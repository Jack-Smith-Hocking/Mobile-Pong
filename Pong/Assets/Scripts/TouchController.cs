using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TouchController : MonoBehaviour
{
    public Transform m_centrePoint;
    public Text m_deltaText;
    public float m_startMultiplier = 0.1f;
    public float m_movementTime = 0.5f;

    private Vector2 m_delta;
    private Vector2 m_mouseStart;
    private Vector2 m_mouseDir;

    private Vector3 m_startPosition;

    private float m_currentMultiplier = 0;
    private float m_targetSize = 0;
    private float m_startCamSize = 0;
    private float m_endMoveTime = 0;

    private bool m_isRotating = false;

    // Start is called before the first frame update
    void Start()
    {
        Input.simulateMouseWithTouches = true;
        Input.multiTouchEnabled = true;

        m_startPosition = transform.position;

        {
            m_startCamSize = (transform.position - m_centrePoint.position).magnitude;
            m_targetSize = m_startCamSize;
            transform.position = m_centrePoint.position - (transform.forward * m_targetSize);

            transform.LookAt(m_centrePoint);
        }

        UpdateMultiplier(1);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(m_centrePoint.position, m_targetSize);
    }

    // Update is called once per frame
    private void Update()
    {
        if (Application.isMobilePlatform)
        {
            TouchUpdate();
        }
        else
        {
            StartCoroutine(MouseUpdate());
        }

        StartCoroutine(Rotate());

        if (Time.time > m_endMoveTime)
        {
            m_isRotating = false;
        }

        Vector3 _targetPos = m_centrePoint.position - (transform.forward * m_targetSize);
        transform.position = Vector3.MoveTowards(transform.position, _targetPos, m_currentMultiplier * 50 * Time.deltaTime);

        transform.LookAt(m_centrePoint);
    }

    IEnumerator Rotate()
    {
        if (m_isRotating)
        {
            m_deltaText.text = $"Move Delta: ({(m_delta.x * 100).ToString("F1")}, {(-m_delta.y * 100).ToString("F1")})";

            transform.RotateAround(m_centrePoint.position, Vector3.right, -m_delta.y);

            yield return new WaitForEndOfFrame();
            
            transform.RotateAround(m_centrePoint.position, Vector3.up, m_delta.x);
        }
    }

    IEnumerator MouseUpdate()
    {
        if (EventSystem.current.currentSelectedGameObject != null) yield return null;

        float _currentDist = (transform.position - m_centrePoint.position).magnitude;

        if (Input.GetMouseButtonDown(0))
        {
            m_mouseStart = Input.mousePosition;
        }

        if (Input.GetMouseButtonUp(0))
        {
            m_endMoveTime = Time.time + m_movementTime;
        }

        if (!ScrollZoom() && Input.GetMouseButton(0))
        {
            yield return new WaitForEndOfFrame();

            m_mouseDir = (Input.mousePosition - m_startPosition).normalized;

            if (m_mouseDir.magnitude >= 0.1f)
            {
                m_delta = m_mouseDir * m_currentMultiplier * Time.deltaTime;
            }

            m_isRotating = true;

            m_mouseStart = Input.mousePosition;
        }
    }

    void TouchUpdate()
    {
        if (EventSystem.current.currentSelectedGameObject != null) return;

        float _currentDist = (transform.position - m_centrePoint.position).magnitude;

        if (Input.touchCount > 0 && !TouchZoom())
        {
            m_endMoveTime = Time.time + m_movementTime;

            m_delta = Input.GetTouch(0).deltaPosition * m_currentMultiplier * Time.deltaTime;

            m_isRotating = true;
        }
    }

    public bool ScrollZoom()
    {
        Vector2 _scrollDelta = Input.mouseScrollDelta;

        if (Mathf.Abs(_scrollDelta.y) > 0.01f)
        {
            m_targetSize += _scrollDelta.y * m_startMultiplier * -1;

            return true;
        }

        return false;
    }

    public bool TouchZoom()
    {
        if (Input.touchCount == 2)
        {
            if (Input.GetTouch(1).phase == TouchPhase.Moved)
            {
                if (Vector2.Dot(Input.GetTouch(1).deltaPosition, Vector2.left) >= 0.5f)
                {
                    m_targetSize += 0.25f * m_startMultiplier;
                }
                else
                {
                    m_targetSize -= 0.25f * m_startMultiplier;
                }

                return true;
            }
        }

        return false;
    }

    public void UpdateMultiplier(float newMulti)
    {
        m_currentMultiplier = m_startMultiplier * newMulti;
    }

    public void ResetPosition()
    {
        transform.position = m_startPosition;
    }

    public void ResetSize()
    {
        m_targetSize = m_startCamSize;
    }
}
