using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreatePrefab : MonoBehaviour
{
    public Transform [] m_where;
    public GameObject [] m_prefab;

    public void Create() {
        Create(m_prefab[Random.Range(0, m_prefab.Length)]);
    }
    public void Create(int index)
    {
        Create(m_prefab[index]);

    }

    public void Create(GameObject toCreate) {
   
        Vector3 wherePos= Vector3.zero;
        Quaternion whereRot ;
        Quaternion[] rotations = new Quaternion[m_where.Length];
        for (int i = 0; i < m_where.Length; i++)
        {
            wherePos += m_where[i].position;
        }
        wherePos /= m_where.Length;
        whereRot = AverageQuaternion(rotations);
        Instantiate(toCreate, wherePos, whereRot);
    }
    Quaternion AverageQuaternion(Quaternion[] qArray)
    {
        Quaternion qAvg = qArray[0];
        float weight;
        for (int i = 1; i < qArray.Length; i++)
        {
            weight = 1.0f / (float)(i + 1);
            qAvg = Quaternion.Slerp(qAvg, qArray[i], weight);
        }
        return qAvg;
    }
}
