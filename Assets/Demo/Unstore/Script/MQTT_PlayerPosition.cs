using UnityEngine;
using System.Net;
using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;
using System;
using System.Collections;
using System.Collections.Generic;

public class MQTT_PlayerPosition : MonoBehaviour
{
    public string m_ipAddress = "143.185.118.233";
    public int m_port = 8080;
    public int m_playerIndex;
    private MqttClient client;

    public Transform m_headPosition;
    public float m_timeBetweenRefresh=0.1f;

    public Queue<string> m_receivedCommand = new Queue<string>();
    public PositionPackage m_stateOfPlayer;


    [System.Serializable]
    public class PositionPackage {
        public int playerIndex;
        public float time;
        public Vector3 position;
    }

    void Start()
    {
        client = new MqttClient(IPAddress.Parse(m_ipAddress), m_port, false, null);
        client.MqttMsgPublishReceived += client_MqttMsgPublishReceived;
        string clientId = Guid.NewGuid().ToString();
        client.Connect(clientId);
        client.Subscribe(new string[] { "player" }, new byte[] { MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE });

        m_stateOfPlayer.playerIndex = m_playerIndex;

        StartCoroutine(SendPosition());
    }
    void client_MqttMsgPublishReceived(object sender, MqttMsgPublishEventArgs e)
    {
        m_receivedCommand.Enqueue(System.Text.Encoding.UTF8.GetString(e.Message));
        Debug.Log("Received: " + System.Text.Encoding.UTF8.GetString(e.Message));
    }
    IEnumerator SendPosition() {
        while (true) {
            yield return new WaitForEndOfFrame();
            yield return new WaitForSeconds(m_timeBetweenRefresh);

            client.Publish("player", System.Text.Encoding.UTF8.GetBytes(GetJsonHeadPosition()), MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE, true);
            
        }
    }

    public void Update()
    {
        m_stateOfPlayer.time = Time.time;
        m_stateOfPlayer.position = transform.position;

        while (m_receivedCommand.Count > 0)
        {
            string value = m_receivedCommand.Dequeue();
            PositionPackage position = JsonUtility.FromJson<PositionPackage>(value);
            if (m_stateOfPlayer.playerIndex != m_playerIndex)
            {
                m_stateOfPlayer = position;

            }
        }
    }

    private string GetJsonHeadPosition()
    {
        return JsonUtility.ToJson(m_stateOfPlayer);
    }
   
}
