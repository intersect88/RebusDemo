using Messages;
using Rebus.Activation;
using Rebus.Config;
using System;
using System.Configuration;

namespace Sender
{
    class Program
    {
        static void Main(string[] args)
        {
            var azureServiceBusConnection = ConfigurationManager.ConnectionStrings["ASBConnection"].ConnectionString;
            using (var activator = new BuiltinHandlerActivator())
            {
                var message = new DemoMessage("Demo Message");
                var bus = Configure.With(activator)
                    .Transport(t => t.UseAzureServiceBus(azureServiceBusConnection, "Sender"))
                    .Start();

                bus.Publish(message);

                Console.ReadLine();

            }
        }
    }
}
