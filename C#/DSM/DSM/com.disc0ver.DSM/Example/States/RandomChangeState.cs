using System;

namespace Disc0ver.Gameplay.StateMachine.Example
{
    public class RandomChangeState: State<Controller>
    {
        protected override void DoEnterState(StateMachine<Controller> stateMachine)
        {
            System.Console.WriteLine($"enter state {GetType()}");
        }

        protected override void DoTick(StateMachine<Controller> stateMachine)
        {
            var random = stateMachine.Owner.random.Next(10);
            System.Console.WriteLine($"{GetType()} On Tick, random value {random}");
            if (random > 5)
                stateMachine.TrySetState(typeof(WaitSecondsState));
        }

        protected override void DoExitState(StateMachine<Controller> stateMachine)
        {
            System.Console.WriteLine($"enter state {GetType()}");
        }
    }
}