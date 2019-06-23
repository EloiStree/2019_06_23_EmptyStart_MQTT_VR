using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestTracker : MonoBehaviour
{
    public int m_tackerCount;
    public int m_webcamCount  ;
    public Texture m_image;
    public Renderer m_renderer;
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {

        m_tackerCount = OVRManager.tracker.count;
        WebCamDevice[] devices = WebCamTexture.devices;
        WebCamTexture webcamTexture = new WebCamTexture();
        m_webcamCount = devices.Length;
        if (devices.Length > 0)
        {

            webcamTexture.deviceName = devices[0].name;
            webcamTexture.Play();
            m_image = webcamTexture;
            m_renderer.material.mainTexture = webcamTexture;
        }
    }
}
