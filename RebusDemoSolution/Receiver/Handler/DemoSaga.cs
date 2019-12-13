using Messages;
using Rebus.Bus;
using Rebus.Handlers;
using Rebus.Sagas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderHandler.Handler
{
    class DemoSaga : Saga<DemoSagaData>,
        IAmInitiatedBy<Order>, IAmInitiatedBy<OrderPayment>
    {
        readonly IBus _bus;

        public DemoSaga(IBus bus)
        {
            _bus = bus;
        }

        protected override void CorrelateMessages(ICorrelationConfig<DemoSagaData> config)
        {
            config.Correlate<Order>(m => m.Id, d => d.OrderId);
            config.Correlate<OrderPayment>(m => m.Id, d => d.OrderId);
        }

        public async Task Handle(Order message)
        {
            if (!IsNew) return;

            Data.OrderType = message.Type;
            Data.OrderReceived = true;

            Console.WriteLine(@"
Order Received -> Type: " + message.Type);

            await CompleteSaga();
        }

        public async Task Handle(OrderPayment message)
        {
            if (IsNew) return;

            Data.PaymentReceived = message.Payment;
            
            Console.WriteLine(@"
Payment Received!
");
            await CompleteSaga();

        }

        async Task CompleteSaga()
        {
            if (Data.Complete())
            {
                Console.WriteLine(@"
Order Complete
");
                MarkAsComplete();
            }
        }

    }
}
