using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MQTTPlayer_VirtualBodyDisplayer : MonoBehaviour
{
    public string linkedUserId;
    public GameObject m_head;
    public GameObject m_leftHand;
    public GameObject m_rightHand;
    public MQTT_UserPackageReceived m_last;
    public VirtualBodyPosition m_lastBodyPosition;
    public float m_latency;
    public bool m_useLerp;

    public void ReceivedMDTTPackage(MQTT_UserPackageReceived package) {

        if(package.m_userDeviceId== linkedUserId)
            if (package.GetTimeWhenSendAsTimestamp() >= m_last.GetTimeWhenSendAsTimestamp()){
                m_last = package;
                m_latency = m_last.GetLatency();
                m_lastBodyPosition = JsonUtility.FromJson<VirtualBodyPosition>(package.m_message);
            
            }

    }
    public void Update()
    {

        SetWith(m_lastBodyPosition);
    }

    void SetWith(VirtualBodyPosition value)
    {
        if (m_useLerp)
        {
            Lerp(m_head, value.m_head);
            Lerp(m_leftHand, value.m_leftHand);
            Lerp(m_rightHand, value.m_rightHand);

        }
        else {
            m_head.transform.position = value.m_head.m_unityPointOfView.m_position;
            m_head.transform.rotation = value.m_head.m_unityPointOfView.m_rotation;

            m_leftHand.transform.position = value.m_leftHand.m_unityPointOfView.m_position;
            m_leftHand.transform.rotation = value.m_leftHand.m_unityPointOfView.m_rotation;

            m_rightHand.transform.position = value.m_rightHand.m_unityPointOfView.m_position;
            m_rightHand.transform.rotation = value.m_rightHand.m_unityPointOfView.m_rotation;
        }
    }

    private void Lerp(GameObject target,  SpaceTrackedPoint value)
    {
        target.transform.position = Vector3.Lerp(target.transform.position, value.m_unityPointOfView.m_position, Time.deltaTime* m_lerpFactor);
        target.transform.rotation = Quaternion.Lerp(target.transform.rotation, value.m_unityPointOfView.m_rotation, Time.deltaTime* m_lerpFactor);
    }
    public float m_lerpFactor=5;
}
