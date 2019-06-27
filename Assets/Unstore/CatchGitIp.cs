using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatchGitIp : MonoBehaviour
{
    public UserMQTT m_user;
    public string m_serverUrl= "https://raw.githubusercontent.com/EloiStree/EmptyStart_MQTT_VR/master/MQTTServer";
    public string m_ip;
    public int m_port;
    public string m_error;
    public string m_text;
    void Awake()
    {
       
                StartCoroutine(
            TryToAccessIPData());
        
    }

    public IEnumerator TryToAccessIPData()
    {
       
            WWW www = new WWW(m_serverUrl);
            yield return www;
        m_error = www.error;
        m_text = www.text;
    try
    {

        if (string.IsNullOrEmpty(www.error))
            {

                string text = www.text;
                string[] lines = text.Split('\n');
                if (lines.Length >= 2)
                {
                    string ip = lines[0];
                    int port = int.Parse(lines[1]);
                m_ip = ip;
                m_port = port;
                }
            }
        }
        catch (Exception)
        {
        }
        if (string.IsNullOrEmpty(m_ip)) {
          m_ip =   PlayerPrefs.GetString("MQTTSERVERIP");
          m_port =   PlayerPrefs.GetInt("MQTTSERVERPORT");
        }

        if (!string.IsNullOrEmpty(m_ip))
        {
            m_user.m_serverIp = m_ip;
            m_user.m_serverPort = m_port;
            PlayerPrefs.SetString("MQTTSERVERIP", m_ip);
            PlayerPrefs.SetInt("MQTTSERVERPORT", m_port);
        }
        m_user.StartMQTT();
    }
    
}
