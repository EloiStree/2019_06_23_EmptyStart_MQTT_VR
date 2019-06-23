using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MQTTPlayer_VirtualBodyGenerator : MonoBehaviour
{
    public GameObject m_displayerPrefab;
    public List<MQTTPlayer_VirtualBodyDisplayer> m_playersBody;

    public void NewPackageReceived(MQTT_UserPackageReceived package) {
        foreach (var body in m_playersBody)
        {
            if(package.m_userDeviceId == body.linkedUserId)
                  body.ReceivedMDTTPackage(package);
        }
    }

    public void CreateNewPlayer(string userId) {
        GameObject newPlayer = Instantiate(m_displayerPrefab);
        newPlayer.transform.parent = this.transform;
        MQTTPlayer_VirtualBodyDisplayer body = newPlayer.GetComponent<MQTTPlayer_VirtualBodyDisplayer>();
        body.linkedUserId = userId;
        m_playersBody.Add(body);
    }

    public void OnValidate()
    {
        if (m_displayerPrefab.GetComponent<MQTTPlayer_VirtualBodyDisplayer>() == null)
        {
            m_displayerPrefab = null;
            Debug.LogWarning("Prefab Displayer must own a virtual body displayer");

        }

    }
}
