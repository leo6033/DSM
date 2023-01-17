namespace Disc0ver.Gameplay.FSM
{
    /// <summary>
    /// interface of state that can be used in <see cref="StateMachine{TState}"/>
    /// </summary>
    public interface IState
    {
        bool CanEnterState { get; }
        
        bool CanExitState { get; }

        void OnEnterState(StateMachine<IState> stateMachine);

        void OnTick(StateMachine<IState> stateMachine);

        void OnExitState(StateMachine<IState> stateMachine);
    }

    public abstract class State : IState
    {
        public bool CanEnterState => true;
        public bool CanExitState => true;
        public void OnEnterState(StateMachine<IState> stateMachine) { }

        public void OnTick(StateMachine<IState> stateMachine) { }

        public void OnExitState(StateMachine<IState> stateMachine) { }
    }
}