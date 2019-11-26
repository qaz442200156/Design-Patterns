using System;
using System.Collections.Generic;
using UnityEngine;

public class StateManager : MonoBehaviour
{
    public delegate Dictionary<string, StateMachine> BuildStateMachineDelegate();

    private BuildStateMachineDelegate BuildStateMachineAction;
    protected Dictionary<string, StateMachine> StateMachines = new Dictionary<string, StateMachine>();

    /// <summary>
    /// 全部の能力データベース
    /// </summary>
    protected Dictionary<string, State> States = new Dictionary<string, State>();

    private void OnDestroy()
    {
        StateMachines.Clear();
        StateMachines = null;

        States.Clear();
        States = null;
    }

    public void Initial(BuildStateMachineDelegate iDelegate)
    {
        BuildStateMachineAction = iDelegate;
    }

    public void BuildStateMachine()
    {
        if (BuildStateMachineAction != null)
        {
            StateMachines = BuildStateMachineAction();
        }
    }

    public StateMachine GetStateMachine(string iID = "")
    {
        if (iID.Length == 0)
        {
            iID = "Main";
        }
        if (StateMachines.ContainsKey(iID) == false)
        {
            return null;
        }

        return StateMachines[iID].Clone();
    }

    public State CrateState(string iStateClassName)
    {
        if (States.ContainsKey(iStateClassName))
        {
            return States[iStateClassName];
        }

        System.Type _Type = System.Type.GetType(iStateClassName);
        if (_Type == null)
        {
            return null;
        }

        State _State = (State)Activator.CreateInstance(_Type);
        if (_State == null)
        {
            Debug.LogError("not has this State tpye : " + iStateClassName);
        }
        else
        {
            States.Add(iStateClassName, _State);
        }

        return _State;
    }
}