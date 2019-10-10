using Rebus.Activation;
using Rebus.Config;
using Rebus.Handlers;
using Rebus.Logging;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Receiver
{
    class Program
    {
        static void Main(string[] args)
        {
            var azureServiceBusConnection = ConfigurationManager.ConnectionStrings["ASBConnection"].ConnectionString;

            using (var activator = new BuiltinHandlerActivator())
            {
                activator.Register(() => new Handler());

                Configure.With(activator)
                    .Logging(l => l.ColoredConsole(minLevel: LogLevel.Warn))
                    .Transport(t => t.UseAzureServiceBus(azureServiceBusConnection, "subscriber1"))
                    .Start();

                activator.Bus.Subscribe<PublishMessage>().Wait();

                Console.WriteLine("Press [enter] to exit.");
                Console.ReadLine();
            }
        }

    }
    internal class PublishMessage
    {
        public string message;

        public PublishMessage(string message)
        {
            this.message = message;
        }
    }

    class Handler : IHandleMessages<PublishMessage>
    {
        public async Task Handle(PublishMessage message)
        {
            Console.WriteLine("Got string: {0}", message.message);
        }
    }

}
