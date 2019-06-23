using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MQTTDebugMono : MonoBehaviour
{
    public UserMQTT m_user;
    public string m_topic= "player/ovrboundary";
    public List<string> m_specificPlayer;

    public string m_userId;
    [TextArea(0,200)]
    public string m_message;

    void Start()
    {
        m_user.m_debugPackage.AddListener(CheckOfPacakge);
    }

    private void CheckOfPacakge(MQTT_UserPackageReceived package)
    {
        for (int i = 0; i < m_specificPlayer.Count; i++)
        {
            if (package.m_userDeviceId == m_specificPlayer[i])
            {
                if (package.m_topic == m_topic)
                {
                    m_userId = package.m_userDeviceId;
                    m_message = package.m_message;
                }
            }

        }
    }
    
}
