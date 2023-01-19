using System.Threading;
using Disc0ver.Gameplay.StateMachine.Example;

namespace Test
{
    public class Class1
    {
        static void Main(string[] args)
        {
            Controller controller = new Controller();
            controller.Init();

            while (true)
            {
                controller.Tick();
                Thread.Sleep(1000);
            }
        }
    }
}