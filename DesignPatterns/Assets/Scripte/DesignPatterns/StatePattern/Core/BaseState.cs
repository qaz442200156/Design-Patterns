using UnityEngine;

public class BaseState<T, U> : StateSetter
{
    public U InitiaStateID;

    [HideInInspector]
    public T host_;
    
    /// <summary>
    /// 魂を初期化処
    /// </summary>
    /// <param name="playerMoveEngine">プレイヤー移動状況</param>
    /// <param name="id">プレイヤーId</param>
    public void Initial(T target, StateManager manager, StateSetter setter)
    {
        host_ = target;
        MyStateMachine = manager.GetStateMachine();
        if (MyStateMachine != null)
        {
            MyStateMachine.BindCharacter(setter);
        }

        ChangeState(InitiaStateID.ToString());
    }

    public void UpdateState()
    {
        if (MyStateMachine == null)
        {
            return;
        }
        MyStateMachine.UpdateState();
    }
}