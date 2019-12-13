using Messages;
using Rebus.Activation;
using Rebus.Bus;
using Rebus.Config;
using Rebus.Handlers;
using Rebus.Routing.TypeBased;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaymentService
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var activator = new BuiltinHandlerActivator())
            {
                var sagaDbConnectionString = ConfigurationManager.ConnectionStrings["SagasDB"].ToString();
                activator.Register((bus, context) => new Handler(bus));

                Configure.With(activator)
                    .Transport(t => t.UseMsmq("PaymentService"))
                    .Subscriptions(s => s.StoreInSqlServer(sagaDbConnectionString, "SubscriptionsTable", isCentralized: true))
                    .Start();

                activator.Bus.Subscribe<OrderConfirmation>().Wait();

                Console.WriteLine("Press [enter] to exit.");
                Console.ReadLine();
            }
        }
    }

    class Handler : IHandleMessages<OrderConfirmation>
    {
        readonly IBus _bus;

        public Handler(IBus bus)
        {
            _bus = bus;
        }

        public async Task Handle(OrderConfirmation message)
        {
            if (message.Confirmation)
                Console.WriteLine("Payment accepted");
            var payment = true;
            _bus.Publish(new OrderPayment(message.Id, payment)).Wait();
        }
    }
}
