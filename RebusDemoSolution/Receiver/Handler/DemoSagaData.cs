using Rebus.Sagas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Receiver.Handler
{
    class DemoSagaData : ISagaData
    {
        public Guid Id { get; set ; }
        public int Revision { get; set; }
        public string OrderType { get; set; }
    }
}
