namespace Disc0ver.Gameplay.StateMachine
{
    /// <summary>
    /// interface of state that can be used in <see cref="StateMachine{TState}"/>
    /// </summary>
    public interface IState
    {
        /// <summary>
        /// State 能否进入判断，条件根据具体游戏战斗系统进行定义，比如 buff、ability 等都会影响 State 的进入
        /// </summary>
        bool CanEnterState { get; }
        
        /// <summary>
        /// State 能否退出判断，条件根据具体游戏战斗系统进行定义，比如 buff、ability 等都会影响 State 的进入
        /// </summary>
        bool CanExitState { get; }

        void OnEnterState<TState>(StateMachine<TState> stateMachine) where TState: class, IState;

        void OnTick<TState>(StateMachine<TState> stateMachine) where TState: class, IState;

        void OnExitState<TState>(StateMachine<TState> stateMachine) where TState: class, IState;
    }

    public abstract class State : IState
    {
        public bool CanEnterState => true;
        public bool CanExitState => true;
        public void OnEnterState<TState>(StateMachine<TState> stateMachine) where TState : class, IState
        {
            DoEnterState(stateMachine);
        }

        public void OnTick<TState>(StateMachine<TState> stateMachine) where TState : class, IState
        {
            DoTick(stateMachine);
        }

        public void OnExitState<TState>(StateMachine<TState> stateMachine) where TState : class, IState
        {
            DoExitState(stateMachine);
        }

        protected abstract void DoEnterState<TState>(StateMachine<TState> stateMachine) where TState : class, IState;
        protected abstract void DoTick<TState>(StateMachine<TState> stateMachine) where TState : class, IState;
        protected abstract void DoExitState<TState>(StateMachine<TState> stateMachine) where TState : class, IState;

    }
}