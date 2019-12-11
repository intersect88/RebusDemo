using Messages;
using Rebus.Activation;
using Rebus.Config;
using Rebus.Handlers;
using Rebus.Routing.TypeBased;
using Receiver.Handler;
using System;
using System.Configuration;
using System.Threading.Tasks;

namespace Receiver
{
    class Program
    {
        static void Main(string[] args)
        {
            //using (var activator = new BuiltinHandlerActivator())
            //{

            var db = ConfigurationManager.ConnectionStrings["SagasDB"].ToString();
            //    activator.Register(() => new Handler());

            //    var bus = Configure.With(activator)
            //        .Transport(t => t.UseMsmq("Receiver"))
            //        .Routing(r => r.TypeBased().Map<DemoMessage>("Receiver"))
            //        .Subscriptions(s => s.StoreInSqlServer(db, "SubscriptionsTable", isCentralized:true))
            //        .Sagas(s => s.StoreInSqlServer(db, "Sagas", "SagaIndex"))
            //        .Timeouts(t => t.StoreInSqlServer(db, "TimeOuts"))
            //        .Start();

            //    bus.Subscribe<DemoMessage>().Wait();

            //activator.Handle<DemoMessage>(async msg =>
            //{
            //    Console.WriteLine($"Received: {msg.Text}");
            //});

            //Console.WriteLine("Press [enter] to exit.");
            //    Console.ReadLine();
                using (var activator = new BuiltinHandlerActivator())
                {
                    activator.Register((bus, context) => new DemoSaga(bus));

                    Configure.With(activator)
                        //.Logging(l => l.ColoredConsole(minLevel: LogLevel.Warn))
                        .Subscriptions(s => s.StoreInSqlServer(db, "SubscriptionsTable", isCentralized: true))
                        .Sagas(s => s.StoreInSqlServer(db, "Sagas", "SagaIndex"))
                        .Timeouts(s => s.StoreInSqlServer(db, "Timeouts"))
                        .Transport(t => t.UseMsmq("Receiver"))
                                //.Routing(r => r.TypeBased().Map<DemoMessage>("Receiver"))

                        .Start();

                activator.Bus.Subscribe<Order>().Wait();
                //activator.Bus.Subscribe<TradeApproved>().Wait();
                //activator.Bus.Subscribe<TradeRejected>().Wait();

                Console.WriteLine("Press ENTER to quit");
                    Console.ReadLine();
                }
                
        }

    }

    //class Handler : IHandleMessages<DemoMessage>
    //{

    //    public async Task Handle(DemoMessage message)
    //    {
    //        Console.WriteLine("Got event -> "+message.Text);
    //    }
    //}
}
