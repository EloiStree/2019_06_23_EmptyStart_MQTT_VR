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
    public int m_serverPort = 1883;
    public List<string> m_listenedTopics ;
    private MqttClient client;
    public bool m_useDebug = false;
    public bool m_useDebugLog = false;

    public float m_timeSincePlaying;
    public float m_bytesSend;

    public Dictionary<string, MQTT_UserPackageReceived> m_lastPackage = new Dictionary<string, MQTT_UserPackageReceived>();
    public Dictionary<string, bool> m_lastPackageReceivedCheck = new Dictionary<string, bool>();
    public Dictionary<string, string> m_userDetectedOnceRegister = new Dictionary<string, string>();


    [Header("Debug info")]
    public List<string> m_userDetectedOnce;
    public List<string> m_userByTopic;

    public OnNewUserDetected m_newUser;
    public OnPackageReceivedEvent m_debugPackage;
    internal void AddTopicToListen(string topic)
    {
        //m_listenedTopics.Add(topic);
        //client.Subscribe(m_listenedTopics.ToArray(), new byte[] { MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE });
    }
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

            string packageInfo = message.Substring(startPackageInfo,endPackageInfo- startPackageInfo);
            string[] infoToken = packageInfo.Split('|');
            string userId = infoToken[0];
            string timeWhenSend=infoToken[1];
            string packageKey = e.Topic + "|" + userId;
            if (m_lastPackage.ContainsKey(packageKey))
            {
                if (m_lastPackage[packageKey].IsPackageMoreRecentThat(timeWhenSend)) {
                    return;
                }
            }
            else m_lastPackage[packageKey] = new MQTT_UserPackageReceived();

            if (! m_userDetectedOnceRegister.ContainsKey(userId)) {
                m_newUser.Invoke(userId);
            }
            m_userDetectedOnceRegister[userId] = userId;
            MQTT_UserPackageReceived info = m_lastPackage[packageKey];
            info.m_userDeviceId = userId;
            info.m_timestampWhenSend = timeWhenSend;
            info.m_timestampWhenReceived =""+ GetTime();
            info.m_message = message.Substring(messageStart);
            info.m_topic = e.Topic;
            if (m_useDebugLog) {

                Debug.Log("Sender: "+info.m_userDeviceId +" ?? " + sender.ToString());
                Debug.Log("Received: " + info.m_message);
                Debug.Log("ID: : " + client.ClientId);
            }
           // m_lastPackage[packageKey] = info;
            m_lastPackageReceivedCheck[packageKey] = true;
        }
    }
    private void Update()
    {

        m_userDetectedOnce = new List<string>(this.m_userDetectedOnceRegister.Keys);

        List<string> keyList = new List<string>(this.m_lastPackageReceivedCheck.Keys);
        m_userByTopic = keyList;
        foreach (string key in keyList)
        {
            if (m_lastPackageReceivedCheck[key]) {
                m_lastPackageReceivedCheck[key] = false;
                m_debugPackage.Invoke(m_lastPackage[key]);
            }
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
[System.Serializable]
public class MQTT_UserPackageReceived
{
    public string m_timestampWhenSend;
    public string m_timestampWhenReceived;
    public string m_userDeviceId;
    public string m_topic;
    public string m_message;

   

    internal float GetLatency()
    {
        return (GetTimeWhenReceivedAsTimestamp() - GetTimeWhenSendAsTimestamp())/10000000f;
    }
    public string GetUserDeviceId() {
        return m_userDeviceId;
    }
    internal long GetTimeWhenReceivedAsTimestamp()
    {
        try
        {
            return long.Parse(m_timestampWhenReceived);
        }catch (Exception) { return 0; }
    }
    internal long GetTimeWhenSendAsTimestamp()
    {
        try {
            return long.Parse(m_timestampWhenSend);
        }
        catch (Exception) { return 0; }
    }

    internal bool IsNotDefined()
    {
        return m_timestampWhenSend == "" || m_timestampWhenReceived == "";
    }

    internal bool IsPackageOlderThat(string timeWhenSend)
    {
        try
        {
            long currentPackage = long.Parse(m_timestampWhenSend);
            long givenPackage = long.Parse(timeWhenSend);
            return currentPackage < givenPackage;
        }
        catch (Exception) { return false; }
    }

    internal bool IsPackageMoreRecentThat(string timeWhenSend)
    {
        try
        {
            long currentPackage = long.Parse(m_timestampWhenSend);
            long givenPackage = long.Parse(timeWhenSend);
            return currentPackage > givenPackage;
        }
        catch (Exception) { return false; }
    }
}


[System.Serializable]
public class OnNewUserDetected : UnityEvent<string> { };

[System.Serializable]
public class OnPackageReceivedEvent : UnityEvent<MQTT_UserPackageReceived> { };