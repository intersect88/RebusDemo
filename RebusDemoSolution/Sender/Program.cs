using Messages;
using Rebus.Activation;
using Rebus.Config;
using Rebus.Routing.TypeBased;
using System;

namespace Sender
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var activator = new BuiltinHandlerActivator())
            {
                var message = new DemoMessage("Demo Message");
                var bus = Configure.With(activator)
                    .Transport(t => t.UseMsmq("Sender"))
                    .Routing(r => r.TypeBased().Map<DemoMessage>("Receiver"))
                    .Start();
                
                bus.Send(message).Wait();
                Console.ReadLine();

            }
        }
    }
}
