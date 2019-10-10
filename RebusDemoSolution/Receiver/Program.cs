using Messages;
using Rebus.Activation;
using Rebus.Config;
using System;

namespace Receiver
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var activator = new BuiltinHandlerActivator())
            {
                activator.Handle<DemoMessage>(async msg =>
                {
                    Console.WriteLine($"Received: {msg.Text}");
                });

                Configure.With(activator)
                    .Transport(t => t.UseMsmq("Receiver"))
                    .Start();
                Console.WriteLine("Press [enter] to exit.");
                Console.ReadLine();
            }
        }

    }
}
