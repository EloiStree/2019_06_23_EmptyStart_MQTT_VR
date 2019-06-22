using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceTrackedPointMonoDebug : MonoBehaviour
{
    public SpaceTrackedPointMono m_toDebug;
    public SpaceTrackedPoint m_trackedPoint;

    public void Update()
    {
        m_trackedPoint = m_toDebug.GetSpaceTrackedPoint();
    }

}