using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MQTTPlayer_VirtualBodyDisplayer : MonoBehaviour
{
    public string linkedUserId;
    public GameObject m_head;
    public GameObject m_leftHand;
    public GameObject m_rightHand;
    public GameObject m_headWithLerp;
    public GameObject m_leftHandWithLerp;
    public GameObject m_rightHandWithLerp;
    public MQTT_UserPackageReceived m_last;
    public OVRCompressedInfo m_lastBodyPosition;
    public float m_latency;

    public void ReceivedMDTTPackage(MQTT_UserPackageReceived package)
    {

        if (package.m_userDeviceId == linkedUserId)
            if (package.GetTimeWhenSendAsTimestamp() >= m_last.GetTimeWhenSendAsTimestamp())
            {
                m_last = package;
                m_latency = m_last.GetLatency();

                
                m_lastBodyPosition = OVRCompressedInfo.TryParse(package.m_message);
            
            }

    }
    public void Update()
    {

        SetWith(m_lastBodyPosition);
    }

    void SetWith(OVRCompressedInfo value)
    {
        if (value == null)
            return;
        Lerp(m_headWithLerp, value.h);
        Lerp(m_leftHandWithLerp, value.l);
        Lerp(m_rightHandWithLerp, value.r);

        m_head.transform.position = value.h.GetPosition();
        m_head.transform.rotation = value.h.GetRotation();

        m_leftHand.transform.position = value.l.GetPosition();
        m_leftHand.transform.rotation = value.l.GetRotation();

        m_rightHand.transform.position = value.r.GetPosition();
        m_rightHand.transform.rotation = value.r.GetRotation();

    }

    private void Lerp(GameObject target, CompressedPosition value)
    {
        if (target == null || value == null)
            return;
        Quaternion q =value.GetRotation();
        if (q.w == 0)
            q = Quaternion.identity;
        target.transform.position = Vector3.Lerp(target.transform.position, value.GetPosition(), Time.deltaTime * m_lerpFactor);
        target.transform.rotation = Quaternion.Lerp(target.transform.rotation, q, Time.deltaTime * m_lerpFactor);
    }
    public float m_lerpFactor = 5;
}
