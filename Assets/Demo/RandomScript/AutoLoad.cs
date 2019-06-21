using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AutoLoad : MonoBehaviour
{
    public float m_time = 2f;
    // Start is called before the first frame update
    IEnumerator Start()
    {
        yield return new WaitForSeconds(m_time);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex+1);
        
    }
    
}
