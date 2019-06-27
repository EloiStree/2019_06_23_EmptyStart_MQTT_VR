using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadRecalibrationTest : MonoBehaviour
{
    public Transform m_calibrationPoint;
    public Transform m_ovrRoot;
    public Transform m_ovrHead;

    public Vector3 m_offsetHeadToRootPosition;
    public Quaternion m_offsetHeadToRootRotation;

    public Vector3 m_offsetHeadToCalibrationPosition;
    public Quaternion m_offsetHeadToCalibrationRotation;

    void Update()
    {
        m_offsetHeadToRootPosition = GetRelocatedPosition(m_ovrHead.position, m_ovrRoot.position, m_ovrRoot.rotation);
        m_offsetHeadToRootRotation = GetRelocatedRotation(m_ovrHead.rotation, m_ovrRoot.rotation);
        Debug.DrawLine(m_offsetHeadToRootPosition, m_offsetHeadToRootPosition + m_offsetHeadToRootRotation * (Vector3.forward * 0.1f), Color.red, Time.deltaTime);
        Debug.DrawLine(Vector3.zero, m_offsetHeadToRootPosition, Color.green, Time.deltaTime);

        m_offsetHeadToCalibrationPosition = m_calibrationPoint.position - m_ovrHead.position;
        m_offsetHeadToCalibrationRotation = GetDifferenceOfQuaternion(m_calibrationPoint.rotation, m_ovrHead.rotation);

        if (Input.GetKeyDown(KeyCode.Space)) {
            Recaliber();
        }
    }

    public Quaternion GetDifferenceOfQuaternion(Quaternion a, Quaternion b) {
        return a * Quaternion.Inverse(b);
    }
    public Quaternion ApplyRotationToQuaterion(Quaternion toA, Quaternion byB) {
        return toA * byB;
    }

    public  void Recaliber()
    {
        m_ovrRoot.position = m_calibrationPoint.position;
        m_ovrRoot.rotation = m_calibrationPoint.rotation;
        Quaternion diffBetweenCalAndHead = GetDifferenceOfQuaternion(m_ovrHead.rotation, m_calibrationPoint.rotation);
        m_ovrRoot.rotation = m_ovrRoot.rotation *  Quaternion.Inverse(diffBetweenCalAndHead);
        m_ovrRoot.position -= m_ovrHead.position - m_ovrRoot.position;
       // m_ovrRoot.rotation = ApplyRotationToQuaterion(m_ovrRoot.rotation, m_offsetHeadToCalibrationRotation);
    }

    public static Vector3 GetRelocatedPosition(Vector3 position, Vector3 refPosition, Quaternion refRotation)
    {
        return Quaternion.Inverse(refRotation) * (position - refPosition);
    }
    public static Quaternion GetRelocatedRotation(Quaternion rotation, Quaternion refRotation)
    {
        return rotation * Quaternion.Inverse(refRotation);
    }
    private void CrossDrawing(Transform info)
    {
        Debug.DrawLine(info.up + info.position, -info.up + info.position, Color.green, 0.1f);
        Debug.DrawLine(info.right + info.position, -info.right + info.position, Color.green, 0.1f);
        Debug.DrawLine(info.forward + info.position, -info.forward + info.position, Color.green, 0.1f);
    }
}
