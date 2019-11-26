using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(StateManager))]
public class BaseStateSystem<T, U> : MonoBehaviour
{
    public static T Instance;

    [HideInInspector]
    public StateManager stateManager_;

    //状態データを初期化するところ
    private void Awake()
    {
        Instance = GetComponent<T>();
        stateManager_ = GetComponent<StateManager>();
        stateManager_.Initial(BuildStateMachineAction);
        stateManager_.BuildStateMachine();
    }

    //TODO　状態データを読み込み　状態を作成する所
    private Dictionary<string, StateMachine> BuildStateMachineAction()
    {
        Dictionary<string, StateMachine> _StateMachines = new Dictionary<string, StateMachine>();
        StateMachine _StateMachine = new StateMachine("");

        foreach (U state in System.Enum.GetValues(typeof(U)))
        {
            _StateMachine.AddState(state.ToString(), stateManager_.CrateState(state.ToString()));
        }

        _StateMachines.Add("Main", _StateMachine);
        return _StateMachines;
    }
}