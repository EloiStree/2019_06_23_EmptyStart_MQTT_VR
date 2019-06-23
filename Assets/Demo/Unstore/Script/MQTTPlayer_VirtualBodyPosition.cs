using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MQTTPlayer_VirtualBodyPosition : MQTTPlayerInfoRegularSender
{
    public SpaceTrackedPointFromTransform m_head;
    public SpaceTrackedPointFromTransform m_leftHand;
    public SpaceTrackedPointFromTransform m_rightHand;
    public VirtualBodyPosition m_lastStatePush;

    public override string ToSendEveryNowAndThen()
    {
        m_lastStatePush.m_head = m_head.GetSpaceTrackedPoint();
        m_lastStatePush.m_leftHand = m_leftHand.GetSpaceTrackedPoint();
        m_lastStatePush.m_rightHand = m_rightHand.GetSpaceTrackedPoint();
        return JsonUtility.ToJson(m_lastStatePush);
    }
}

[System.Serializable]
public class VirtualBodyPosition {
    public SpaceTrackedPoint m_head;
    public SpaceTrackedPoint m_leftHand;
    public SpaceTrackedPoint m_rightHand;
}
