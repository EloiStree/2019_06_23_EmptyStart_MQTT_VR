using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateGun : MonoBehaviour
{
    public GameObject[] m_weaponPrefab;
    public CreatePrefab m_createFactory;

    public void Create(string name) {
        for (int i = 0; i < m_weaponPrefab.Length; i++)
        {
         Debug.Log(name.ToLower() + "><" + m_weaponPrefab[i].name.ToLower());
            if (m_weaponPrefab[i].name.ToLower() == name.ToLower())
                m_createFactory.Create(m_weaponPrefab[i]);
        }
    }
}
