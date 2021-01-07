using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Custom.Utility.Mobile
{
    public class TouchEvent : MonoBehaviour
    {
        public UnityEvent m_onMainPress;

        public void Start()
        {
            TouchInputWrapper.Instance.m_mainInputActivated += () => { m_onMainPress.Invoke(); };
        }
    }
}