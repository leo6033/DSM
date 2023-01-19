using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;

namespace Disc0ver.Gameplay.StateMachine
{
    public partial class StateMachine<TOwner>
    {
        [Serializable]
        public new class WithDefault : StateMachine<TOwner>
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

            public WithDefault(TOwner owner): base(owner)
            {
                ForceSetDefaultState = () => ForceSetState(_defaultKey);
            }

            public WithDefault(TOwner owner, Type defaultKey) : this(owner)
            {
                _defaultKey = defaultKey;
                ForceSetState(defaultKey);
            }

            public WithDefault(TOwner owner, IDictionary<Type, State<TOwner>> dictionary, Type defaultKey) : base(owner, dictionary)
            {
                _defaultKey = defaultKey;
                ForceSetState(defaultKey);
            }

            public State<TOwner> TrySetDefaultState() => TrySetState(_defaultKey);

            public State<TOwner> TryResetDefaultState() => TryResetState(_defaultKey);
        }
    }
}