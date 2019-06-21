using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveForward : MonoBehaviour
{
    public CharacterController m_moveForward;
    public Transform m_direction;
    public Transform m_affected;
    public float m_speed=1;
    public Rigidbody m_rig;
    public float m_force = 10;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update() {
        //{
        Vector3 dir = m_direction.forward;
        dir.y = 0;
        //    m_affected.position+=dir * Time.deltaTime* m_speed;
        //       m_moveForward.Move();
        // m_moveForward.SimpleMove(dir * m_speed);//* Time.deltaTime * m_speed);
        m_rig.AddForce(dir * m_force * Time.deltaTime, ForceMode.Impulse);
        m_rig.AddForce(Vector3.down * m_force * Time.deltaTime, ForceMode.Impulse);

    }
}
