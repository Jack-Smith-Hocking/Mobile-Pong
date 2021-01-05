using System.Collections;
using System.Collections.Generic;
using Custom.Utility;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class SliderEvent
{
    public UnityEvent m_event;
    public FloatRange m_range;
}

public class SliderEvents : MonoBehaviour
{
    public float m_sliderVal = 0;
    [Space]
    public List<SliderEvent> m_sliderEvents = new List<SliderEvent>();

    // Start is called before the first frame update
    void Start()
    {
        m_sliderEvents.Sort((a, b) => { return a.m_range.MinVal.CompareTo(b.m_range.MinVal); });
    }

    public void SetSliderVal(float sliderVal)
    {
        m_sliderVal = sliderVal;
        UpdateSliderState();
    }
    public void UpdateSliderState()
    {
        foreach (var se in m_sliderEvents)
        {
            if (se.m_range.WithinRange(m_sliderVal))
            {
                se.m_event.Invoke();
            }
        }
    }
}
