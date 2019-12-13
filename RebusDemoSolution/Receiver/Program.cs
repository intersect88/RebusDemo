using Messages;
using Rebus.Activation;
using Rebus.Config;
using Rebus.Handlers;
using Rebus.Routing.TypeBased;
using OrderHandler.Handler;
using System;
using System.Configuration;
using System.Threading.Tasks;
using Rebus.Logging;

namespace OrderHandler
{
    class Program
    {
        static void Main(string[] args)
        {
            var sagaDbConnectionString = ConfigurationManager.ConnectionStrings["SagasDB"].ToString();

            using (var activator = new BuiltinHandlerActivator())
            {
                activator.Register((bus, context) => new DemoSaga(bus));

                Configure.With(activator)
                    .Subscriptions(s => s.StoreInSqlServer(sagaDbConnectionString, "SubscriptionsTable", isCentralized: true))
                    .Sagas(s => s.StoreInSqlServer(sagaDbConnectionString, "Sagas", "SagaIndex"))
                    .Transport(t => t.UseMsmq("OrderHandler"))
                    .Start();

                activator.Bus.Subscribe<Order>().Wait();
                activator.Bus.Subscribe<OrderPayment>().Wait();

                Console.WriteLine("Press ENTER to quit");
                Console.ReadLine();

            }

        }

    }
}
