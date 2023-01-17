using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;


namespace Disc0ver.Gameplay.FSM
{
    public interface IStateMachine
    {
        /// <summary>
        /// 当前状态
        /// </summary>
        object CurrentState { get; }
        
        object PreviousState { get; }
        
        object NextState { get; }

        /// <summary>
        /// 是否能进入指定的 state
        /// </summary>
        /// <remarks> </remarks>
        bool CanSetState(object state);

        /// <summary>
        /// 返回首个能进入的状态
        /// </summary>
        /// <param name="states"></param>
        /// <returns></returns>
        object CanSetState(IList states);

        
        bool TrySetState(object state);
    }
    
    public partial class StateMachine<TState> : IStateMachine 
        where TState: class, IState
    {
        [ShowInInspector] private TState _currentState;
        public object CurrentState => _currentState;
        
        
        public object PreviousState { get; }
        public object NextState { get; }
        public bool CanSetState(object state)
        {
            throw new System.NotImplementedException();
        }

        public object CanSetState(IList states)
        {
            throw new System.NotImplementedException();
        }

        public bool TrySetState(object state)
        {
            throw new System.NotImplementedException();
        }
    }
}

