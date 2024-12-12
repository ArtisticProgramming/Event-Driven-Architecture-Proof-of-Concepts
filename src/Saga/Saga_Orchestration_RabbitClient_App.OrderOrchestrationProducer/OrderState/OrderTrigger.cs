using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Saga_Orchestration_RabbitClient_App.OrderOrchestrationProducer.OrderState
{
    public enum OrderTrigger
    {
        PlaceOrder,
        PaymentAccepted,
        PaymentRejected,
        StockConfirmed,
        StockRejected,
        Delivered,
        OrderCancelled,
    }
}
