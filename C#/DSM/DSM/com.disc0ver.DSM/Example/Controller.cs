using System;
using System.Collections.Generic;

namespace Disc0ver.Gameplay.StateMachine.Example
{
    public class Controller: IStateMachineOwner
    {
        public StateMachine<Controller>.WithDefault stateMachine;

        public int count;
        public Random random;

        public void Init()
        {
            count = 0;
            random = new Random();
            stateMachine = new StateMachine<Controller>.WithDefault(this, new Dictionary<Type, State<Controller>>()
            {
                {typeof(WaitSecondsState), new WaitSecondsState()},
                {typeof(RandomChangeState), new RandomChangeState()}
            }, typeof(WaitSecondsState));
        }
        
        public void Tick()
        {
            stateMachine.Tick();
        }
    }
}