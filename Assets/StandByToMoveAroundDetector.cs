using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class StandByToMoveAroundDetector : MonoBehaviour
{
    public HandsMovementDetection m_handsMovement;
    public HandsPosition m_hands;
    public float m_moveOutDistance = 0.05f;
    public bool m_positionLocked;
    public OnPositionLock m_onPositionlock;
    public OnMoveOut m_onMoveOut;

    public void Awake()
    {
        m_handsMovement.m_onStopMoving.AddListener(LockPosition);
    }
    public void LockPosition()
    {
        if(!m_positionLocked){
            m_positionLocked = true;
            m_hands = GetHandsInfoFrom(m_handsMovement);
            m_onPositionlock.Invoke(m_hands);
        }
    }
    public void Unlock() {
        m_onMoveOut.Invoke(m_hands);
        m_positionLocked = false;
    }
    public void Update()
    {
        if (m_positionLocked)
        {
            if (m_moveOutDistance < Vector3.Distance(m_hands.m_left.m_position, m_handsMovement.m_leftHand.m_hand.position)
            && m_moveOutDistance < Vector3.Distance(m_hands.m_right.m_position, m_handsMovement.m_rightHand.m_hand.position))
            {
                Unlock();
            }
        }
    }
    private HandsPosition GetHandsInfoFrom(HandsMovementDetection hands)
    {
        HandsPosition result = new HandsPosition();
        result.m_left.m_position = hands.m_leftHand.m_hand.position;
        result.m_right.m_position = hands.m_rightHand.m_hand.position;
        result.m_left.m_rotation = hands.m_leftHand.m_hand.rotation;
        result.m_right.m_rotation = hands.m_rightHand.m_hand.rotation;
        return result;
    }
}
[System.Serializable]
public struct HandsPosition {
    public HandPosition m_left;
    public HandPosition m_right;
}
[System.Serializable]

public struct HandPosition
{
    public Vector3 m_position;
    public Quaternion m_rotation;
}
[System.Serializable]
public class OnPositionLock : UnityEvent<HandsPosition> { }
[System.Serializable]
public class OnMoveOut : UnityEvent<HandsPosition> { }

