using Messages;
using Rebus.Activation;
using Rebus.Config;
using System;
using System.Configuration;

namespace Receiver
{
    class Program
    {
        static void Main(string[] args)
        {
            var azureServiceBusConnection = ConfigurationManager.ConnectionStrings["ASBConnection"].ConnectionString;

            using (var activator = new BuiltinHandlerActivator())
            {
                activator.Handle<DemoMessage>(async msg =>
                {
                    Console.WriteLine($"Received: {msg.Text}");
                });

                var bus = Configure.With(activator)
                    .Transport(t => t.UseAzureServiceBus(azureServiceBusConnection, "Receiver"))
                    .Start();

                bus.Subscribe<DemoMessage>().Wait();

                Console.WriteLine("Press [enter] to exit.");
                Console.ReadLine();
            }
        }

    }
}
