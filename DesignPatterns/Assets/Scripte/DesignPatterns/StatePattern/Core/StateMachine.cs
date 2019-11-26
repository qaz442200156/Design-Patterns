using System.Collections.Generic;
using UnityEngine;

public class StateMachine : object
{
    //誰か利用している
    protected string ID;

    protected Dictionary<string, State> States = new Dictionary<string, State>();
    protected State CurrentState = null;
    protected StateSetter MyCharacter = null;

    public StateMachine(string iID = "")
    {
        if (iID.Length == 0)
        {
            iID = "Main";
        }
        ID = iID;
    }

    ~StateMachine()
    {
        if (States != null)
        {
            States.Clear();
            States = null;
        }

        this.CurrentState = null;
        this.MyCharacter = null;
    }

    //誰か利用しているを確認する
    public void BindCharacter(StateSetter iCharacter)
    {
        this.MyCharacter = iCharacter;
    }

    /// <summary>
    /// 新しい能力を追加する処
    /// </summary>
    /// <param name="iID"></param>
    /// <param name="iState"></param>
    public void AddState(string iID, State iState)
    {
        if (iState == null)
        {
            return;
        }

        if (this.States.ContainsKey(iID))
        {
            Debug.LogWarning("同じStateが存在する：" + iID);
            return;
        }

        this.States.Add(iID, iState);
    }

    /// <summary>
    /// 能力を除く処
    /// </summary>
    /// <param name="iID"></param>
    public void RemoveState(string iID)
    {
        if (this.States.ContainsKey(iID))
        {
            this.States.Remove(iID);
        }
    }

    /// <summary>
    /// 新しい状態遷移処
    /// </summary>
    /// <param name="iID"></param>
    public void ChangeState(string iID)
    {
        if (this.MyCharacter == null)
        {
            return;
        }

        if (this.States.ContainsKey(iID) == false)
        {
            return;
        }
        this.MyCharacter.isChangingState = true;
        if (this.CurrentState != null)
        {
            //今の状態を出る前に、最後の指示をする
            this.CurrentState.PostUpdate(this.MyCharacter);
        }

        //新しい状態に移動する
        this.CurrentState = this.States[iID];
        this.MyCharacter.isChangingState = false;
        this.CurrentState.PreUpdate(this.MyCharacter);
    }

    /// <summary>
    /// Update
    /// </summary>
	public void UpdateState()
    {
        if (this.CurrentState == null || this.MyCharacter == null)
        {
            return;
        }
        this.CurrentState.Update(this.MyCharacter);
    }

    public StateMachine Clone()
    {
        StateMachine _New = new StateMachine(ID);
        if (_New == null)
        {
            return null;
        }

        foreach (KeyValuePair<string, State> _KV in States)
        {
            _New.AddState(_KV.Key, _KV.Value);
        }

        return _New;
    }
}