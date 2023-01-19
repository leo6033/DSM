namespace Disc0ver.Gameplay.StateMachine
{
    // /// <summary>
    // /// interface of state that can be used in <see cref="StateMachine"/>
    // /// </summary>
    // public interface IState<TState> where TState : State
    // {
    //     /// <summary>
    //     /// State 能否进入判断，条件根据具体游戏战斗系统进行定义，比如 buff、ability 等都会影响 State 的进入
    //     /// </summary>
    //     bool CanEnterState { get; }
    //     
    //     /// <summary>
    //     /// State 能否退出判断，条件根据具体游戏战斗系统进行定义，比如 buff、ability 等都会影响 State 的进入
    //     /// </summary>
    //     bool CanExitState { get; }
    //
    //     void OnEnterState(StateMachine stateMachine);
    //
    //     void OnTick(StateMachine stateMachine);
    //
    //     void OnExitState(StateMachine stateMachine);
    // }

    public abstract class State<TOwner> where TOwner: class, IStateMachineOwner
    {
        /// <summary>
        /// State 能否进入判断，条件根据具体游戏战斗系统进行定义，比如 buff、ability 等都会影响 State 的进入
        /// </summary>
        public virtual bool CanEnterState(StateMachine<TOwner> stateMachine) => true;
        
        /// <summary>
        /// State 能否退出判断，条件根据具体游戏战斗系统进行定义，比如 buff、ability 等都会影响 State 的进入
        /// </summary>
        public virtual bool CanExitState(StateMachine<TOwner> stateMachine) => true;
        
        public void OnEnterState(StateMachine<TOwner> stateMachine)
        {
            DoEnterState(stateMachine);
        }

        public void OnTick(StateMachine<TOwner> stateMachine)
        {
            DoTick(stateMachine);
        }

        public void OnExitState(StateMachine<TOwner> stateMachine)
        {
            DoExitState(stateMachine);
        }

        protected abstract void DoEnterState(StateMachine<TOwner> stateMachine);
        protected abstract void DoTick(StateMachine<TOwner> stateMachine);
        protected abstract void DoExitState(StateMachine<TOwner> stateMachine);

    }
}