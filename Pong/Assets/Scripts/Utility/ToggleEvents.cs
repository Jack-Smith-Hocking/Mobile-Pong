using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ToggleEvents : MonoBehaviour
{
    public bool m_toggleState = false;
    [Space]
    public UnityEvent m_onEvent;
    public UnityEvent m_offEvent;

    void Start()
    {
        UpdateToggleState();
    }

    public void SetToggleState(bool state)
    {
        m_toggleState = state;
        UpdateToggleState();
    }
    public void ToggleState()
    {
        m_toggleState = !m_toggleState;
        UpdateToggleState();
    }

    void UpdateToggleState()
    {
        if (m_toggleState) m_onEvent.Invoke();
        else m_offEvent.Invoke();
    }
}
