using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandsContextLoader : MonoBehaviour
{
    public Transform m_leftHand;
    public Transform m_rightHand;

    public Context [] contexts;
    public string m_loadedContext;

    public void Update()
    {
        Vector3 rigthPositionRelocated = GetRelocatedPosition(m_rightHand.position, m_leftHand.position, m_leftHand.rotation);
        Quaternion rightRotationRelocated = GetRelocatedRotation(m_rightHand.rotation, m_leftHand.rotation);
        CrossDrawing(m_leftHand);
        Debug.DrawLine(rigthPositionRelocated, rigthPositionRelocated + rightRotationRelocated * (Vector3.forward * 0.1f), Color.red, Time.deltaTime);
        Debug.DrawLine(Vector3.zero, rigthPositionRelocated, Color.green, Time.deltaTime);


        string contextFound = "";
        for (int i = 0; i < contexts.Length; i++)
        {
            if(contexts[i].IsConditionRespected(rigthPositionRelocated, rightRotationRelocated))
            {
                string context = contexts[i].GetContext();
                if (context != "")
                    contextFound = context;
            }
        }
        m_loadedContext = contextFound;
    }

    public string GetContext()
    {
        return m_loadedContext;
    }

    private void CrossDrawing(Transform info)
    {
        Debug.DrawLine(info.up + info.position, -info.up + info.position, Color.green, 0.1f);
        Debug.DrawLine(info.right + info.position, -info.right + info.position, Color.green, 0.1f);
        Debug.DrawLine(info.forward + info.position, -info.forward + info.position, Color.green, 0.1f);
    }

  

  
    public static Vector3 GetRelocatedPosition(Vector3 position, Vector3 refPosition, Quaternion refRotation)
    {

        return Quaternion.Inverse(refRotation) * (position - refPosition);
    }
    public static Quaternion GetRelocatedRotation(Quaternion rotation, Quaternion refRotation)
    {
        // To verify
        return rotation * Quaternion.Inverse(refRotation);
    }

}

[System.Serializable]
public class Context {
    public string m_context;
    public ContextConditionScritable m_conditionsDistance;

    public  bool IsConditionRespected(Vector3 rigthPositionRelocated, Quaternion rightRotationRelocated)
    {
       return  m_conditionsDistance.IsConditionRespected(rigthPositionRelocated, rightRotationRelocated);
    }

    internal string GetContext()
    {
        return m_context;
    }
}


public abstract class RightHandContextCondition {

    public bool IsConditionRespected(Vector3 rightHandRelocated) {
        return IsConditionRespected(rightHandRelocated, Quaternion.identity);
    }
    public abstract bool IsConditionRespected(Vector3 rightHandRelocated, Quaternion rightHandrotationRelocated);
}

[System.Serializable]
// Context to load based on left angle compare to the both hands
public class ContextOnRightHandAngle: RightHandContextCondition
{
    public float m_angle = 0;
    public float m_aproximation = 5;
    [Header("Debug")]
    public float m_lastAngleValue;
    public bool m_lastConditionResult;
    public override bool IsConditionRespected(Vector3 rightHandRelocated, Quaternion rightHandrotationRelocated)
    {
        Vector3 euler = rightHandrotationRelocated.eulerAngles;
        euler.x = 0;
        return m_lastConditionResult= false;
    }
}
[System.Serializable]
// Context to load based on the distance compare to the both hands
public class ContextOnDistance : RightHandContextCondition
{
    public float m_distanceMeter = 0.10f;
    public float m_approximationMeter = 0.01f;
    public bool m_ignoreAxeX=false;
    public bool m_ignoreAxeY=true;
    public bool m_ignoreAxeZ=false;
    [Header("Debug")]
    public float m_lastValue;
    public bool m_lastConditionResult;

    public override bool IsConditionRespected(Vector3 rightHandRelocated, Quaternion rightHandrotationRelocated)
    {
        if (m_ignoreAxeX)
            rightHandRelocated.x = 0;
        if (m_ignoreAxeY)
            rightHandRelocated.y = 0;
        if (m_ignoreAxeZ)
            rightHandRelocated.z = 0;

        float dist = Vector3.Distance(Vector3.zero, rightHandRelocated);
        m_lastValue = dist;
        return m_lastConditionResult=MyMathf.IsBetween(dist, m_distanceMeter, m_approximationMeter);
    }

}



[System.Serializable]
// Context to load based on the position compare to the both hands
public class ContextOnRightHandOffset : RightHandContextCondition
{
    public Vector3 m_offsetPosition = Vector3.zero;
    public float m_appoximationMeter = 0.02f;

    [Header("Debug")]
    public float m_lastValue;
    public bool m_lastConditionResult;

    public override bool IsConditionRespected(Vector3 rightHandRelocated, Quaternion rightHandrotationRelocated)
    {
        float dist = Vector3.Distance(m_offsetPosition, rightHandRelocated);
        m_lastValue = dist;
        return m_lastConditionResult = MyMathf.IsBetween(dist, 0, m_appoximationMeter); ;
    }
}
[System.Serializable]
// Gropue of angle, distance and offset if all true
public class ContextGroupAnd: RightHandContextCondition
{
    public RightHandContextCondition[] conditions;

    public override bool IsConditionRespected(Vector3 rightHandRelocated, Quaternion rightHandrotationRelocated)
    {
        for (int i = 0; i < conditions.Length; i++)
        {
            if (!conditions[i].IsConditionRespected(rightHandRelocated, rightHandrotationRelocated))
                return false;
        }
        return true;
    }
}
[System.Serializable]
// Groupe of angle, distance and offset if one true
public class ContextGroupOr : RightHandContextCondition
{
    public RightHandContextCondition[] conditions;

    public override bool IsConditionRespected(Vector3 rightHandRelocated, Quaternion rightHandrotationRelocated)
    {
        for (int i = 0; i < conditions.Length; i++)
        {
            if (conditions[i].IsConditionRespected(rightHandRelocated, rightHandrotationRelocated))
                return true;
        }
        return false;
    }
}
public class MyMathf
{
    public static bool IsBetween(float value, float distance, float approximation)
    {
        return (value > distance - approximation) && (value < distance + approximation);
    }
}

public abstract class ContextConditionScritable :ScriptableObject {
    public bool IsConditionRespected(Vector3 rightHandRelocated)
    {
        return IsConditionRespected(rightHandRelocated, Quaternion.identity);
    }
    public abstract bool IsConditionRespected(Vector3 rightHandRelocated, Quaternion rightHandrotationRelocated);
    
}


[CreateAssetMenu(fileName = "ContextConditionOffset", menuName = "Context/Offset", order = 1)]
public class ContextOnOffsetScritable : ContextConditionScritable
{
    public ContextOnRightHandOffset m_condition;
    public override bool IsConditionRespected(Vector3 rightHandRelocated, Quaternion rightHandrotationRelocated)
    {
        return m_condition.IsConditionRespected(rightHandRelocated, rightHandrotationRelocated);
    }
}

[CreateAssetMenu(fileName = "ContextConditionAngle", menuName = "Context/Angle", order = 1)]
public class ContextOnAngleScritable : ContextConditionScritable
{
    public ContextOnRightHandAngle m_condition;
    public override bool IsConditionRespected(Vector3 rightHandRelocated, Quaternion rightHandrotationRelocated)
    {
        return m_condition.IsConditionRespected(rightHandRelocated, rightHandrotationRelocated);
    }
}

[CreateAssetMenu(fileName = "ContextConditionAnd", menuName = "Context/And", order = 1)]
public class ContextOnAndScritable : ContextConditionScritable
{
    public ContextGroupAnd m_condition;
    public override bool IsConditionRespected(Vector3 rightHandRelocated, Quaternion rightHandrotationRelocated)
    {
        return m_condition.IsConditionRespected(rightHandRelocated, rightHandrotationRelocated);
    }
}

[CreateAssetMenu(fileName = "ContextConditionOr", menuName = "Context/Or", order = 1)]
public class ContextOnOrScritable : ContextConditionScritable
{
    public ContextGroupOr m_condition;
    public override bool IsConditionRespected(Vector3 rightHandRelocated, Quaternion rightHandrotationRelocated)
    {
        return m_condition.IsConditionRespected(rightHandRelocated, rightHandrotationRelocated);
    }
}
