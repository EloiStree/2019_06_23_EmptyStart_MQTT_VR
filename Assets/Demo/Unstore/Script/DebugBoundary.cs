using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugBoundary : MonoBehaviour
{
    public OVRBoundaryInfo m_info;
    public float m_drawTime = 5f;
    public void CheckOfPacakge(MQTT_UserPackageReceived package)
    {
        m_info = JsonUtility.FromJson<OVRBoundaryInfo>(package.m_message);


        DrawLineOf(m_info.boundaryGeometryArea, Color.green);
        DrawLineOf(m_info.boundaryGeometryOuter, Color.yellow);

    }

    private void DrawLineOf(Vector3[] lines, Color color )
    {
        if (lines.Length > 1)
            for (int i = 1; i < lines.Length; i++)
            {
                Debug.DrawLine(lines[i - 1], lines[i], color, m_drawTime);


            }
    }
}
