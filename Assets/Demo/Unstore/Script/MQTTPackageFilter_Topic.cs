using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MQTTPackageFilter_Topic : MonoBehaviour
{
    public List<string> m_specificTopic;
    public OnPackageReceivedEvent m_onPackageReceived;

    public void PackageReceived(MQTT_UserPackageReceived package)
    {
        for (int i = 0; i < m_specificTopic.Count; i++)
        {
            if (package.m_topic == m_specificTopic[i])
            {
                m_onPackageReceived.Invoke(package);
            }

        }
    }
}