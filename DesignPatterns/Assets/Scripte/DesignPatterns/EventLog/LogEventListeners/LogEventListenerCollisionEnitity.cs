using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LogEventListenerCollisionEnitity : LogEventListenerBaseEnitity
{
    [Header("------ Trigger Enitity Bool------")]
    public bool sendWithoutCheckColliderObject;
    public bool sendLogOnCollisionEnter;
    public bool sendLogOnCollisionStay;
    public float onStayDelayToSentLogTime = 0;
    public bool sendLogOnCollisionExit;
    public bool nextSendNeedWiteTargetExit;
    private bool isCollisioned = false;

    public List<Collider> allowToSendLogCollisionObjectList = new List<Collider>();
    private float lastSentLogTime;

    private void OnCollisionEnter(Collision collision)
    {
        if (sendLogOnCollisionEnter)
        {
            if (checkIfCanSendLogEvent(collision))
            {
                Send();
            }
        }
        isCollisioned = true;
    }

    private void OnCollisionStay(Collision collision)
    {
        if (sendLogOnCollisionStay)
        {
            if (Time.time - lastSentLogTime > onStayDelayToSentLogTime && checkIfCanSendLogEvent(collision))
            {
                Send();
            }
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (sendLogOnCollisionExit)
        {
            if (Time.time - lastSentLogTime > onStayDelayToSentLogTime && checkIfCanSendLogEvent(collision))
            {
                Send();
            }
        }
        isCollisioned = false;
    }

    bool checkIfCanSendLogEvent(Collision other)
    {
        if (nextSendNeedWiteTargetExit && isCollisioned)
        {
            return false;
        }
        if (sendWithoutCheckColliderObject)
        {
            return true;
        }
        else
        {
            if (allowToSendLogCollisionObjectList.Contains(other.collider))
            {
                return true;
            }
        }

        return false;
    }
}
