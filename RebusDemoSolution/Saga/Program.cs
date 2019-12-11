using Rebus.Activation;
using Rebus.Config;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Saga
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var activator = new BuiltinHandlerActivator())
            {
                activator.Register((bus, context) => new InvoicingSaga(bus));

                Configure.With(activator)
                    //.Logging(l => l.ColoredConsole(minLevel: LogLevel.Warn))
                    .Subscriptions(s => s.StoreInSqlServer(ConnectionString, "Subscriptions", isCentralized: true))
                    .Sagas(s => s.StoreInSqlServer(ConnectionString, "Sagas", "SagaIndex"))
                    .Timeouts(s => s.StoreInSqlServer(ConnectionString, "Timeouts"))
                    .Transport(t => t.UseMsmq("invoicing"))
                    .Start();

                activator.Bus.Subscribe<TradeRecorded>().Wait();
                activator.Bus.Subscribe<TradeApproved>().Wait();
                activator.Bus.Subscribe<TradeRejected>().Wait();

                Console.WriteLine("Invoicing is running - press ENTER to quit");
                Console.ReadLine();
            }
        }
    }
}
