using Messages;
using Rebus.Activation;
using Rebus.Config;
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
                    .Transport(t => t.UseRabbitMqAsOneWayClient("amqp://localhost"))
                    .Start();

                bus.Publish(message).Wait();
                Console.ReadLine();

            }
        }
    }
}
