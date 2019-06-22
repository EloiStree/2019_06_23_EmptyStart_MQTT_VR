using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceTrackedPointFromTransform : SpaceTrackedPointMono
{
    public Transform m_unityTransform;
    public Transform m_trackerTransform;
    public Transform m_headTransform;


    public void SetPoint(Transform unityPosition, Transform trackerRoot, Transform headPosition)
    {
        m_unityTransform = unityPosition;
        m_trackerTransform = trackerRoot;
        m_headTransform = headPosition;
    }
    public override SpaceTrackedPoint GetSpaceTrackedPoint()
    {
        return new SpaceTrackedPoint(m_unityTransform, m_trackerTransform, m_headTransform);
    }
}