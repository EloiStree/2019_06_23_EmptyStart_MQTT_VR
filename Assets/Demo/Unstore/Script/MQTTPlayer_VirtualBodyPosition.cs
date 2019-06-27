using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MQTTPlayer_VirtualBodyPosition : MQTTPlayerInfoRegularSender
{
    public SpaceTrackedPointFromTransform m_head;
    public SpaceTrackedPointFromTransform m_leftHand;
    public SpaceTrackedPointFromTransform m_rightHand;
    public VirtualBodyPosition m_lastStatePush;
    public OVRCompressedInfo m_compressed = new OVRCompressedInfo();

    public override string ToSendEveryNowAndThen()
    {
        m_lastStatePush.m_head = m_head.GetSpaceTrackedPoint();
        m_lastStatePush.m_leftHand = m_leftHand.GetSpaceTrackedPoint();
        m_lastStatePush.m_rightHand = m_rightHand.GetSpaceTrackedPoint();
        m_compressed.SetWith(m_head, m_leftHand, m_rightHand);
        return m_compressed.GetCompressed();
    }
}

[System.Serializable]
public class VirtualBodyPosition {
    public SpaceTrackedPoint m_head;
    public SpaceTrackedPoint m_leftHand;
    public SpaceTrackedPoint m_rightHand;
}

[System.Serializable]
public class CompressedPosition
{
      const float round= 100f;
    public int x;
    public int y;
    public int z;
    public int u;
    public int i;
    public int o;
    public int p;
    public void SetWith(Vector3 position)
    {
        x = RoundPosTo(position.x);
        y = RoundPosTo(position.y);
        z = RoundPosTo(position.z);
    }
    public void SetWith(Quaternion rotation)
    {
        u = RoundQuaternionTo(rotation.x);
        i = RoundQuaternionTo(rotation.y);
        o = RoundQuaternionTo(rotation.z);
        p = RoundQuaternionTo(rotation.w);
    }
    public int RoundPosTo(float value)
    {
        return (int)(value * 1000f);
    }
    public int RoundQuaternionTo(float value)
    {
        return (int)(value * 100f);
    }
    public Vector3 GetPosition() { return new Vector3(x/1000f,y / 1000f, z / 1000f); }
    public Quaternion GetRotation() { return new Quaternion(u / 100f, i / 100f, o / 100f, p / 100f); }

    public string GetCompressed() {


        return string.Join("|", x, y, z, u, i, o, p);
    }
    public void SetWithCompressed(string compressed) {
        string[] value = compressed.Split('|');
        x = int.Parse(value[0]);
        y = int.Parse(value[1]);
        z = int.Parse(value[2]);
        u = int.Parse(value[3]);
        i = int.Parse(value[4]);
        o = int.Parse(value[5]);
        p = int.Parse(value[6]);

    }
}
[System.Serializable]
public class OVRCompressedInfo
{
    public CompressedPosition l = new CompressedPosition();
    public CompressedPosition r = new CompressedPosition();
    public CompressedPosition h = new CompressedPosition();
    public void SetWith(SpaceTrackedPointFromTransform head, SpaceTrackedPointFromTransform leftHand, SpaceTrackedPointFromTransform rightHand)
    {
         h.SetWith( head.m_unityTransform.position);
         h.SetWith( head.m_unityTransform.rotation);
         l.SetWith(leftHand.m_unityTransform.position);
         l.SetWith(leftHand.m_unityTransform.rotation);
         r.SetWith(rightHand.m_unityTransform.position);
         r.SetWith(rightHand.m_unityTransform.rotation);
    }

    public string GetCompressed()
    {
        return string.Join("#", l.GetCompressed(), r.GetCompressed(), h.GetCompressed());
    }
    public void SetWithCompressed(string compressed)
    {
        string[] value = compressed.Split('#');
        l.SetWithCompressed(value[0]);
        r.SetWithCompressed(value[1]);
        h.SetWithCompressed(value[2]); 
    }

    public static OVRCompressedInfo TryParse(string compressed)
    {
        OVRCompressedInfo comp = null; 

        try
        {
            comp = new OVRCompressedInfo();
            comp.SetWithCompressed(compressed);
        }
        catch (Exception e ) {
            Debug.Log("Parse fail:"+e.Message);
        }
        return comp;

    }
}