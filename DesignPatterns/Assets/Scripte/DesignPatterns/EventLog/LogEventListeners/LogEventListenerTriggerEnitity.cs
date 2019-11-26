using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LogEventListenerTriggerEnitity : LogEventListenerBaseEnitity
{
    [Header("------ Collider Enitity Bool------")]
    public bool sendWithoutCheckTriggerObject;
    public bool sendLogOnTriggerEnter;
    public bool sendLogOnTriggerStay;
    public float onStayDelayToSentLogTime = 0;
    public bool sendLogOnTriggerExit;
    public bool nextSendNeedWiteTargetExit;
    private bool isTriggered = false;

    public List<Collider> allowToSendLogTriggerObjectList = new List<Collider>();
    private float lastSentLogTime;

    public void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (sendLogOnTriggerEnter)
        {

            if (checkIfCanSendLogEvent(other))
            {
                lastSentLogTime = Time.time;
                Send();
            }
            isTriggered = true;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (sendLogOnTriggerStay)
        {
            if (Time.time - lastSentLogTime > onStayDelayToSentLogTime && checkIfCanSendLogEvent(other))
            {
                lastSentLogTime = Time.time;
                Send();
            }
            isTriggered = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (sendLogOnTriggerExit)
        {
            if (Time.time - lastSentLogTime > onStayDelayToSentLogTime && checkIfCanSendLogEvent(other))
            {
                lastSentLogTime = Time.time;
                Send();
            }
            isTriggered = true;
        }
        isTriggered = false;
    }

    bool checkIfCanSendLogEvent(Collider other)
    {
        if (nextSendNeedWiteTargetExit && isTriggered)
        {
            return false;
        }

        if (sendWithoutCheckTriggerObject)
        {
            return true;
        }
        else
        {
            if (allowToSendLogTriggerObjectList.Contains(other))
            {
                return true;
            }
        }

        return false;
    }
}
