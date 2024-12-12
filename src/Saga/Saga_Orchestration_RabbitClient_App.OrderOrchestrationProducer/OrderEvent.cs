using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Saga_Orchestration_RabbitClient_App.OrderOrchestrationProducer
{
    public class OrderEvent
    {
        public string Type { get; set; }
        public int OrderId { get; set; }
    }
}
