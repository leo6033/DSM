using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;


namespace Disc0ver.Gameplay.StateMachine
{
    public interface IStateMachine<TOwner, TKey> where TOwner: class, IStateMachineOwner
    {
        TOwner Owner { get; }
        
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
    
    public partial class StateMachine<TOwner> : IStateMachine<TOwner, Type>
        where TOwner: class, IStateMachineOwner
    {
        private TOwner _owner;
        public TOwner Owner => _owner;
        public IDictionary<Type, State<TOwner>> dictionary { get; set; }

        [ShowInInspector] private Type _currentKey;
        private Type _previousKey;

        private State<TOwner> _currentState;
        private State<TOwner> _previousState;
        
        public State<TOwner> CurrentState => _currentState;
        public State<TOwner> PreviousState => _previousState;
        
        public Type CurrentKey => _currentKey;
        public Type PreviousKey => _previousKey;

        public StateMachine(TOwner owner, State<TOwner> defaultState)
        {
            _owner = owner;
            dictionary = new Dictionary<Type, State<TOwner>>()
            {
                { defaultState.GetType(), defaultState }
            };
            ForceSetState(defaultState.GetType());
        }

        public StateMachine(TOwner owner, IDictionary<Type, State<TOwner>> dictionary)
        {
            _owner = owner;
            this.dictionary = dictionary;
        }

        public StateMachine(TOwner owner, IDictionary<Type, State<TOwner>> dictionary, State<TOwner> state)
        {
            _owner = owner;
            this.dictionary = dictionary;
            dictionary.Add(state.GetType(), state);
            ForceSetState(state.GetType());
        }

        public StateMachine(TOwner owner)
        {
            _owner = owner;
            dictionary = new Dictionary<Type, State<TOwner>>();
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
        public State<TOwner> TrySetState(Type key)
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
        public State<TOwner> TryResetState(Type key)
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
        private bool TryResetState(Type key, State<TOwner> state)
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
        public State<TOwner> ForceSetState(Type key)
        {
            dictionary.TryGetValue(key, out var state);
            ForceSetState(key, state);
            return state;
        }
        
        /// <summary>
        /// 强制改变当前状态
        /// </summary>
        /// <remarks>
        /// 这个接口不会调用 <see cref="State{TOwner}.CanExitState"/> 或 <see cref="State{TOwner}.CanEnterState"/>,且不会判空，
        /// 如有需要，调用 <see cref="TryResetState(Type, State{TOwner})"/>
        /// </remarks>
        private void ForceSetState(Type key, State<TOwner> state)
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
        private bool CanSetState(State<TOwner> state)
        {
            if (_currentState != null && !_currentState.CanExitState(this))
                return false;

            if (state != null && !state.CanEnterState(this))
                return false;

            return true;
        }

        public void AddRange(State<TOwner>[] states)
        {
            foreach (var state in states)
            {
                dictionary.Add(state.GetType(), state);
            }
        }

        #region IStateMachine

        object IStateMachine<TOwner, Type>.TrySetState(Type type) => TrySetState(type);

        object IStateMachine<TOwner,Type>.TryResetState(Type type) => TryResetState(type);

        object IStateMachine<TOwner,Type>.ForceSetState(Type type) => ForceSetState(type);

        #endregion
    }
}

