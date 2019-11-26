using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateExampleStart : State
{
    /// <summary>
    /// この状況に入る時最初何かするか（一回のみ）
    /// </summary>
    /// <param name="setter"></param>
    public override void PreUpdate(StateSetter setter)
    {
        StateExampleState state_ = (StateExampleState)setter;
        Debug.Log("State Start");
    }

    /// <summary>
    /// Auto Upate by StateExample.cs　（Loop）
    /// </summary>
    /// <param name="setter"></param>
    public override void Update(StateSetter setter)
    {
        StateExampleState state_ = (StateExampleState)setter;
        state_.ChangeState(StateExampleState.StateExampleStates.StateExampleStateA.ToString());
    }

    /// <summary>
    /// この状況から別の状態に遷移する時、最後何かするか（一回のみ）
    /// </summary>
    /// <param name="setter"></param>
    public override void PostUpdate(StateSetter setter)
    {
        StateExampleState state_ = (StateExampleState)setter;
    }
}
