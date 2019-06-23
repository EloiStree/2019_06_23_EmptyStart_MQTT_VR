using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestWithBoundary : MQTTPlayerInfoRegularSender
{

    public OVRBoundaryInfo m_boundaryInfo;

    public override string ToSendEveryNowAndThen()
    {
        return JsonUtility.ToJson(m_boundaryInfo);
    }

    void Update()
    {
        m_boundaryInfo = TestWithTracker.GetBoundaryInfo();
    }
}
