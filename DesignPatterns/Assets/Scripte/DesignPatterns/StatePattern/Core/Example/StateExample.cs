using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateExample : MonoBehaviour
{
    public StateExampleState state_;
    // Start is called before the first frame update
    void Start()
    {
        //State システムの初期化
        state_.Initial(this, StateExampleSystem.Instance.stateManager_, state_);

        //State の状態遷移 ※注目　StateExampleStateA.cs が必要です　無いの場合遷移が必ず失敗する
        state_.ChangeState(StateExampleState.StateExampleStates.StateExampleStateA.ToString());
    }

    private void Update()
    {
        if(state_ != null) { 
            //State の状態中のUpdate
            state_.UpdateState();
        }
    }
}
