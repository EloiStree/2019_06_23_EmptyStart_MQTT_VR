using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugTracker : MonoBehaviour
{
    public OVRManagerInfo m_info;
    public float m_drawTime = 5f;
    public void CheckOfPacakge(MQTT_UserPackageReceived package)
    {
        m_info = JsonUtility.FromJson<OVRManagerInfo>(package.m_message);
        DrawLineOf(m_info.boundary.boundaryGeometryArea, Color.green,  m_info.headAndHands.m_rootUnityPosition);
        DrawLineOf(m_info.boundary.boundaryGeometryOuter, Color.yellow, m_info.headAndHands.m_rootUnityPosition);
    }

    

    private void DrawLineOf(Vector3[] lines, Color color, SpacePoint point)
    {
        if (point == null)
        {
            DrawLineOf(lines, color);
            return;
        }

        Vector3 start;
        Vector3 end;
        if (lines.Length > 1)
            for (int i = 1; i < lines.Length; i++)
            {
                 start = SpacePoint.GetRelocatedPosition(lines[i - 1], point);
                 end = SpacePoint.GetRelocatedPosition(lines[i], point);

                Debug.DrawLine(start,end, color, m_drawTime);
            }
         start = SpacePoint.GetRelocatedPosition(lines[lines.Length - 1], point);
         end = SpacePoint.GetRelocatedPosition(lines[0], point);

        Debug.DrawLine(start, end, color, m_drawTime);

    }
    private void DrawLineOf(Vector3[] lines, Color color)
    {
        if (lines.Length > 1)
            for (int i = 1; i < lines.Length; i++)
            {
                Debug.DrawLine(lines[i - 1], lines[i], color, m_drawTime);
            }
        Debug.DrawLine(lines[lines.Length - 1], lines[0], color, m_drawTime);

    }
}
