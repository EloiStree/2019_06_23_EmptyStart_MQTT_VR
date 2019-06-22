using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SpaceTrackedPointMono : MonoBehaviour
{

    public abstract SpaceTrackedPoint GetSpaceTrackedPoint();
}






[System.Serializable]
public struct SpaceTrackedPoint {

    public SpacePoint m_unityPointOfView;
    public SpacePoint m_trackerPointOfView;
    public SpacePoint m_headPointOfView;

    public  SpaceTrackedPoint(Transform unityPosition, Transform trackerRoot, Transform headPosition) {
        m_unityPointOfView = new SpacePoint();
        m_trackerPointOfView = new SpacePoint();
        m_headPointOfView = new SpacePoint();
        SetPoint(unityPosition, trackerRoot, headPosition);
    }

    public void SetPoint(Transform unityPosition, Transform trackerRoot, Transform headPosition) {

        m_unityPointOfView.SetWith( unityPosition);
        m_trackerPointOfView.SetWith(trackerRoot, trackerRoot);
        m_headPointOfView.SetWith(headPosition, headPosition);
    }


}
[System.Serializable]
public struct SpacePoint {
    public Vector3 m_position;
    public Quaternion m_rotation;

    public SpacePoint(Transform reference)
    {
        m_position = new Vector3();
        m_rotation = new Quaternion();
        SetWith(reference);
    }
    public SpacePoint(Transform reference, Transform pointOfView)
    {
        m_position = new Vector3();
        m_rotation = new Quaternion();
        SetWith(reference, pointOfView);
    }
    public SpacePoint(Vector3 position, Quaternion rotation)
    {
        m_position = new Vector3();
        m_rotation = new Quaternion();
        SetWith(position, rotation);
    }
 
    public void SetWith(Transform reference)
    {
        m_position = reference.position;
        m_rotation = reference.rotation;
    }
    public void SetWith(Transform reference, Transform pointOfView)
    {
        SetWith(reference.position, reference.rotation, pointOfView.position, pointOfView.rotation);
    }

    public void SetWith(Vector3 position, Quaternion rotation)
    {
        m_position = position;
        m_rotation = rotation;
    }

    public void SetWith(Vector3 position, Quaternion rotation, Vector3 pointOfViewPosition, Quaternion pointOfViewRotation) {
        SpacePoint pointOfView = new SpacePoint(pointOfViewPosition, pointOfViewRotation);
        m_position = GetRelocatedPosition(position, pointOfView);
        m_rotation = GetRelocatedRotation(rotation, pointOfView);
    }
    
    public static SpacePoint GetRelocatedPoint(SpacePoint point, SpacePoint pointOfView) {
        SpacePoint result;
        result.m_position = GetRelocatedPosition(point.m_position, pointOfView);
        result.m_rotation = GetRelocatedRotation(point.m_rotation, pointOfView);
        return result; 
    }

    public static Vector3 GetRelocatedPosition(Vector3 position, SpacePoint pointOfView)
    {

        return Quaternion.Inverse(pointOfView.m_rotation) * (position - pointOfView.m_position);
    }
    public static Quaternion GetRelocatedRotation(Quaternion rotation, SpacePoint pointOfView)
    {
        // To verify
        return rotation * Quaternion.Inverse(pointOfView.m_rotation);
    }

}