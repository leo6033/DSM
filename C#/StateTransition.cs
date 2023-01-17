using System;
using System.Collections;
using System.Collections.Generic;

namespace Disc0ver.Gameplay.FSM
{
    public struct StateTransition<TState> : IDisposable where TState: class, IState
    {
        [ThreadStatic] private static StateTransition<TState> _current;

        private StateMachine<TState> _stateMachine;
        private TState _previousState;
        private TState _nextState;

        public static bool IsActive => _current._stateMachine != null;

        public static StateMachine<TState> StateMachine => _current._stateMachine;

        public static TState PreviousState => _current._previousState;

        public static TState NextState => _current._nextState;

        internal StateTransition(StateMachine<TState> stateMachine, TState previousState, TState nextState)
        {
            this = _current;

            _current._stateMachine = stateMachine;
            _current._previousState = previousState;
            _current._nextState = nextState;
        }

        public void Dispose()
        {
            _current = this;
        }
    }
}

