namespace Disc0ver.Gameplay.StateMachine.Example
{
    public class WaitSecondsState : State<Controller>
    {

        protected override void DoEnterState(StateMachine<Controller> stateMachine)
        {
            stateMachine.Owner.count = 0;
            System.Console.WriteLine($"enter state {GetType()}");
        }

        protected override void DoTick(StateMachine<Controller> stateMachine)
        {
            stateMachine.Owner.count += 1;
            
            System.Console.WriteLine($"{GetType()} On Tick, count {stateMachine.Owner.count}");

            if (stateMachine.Owner.count > 10)
                stateMachine.TrySetState(typeof(RandomChangeState));
        }

        protected override void DoExitState(StateMachine<Controller> stateMachine)
        {
            System.Console.WriteLine($"exit state {GetType()}");
        }
    }
}