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
                string message = "Demo Message";
                var bus = Configure.With(activator)
                    //.Logging(l => l.ColoredConsole(LogLevel.Info))
                    //.Options(o => o.LogPipeline(verbose: true))
                    .Transport(t => t.UseMsmq("Sender"))
                    .Routing(r => r.TypeBased().Map<string>("Receiver"))
                    .Start();


                if (!string.IsNullOrWhiteSpace(message))
                    bus.Send(message).Wait();
                Console.ReadLine();

            }
        }
    }
}
