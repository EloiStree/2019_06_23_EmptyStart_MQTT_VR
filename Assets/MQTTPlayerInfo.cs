using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MQTTPlayerInfo :MonoBehaviour{

    [SerializeField] protected UserMQTT m_userInfo;
}

public abstract class MQTTPlayerInfoRegularSender : MQTTPlayerInfo
{
    public float m_timeBetweenPackage = 0.1f;
    public string topic= "player/info";
    public void Start()
    {
        StartCoroutine(SendInfo());
        m_userInfo.AddTopicToListen(topic);

    }

    private IEnumerator SendInfo()
    {
        while (true) {
            yield return new WaitForEndOfFrame();
            yield return new WaitForSeconds(m_timeBetweenPackage);
            string msg = ToSendEveryNowAndThen();
            m_userInfo.Send(topic, msg);
           
        }
    }
    public abstract string ToSendEveryNowAndThen();

    public void SendInfo(string topic, string msg) {
        if (!string.IsNullOrEmpty(msg))
            m_userInfo.Send(topic, msg);
    }
}
