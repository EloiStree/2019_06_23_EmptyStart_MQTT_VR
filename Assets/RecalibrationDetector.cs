using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class RecalibrationDetector : MonoBehaviour
{
    public HandsContextLoader m_context;
    public OnHandContextRequest m_contextRequest;
    public string m_storedContext;

    public void LockContext() {
        m_storedContext = m_context.GetContext();
    }

    public void NotifyHandContextRequest(HandsPosition handsPosition) {
        ContextRequest request = new ContextRequest();
        request.m_context = m_storedContext;
        request.m_hands = handsPosition;
        m_contextRequest.Invoke(request);
    }
}
[System.Serializable]
public class OnHandContextRequest : UnityEvent<ContextRequest>
{

}
[System.Serializable]
public struct ContextRequest{
    public string m_context;
    public HandsPosition m_hands;
}