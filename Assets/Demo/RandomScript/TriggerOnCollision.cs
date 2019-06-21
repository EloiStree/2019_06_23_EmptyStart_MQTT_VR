using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TriggerOnCollision : MonoBehaviour
{
    public UnityEvent m_on;
    public UnityEvent m_off;
    public UnityEvent m_onChange;

    public bool m_onOff;

    private void OnCollisionEnter(Collision collision)
    {
        m_onOff = !m_onOff;

        if (m_onOff)
            m_on.Invoke();
        else
            m_off.Invoke();
        m_onChange.Invoke();



    }
}
