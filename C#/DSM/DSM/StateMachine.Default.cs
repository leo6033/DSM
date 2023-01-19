using System;
using Sirenix.OdinInspector;

namespace Disc0ver.Gameplay.StateMachine
{
    public partial class StateMachine<TState>
    {
        [Serializable]
        public new class WithDefault : StateMachine<TState>
        {
            [ShowInInspector] private Type _defaultKey;

            public Type DefaultKey
            {
                get => _defaultKey;
                set
                {
                    _defaultKey = value;
                    if (CurrentState == null && value != null)
                        ForceSetState(value);
                }
            }
            
            public readonly Action ForceSetDefaultState;

            public WithDefault()
            {
                ForceSetDefaultState = () => ForceSetState(_defaultKey);
            }

            public WithDefault(Type defaultKey) : this()
            {
                _defaultKey = defaultKey;
                ForceSetState(defaultKey);
            }

            public TState TrySetDefaultState() => TrySetState(_defaultKey);

            public TState TryResetDefaultState() => TryResetState(_defaultKey);
        }
    }
}