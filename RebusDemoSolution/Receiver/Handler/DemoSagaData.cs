using Rebus.Sagas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderHandler.Handler
{
    class DemoSagaData : ISagaData
    {
        
        public Guid Id { get; set ; }
        public int Revision { get; set; }

        public int OrderId { get; set; }
        public string OrderType { get; set; }
        public bool OrderReceived { get; set; }
        public bool PaymentReceived { get; set; }
        public bool Complete()
        {
            return OrderReceived && PaymentReceived;
        }
    }
}
