using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MQTTPackageFilter_UserID : MonoBehaviour
{
    public List<string> m_specificPlayer;
    public OnPackageReceivedEvent m_onPackageReceived;

    public void PackageReceived ( MQTT_UserPackageReceived package)
    {
        for (int i = 0; i < m_specificPlayer.Count; i++)
        {
            if (package.m_userDeviceId == m_specificPlayer[i])
            {
                m_onPackageReceived.Invoke(package);
            }

        }
    }
}
