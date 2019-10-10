using Rebus.Activation;
using Rebus.Config;
using Rebus.Routing.TypeBased;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sender
{
    class Program
    {
        static void Main(string[] args)
        {
            var azureServiceBusConnection = ConfigurationManager.ConnectionStrings["ASBConnection"].ConnectionString;
            using (var activator = new BuiltinHandlerActivator())
            {
                string message = "Demo Message";
                Configure.With(activator)
                    //.Logging(l => l.ColoredConsole(LogLevel.Info))
                    //.Options(o => o.LogPipeline(verbose: true))
                    .Transport(t => t.UseAzureServiceBus(azureServiceBusConnection, "publisher"))
                    .Start();

                var bus = activator.Bus.Advanced.SyncBus;
                //bus.
                if (!string.IsNullOrWhiteSpace(message))
                {
                    bus.Publish(new PublishMessage(message));
                }
                Console.ReadLine();

            }
        }
    }

    internal class PublishMessage
    {
        private string message;

        public PublishMessage(string message)
        {
            this.message = message;
        }
    }
}
