using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "ContextConditionDistance", menuName = "Context/Distance", order = 1)]
public class ContextDistanceScritable : ContextConditionScritable
{
    public ContextOnDistance m_condition;
    public override bool IsConditionRespected(Vector3 rightHandRelocated, Quaternion rightHandrotationRelocated)
    {
        return m_condition.IsConditionRespected(rightHandRelocated, rightHandrotationRelocated);
    }
}