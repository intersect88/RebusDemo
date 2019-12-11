using Messages;
using Rebus.Bus;
using Rebus.Handlers;
using Rebus.Sagas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Receiver.Handler
{
    class DemoSaga : Saga<DemoSagaData>,
        IAmInitiatedBy<Order>
    {
        readonly IBus _bus;

        public DemoSaga(IBus bus)
        {
            _bus = bus;
        }

        protected override void CorrelateMessages(ICorrelationConfig<DemoSagaData> config)
        {
            config.Correlate<Order>(m => m.Type, d => d.OrderType);
        }

        public async Task Handle(Order message)
        {
            if (!IsNew) return;


            // store the CRM customer ID in the saga
            Data.OrderType = message.Type;

            // command that legal information be acquired for the customer
            //await _bus.Publish(new OrderQuantity(message.quantity));
            Console.WriteLine("Order Received -> Type: "+message.Type);
            MarkAsComplete();

        }

    }
}
