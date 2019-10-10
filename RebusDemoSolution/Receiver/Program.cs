using Messages;
using Rebus.Activation;
using Rebus.Config;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
                    .Transport(t => t.UseRabbitMq("amqp://localhost", "Receiver"))
                    .Start();

                activator.Bus.Subscribe<DemoMessage>().Wait();

                Console.WriteLine("Press [enter] to exit.");
                Console.ReadLine();
            }
        }

    }
}
