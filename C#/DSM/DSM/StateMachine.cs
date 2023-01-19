using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;


namespace Disc0ver.Gameplay.StateMachine
{
    public interface IStateMachine<TKey>
    {
        /// <summary>
        /// 当前状态
        /// </summary>
        TKey CurrentKey { get; }
        
        TKey PreviousKey { get; }
        
        /// <summary>
        /// 尝试进入指定状态
        /// </summary>
        /// <remarks> 若当前状态与指定状态一致，则直接返回 </remarks>
        /// <returns> state if success </returns>
        object TrySetState(TKey key);

        /// <summary>
        /// 尝试进入指定状态
        /// </summary>
        /// <remarks> 若当前状态与指定状态一致，则重置当前状态 </remarks>
        /// <returns> state if success </returns>
        object TryResetState(TKey key);

        /// <summary>
        /// 强制设置为指定状态，如果 key 没有注册，则设置为默认状态
        /// </summary>
        /// <returns></returns>
        object ForceSetState(TKey key);

        void Tick();
    }
    
    public partial class StateMachine<TState> : IStateMachine<Type>
        where TState: class, IState
    {
        public IDictionary<Type, TState> dictionary { get; set; }

        [ShowInInspector] private Type _currentKey;
        private Type _previousKey;

        private TState _currentState;
        private TState _previousState;
        
        public TState CurrentState => _currentState;
        public TState PreviousState => _previousState;
        
        public Type CurrentKey => _currentKey;
        public Type PreviousKey => _previousKey;

        public StateMachine(TState defaultState)
        {
            dictionary = new Dictionary<Type, TState>()
            {
                { defaultState.GetType(), defaultState }
            };
            ForceSetState(defaultState.GetType());
        }

        public StateMachine(IDictionary<Type, TState> dictionary)
        {
            this.dictionary = dictionary;
        }

        public StateMachine(IDictionary<Type, TState> dictionary, TState state)
        {
            this.dictionary = dictionary;
            dictionary.Add(state.GetType(), state);
            ForceSetState(state.GetType());
        }

        public StateMachine()
        {
            dictionary = new Dictionary<Type, TState>();
        }

        public void Tick()
        {
            CurrentState?.OnTick(this);
        }

        /// <summary>
        /// 尝试进入指定状态
        /// </summary>
        /// <remarks> 若当前状态与指定状态一致，则直接返回 </remarks>
        /// <returns> state if success </returns>
        public TState TrySetState(Type key)
        {
            if (_currentKey == key)
                return _currentState;
            return TryResetState(key);
        }

        /// <summary>
        /// 尝试进入指定状态
        /// </summary>
        /// <remarks> 若当前状态与指定状态一致，则重置当前状态 </remarks>
        /// <returns> state if success </returns>
        public TState TryResetState(Type key)
        {
            if (dictionary.TryGetValue(key, out var state) && TryResetState(key, state))
                return state;
            return null;
        }

        /// <summary>
        /// 尝试进入指定状态
        /// </summary>
        /// <remarks> 若当前状态与指定状态一致，则重置当前状态 </remarks>
        /// <returns> state if success </returns>
        private bool TryResetState(Type key, TState state)
        {
            if (!CanSetState(state))
                return false;
            
            ForceSetState(key, state);
            return true;
        }

        /// <summary>
        /// 外部调用接口，强制改变当前状态
        /// </summary>
        /// <param name="key"> 状态的 key </param>
        public TState ForceSetState(Type key)
        {
            dictionary.TryGetValue(key, out var state);
            ForceSetState(key, state);
            return state;
        }
        
        /// <summary>
        /// 强制改变当前状态
        /// </summary>
        /// <remarks>
        /// 这个接口不会调用 <see cref="IState.CanExitState"/> 或 <see cref="IState.CanEnterState"/>,且不会判空，
        /// 如有需要，调用 <see cref="TryResetState(Type, TState)"/>
        /// </remarks>
        private void ForceSetState(Type key, TState state)
        {
            _previousKey = _currentKey;
            _currentState?.OnExitState(this);
            _previousState = _currentState;
            _currentState = state;
            _currentState?.OnEnterState(this);
            _currentKey = key;
        }

        /// <summary>
        /// 用于判断是否能够设置对应状态 
        /// </summary>
        private bool CanSetState(TState state)
        {
            if (_currentState != null && !_currentState.CanExitState)
                return false;

            if (state != null && !state.CanEnterState)
                return false;

            return true;
        }

        public void AddRange(TState[] states)
        {
            foreach (var state in states)
            {
                dictionary.Add(state.GetType(), state);
            }
        }

        #region IStateMachine

        object IStateMachine<Type>.TrySetState(Type type) => TrySetState(type);

        object IStateMachine<Type>.TryResetState(Type type) => TryResetState(type);

        object IStateMachine<Type>.ForceSetState(Type type) => ForceSetState(type);

        #endregion
    }
}

