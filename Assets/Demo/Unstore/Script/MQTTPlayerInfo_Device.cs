using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class MQTTPlayerInfo_Device : MQTTPlayerInfo
{
    public string topic = "playerinfo/device";
    void Start()
    {
        UserDeviceInfo deviceInfo = new UserDeviceInfo();
        deviceInfo.m_deviceId = SystemInfo.deviceUniqueIdentifier;
        deviceInfo.m_screenHeight = Screen.height;
        deviceInfo.m_screenWidth = Screen.width;
        deviceInfo.m_targetPlatform = Application.platform;
        deviceInfo.m_headset.m_virtualRealityType = GetVirtualRealityDeviceType();
        deviceInfo.m_headset.m_supportedDevices = XRSettings.supportedDevices;
        deviceInfo.m_headset.loadedDeviceName = XRSettings.loadedDeviceName;
        deviceInfo.m_headset.isActive = XRSettings.isDeviceActive;
        deviceInfo.m_headset.isEnable = XRSettings.enabled;
        deviceInfo.m_headset.eyeTextureWidth = XRSettings.eyeTextureWidth;
        deviceInfo.m_headset.eyeTextureHeight = XRSettings.eyeTextureHeight;

        string json = JsonUtility.ToJson(deviceInfo);
    }
    

    private VRDeviceType GetVirtualRealityDeviceType()
    {
        return VRDeviceType.Unkown;
    }
}
[System.Serializable]
public struct VirtualRealtyDeviceInfo
{
    //https://docs.unity3d.com/Manual/VROverview.html

    public VRDeviceType m_virtualRealityType;
    public string m_virtualRealityRuntime;
    public string[] m_supportedDevices;
    public string loadedDeviceName;
    internal int eyeTextureWidth;
    internal int eyeTextureHeight;
    internal bool isEnable;
    internal bool isActive;
}

[System.Serializable]
public struct UserDeviceInfo {
    public string m_deviceId;
    public float m_screenWidth;
    public float m_screenHeight;
    public RuntimePlatform m_targetPlatform;
    public VirtualRealtyDeviceInfo m_headset;

}


public enum VRDeviceType
{
    OculusGo,
    OculusQuest,
    GearVR_DK1,
    GearVR_CV1,
    GearVR_CV2,
    OculusRift,
    OculusS,
    HTC_Vive,
    Other,
    Unkown
}
public struct UserDeviceInfoAfterByTime {
    

}
