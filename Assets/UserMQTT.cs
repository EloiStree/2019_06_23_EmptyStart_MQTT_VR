using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net;
using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;
using System;
using UnityEngine.Events;

public class UserMQTT : MonoBehaviour
{
    public string m_deviceId;

    [Header("MQTT")]
    public string m_serverIp = "143.185.118.233";
    public int m_serverPort = 8080;
    public List<string> m_listenedTopics ;
    private MqttClient client;
    public bool m_useDebug = false;

    public float m_timeSincePlaying;
    public float m_bytesSend;

    internal void AddTopicToListen(string topic)
    {
        //m_listenedTopics.Add(topic);
        //client.Subscribe(m_listenedTopics.ToArray(), new byte[] { MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE });
    }

    public OnPackageReceivedEvent m_debugPackage;

    public Queue<MQTT_UserPackageReceived> m_receivedByThread = new Queue<MQTT_UserPackageReceived>();

     void Awake()
    {
        m_deviceId = GetUserId();
        client = new MqttClient(IPAddress.Parse(m_serverIp), m_serverPort, false, null);
        client.MqttMsgPublishReceived += client_MqttMsgPublishReceived;
        string clientId = Guid.NewGuid().ToString();
        client.Connect(clientId);
        client.Subscribe(m_listenedTopics.ToArray(), new byte[] { MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE });

    }

 
    void client_MqttMsgPublishReceived(object sender, MqttMsgPublishEventArgs e)
    {
        MqttClient client = (MqttClient)sender;
       
        string message = System.Text.Encoding.UTF8.GetString(e.Message);
        if (message.IndexOf("<HeyMonAmi>") == 0) {
            int startPackageInfo = "<HeyMonAmi>".Length;
            int endPackageInfo = message.IndexOf("<HeyMonAmi/>");
            int messageStart = endPackageInfo + "<HeyMonAmi/>".Length;

            string packageInfo = message.Substring(startPackageInfo, endPackageInfo);
            string[] infoToken = packageInfo.Split('|');
            MQTT_UserPackageReceived info = new MQTT_UserPackageReceived();
            info.m_userDeviceId = infoToken[0];
            info.m_timestampWhenSend = infoToken[1];
            info.m_timestampWhenReceived =""+ GetTime();
            info.m_message = message.Substring(messageStart);
            info.m_topic = e.Topic;
            Debug.Log("Sender: "+info.m_userDeviceId +" ?? " + sender.ToString());
            Debug.Log("Received: " + info.m_message);
            Debug.Log("ID: : " + client.ClientId);
            m_receivedByThread.Enqueue(info);
        }
    }
    private void Update()
    {
        while (m_receivedByThread.Count > 0) {
            MQTT_UserPackageReceived info = m_receivedByThread.Dequeue();
            m_debugPackage.Invoke(info);
        }
        m_timeSincePlaying += Time.deltaTime;
    }

    public void Send(string topic, string message)
    {
        string oneLineInfo = "<HeyMonAmi>" + GetUserId() + "|" + GetTime() + "<HeyMonAmi/>";
        byte[] toSend = System.Text.Encoding.UTF8.GetBytes(oneLineInfo + message);

        m_bytesSend += toSend.Length;
        client.Publish(topic,toSend , MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE, true);
        if (m_useDebug)
        {
            m_bytesSend += toSend.Length;
            client.Publish("debug/all", toSend, MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE, true);
        }
    }

    private static long GetTime()
    {
        return DateTime.Now.ToFileTime();
    }

    public static string GetUserId() {
        return SystemInfo.deviceUniqueIdentifier;
    }

    
}
public struct MQTT_UserPackageReceived
{
    public string m_timestampWhenSend;
    public string m_timestampWhenReceived;
    public string m_userDeviceId;
    public string m_topic;
    public string m_message;
}
[System.Serializable]
public class OnPackageReceivedEvent : UnityEvent<MQTT_UserPackageReceived> { };