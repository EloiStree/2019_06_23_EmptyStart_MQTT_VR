using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class HandsMovementDetection : MonoBehaviour
{
    public HandInfo m_leftHand;
    public HandInfo m_rightHand;
    public float m_notMovingSince;
    public float m_timeToBeConsiderNotMoving = 1f;

    [Header("Debug")]
    public bool m_isDetectedAsNotMoving;

    public UnityEvent m_onStopMoving;
    public UnityEvent m_onRestartMoving;

    [System.Serializable]
    public class HandInfo
    {
        public Transform m_hand;
        public Vector3 m_lastPosition;
        public float m_precisionMeter = 0.01f;
        public bool m_areNotMoving;
        public bool IsNotMoving()
        {
            return m_areNotMoving = Vector3.Distance(m_hand.position, m_lastPosition) < m_precisionMeter;
        }
    }

    internal bool IsInSameDirectionOnY(int angle =5)
    {
        return Vector3.Angle(m_leftHand.m_hand.up, m_rightHand.m_hand.up)<angle;
    }

    public enum HandType { Left, Right }
    public bool IsNotMoving(HandType handType)
    {
        return handType == HandType.Left ? m_leftHand.IsNotMoving() : m_rightHand.IsNotMoving();
    }

    public void Awake()
    {
        InvokeRepeating("CheckMovement", 0, 0.1f);
    }
    public void CheckMovement()
    {
        m_leftHand.m_lastPosition  = m_leftHand.m_hand.position;
        m_rightHand.m_lastPosition = m_rightHand.m_hand.position;
    }
    void Update()
    {
        if (IsNotMoving(HandType.Left) && IsNotMoving(HandType.Right))
        {
            float oldValue = m_notMovingSince;
            m_notMovingSince += Time.deltaTime;
            if ((m_notMovingSince > m_timeToBeConsiderNotMoving) && (oldValue <= m_timeToBeConsiderNotMoving))
            {
                m_isDetectedAsNotMoving = true;
                m_onStopMoving.Invoke();
            }
        }
        else {

            m_notMovingSince = 0;
            if (m_isDetectedAsNotMoving) {
                m_isDetectedAsNotMoving = false;
                m_onRestartMoving.Invoke();
            }

        }
    }
}
