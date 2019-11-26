public class StateExampleState : BaseState<StateExample, StateExampleState.StateExampleStates>
{
    // You can create Enum here or use other Enum
    public enum StateExampleStates {
        StateExampleStart,
        StateExampleStateA,
        StateExampleStateB,
        StateExampleStateC,
        StateExampleEnd
    }
}
