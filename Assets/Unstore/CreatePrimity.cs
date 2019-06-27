using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreatePrimity : MonoBehaviour
{
    public float m_delay=2;
    public PrimitiveType m_type = PrimitiveType.Cube;
    public GameObject m_prefab;
    // Start is called before the first frame update
    IEnumerator Start()
    {

        while (true)
        {
            yield return new WaitForEndOfFrame();
            yield return new WaitForSeconds(m_delay);

            CreatePoint();
        }
    }

    public void CreatePoint()
    {
        GameObject obj;
        if (m_prefab)
            obj = GameObject.Instantiate(m_prefab);
        else obj = GameObject.CreatePrimitive(m_type);
        obj.transform.position = transform.position;
        obj.transform.rotation = transform.rotation;
    }
}
