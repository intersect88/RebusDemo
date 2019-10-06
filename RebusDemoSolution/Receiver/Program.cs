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
                activator.Handle<string>(async str =>
                {
                    Console.WriteLine($"Received: {str}");
                });

                Configure.With(activator)
                    //.Logging(l => l.ColoredConsole(minLevel: LogLevel.Warn))
                    //.Options(o => o.LogPipeline(verbose: true))
                    .Transport(t => t.UseRabbitMq("Receiver", "QueueDemo"))
                    .Start();

                activator.Bus.Subscribe<string>().Wait();
                Console.WriteLine("Press [enter] to exit.");
                Console.ReadLine();
            }
        }

    }
}
