using System.Collections.Generic;
using UnityEngine;

public class StateSetter : MonoBehaviour
{
    public Animator Ani;
    public string MyID;
    public string LastState;
    public bool isChangingState = false;
    protected StateMachine MyStateMachine = null;
    protected Dictionary<string, Attribute> Attributes = new Dictionary<string, Attribute>();

    private void OnDestroy()
    {
        MyStateMachine = null;
        Attributes.Clear();
        Attributes = null;
    }

    public void ChangeState(string iID)
    {
        if (MyStateMachine == null)
        {
            return;
        }
        PlayAnimation(iID);
        LastState = iID;
        MyStateMachine.ChangeState(iID);
    }

    virtual public void PlayAnimation(string iID)
    {
        if (Ani == null)
        {
            return;
        }

        Ani.Play(iID);
    }

    public Attribute GetAttribute(string iID)
    {
        if (Attributes.ContainsKey(iID) == false)
        {
            return null;
        }
        return Attributes[iID];
    }
}