using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MQTTDebug_Topic : MonoBehaviour
{
    public InputField m_topic;
    public InputField m_display;

    public void RefreshWith(MQTT_UserPackageReceived package) {
        if (package.m_topic == m_topic.text) {
            m_display.text = package.m_message;
        }
    }
}
