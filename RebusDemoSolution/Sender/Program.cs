using Messages;
using Rebus.Activation;
using Rebus.Config;
using Rebus.Routing.TypeBased;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
                    .Routing(r => r.TypeBased().Map<DemoMessage>("Receiver"))
                    .Start();

                    bus.Publish(message).Wait();
                Console.ReadLine();

            }
        }
    }
}
