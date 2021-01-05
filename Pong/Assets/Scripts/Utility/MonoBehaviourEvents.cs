using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MonoBehaviourEvents : MonoBehaviour
{
    public UnityEvent m_onAwake;
    public UnityEvent m_onStart;
    public UnityEvent m_onUpdate;

    private void Awake()
    {
        m_onAwake.Invoke();
    }
    // Start is called before the first frame update
    void Start()
    {
        m_onStart.Invoke();
    }

    // Update is called once per frame
    void Update()
    {
        m_onUpdate.Invoke();
    }
}
