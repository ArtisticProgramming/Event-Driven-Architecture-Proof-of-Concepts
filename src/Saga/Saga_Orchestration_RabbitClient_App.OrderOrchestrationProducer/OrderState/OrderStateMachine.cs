using Stateless;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Threading.Tasks;

namespace Saga_Orchestration_RabbitClient_App.OrderOrchestrationProducer.OrderState
{
    public class OrderStateMachine
    {
        private StateMachine<OrderState, OrderTrigger> _sm { get; set; }

        public OrderStateMachine()
        {
            //Init
            _sm = new StateMachine<OrderState, OrderTrigger>(OrderState.New);

            //Config
            _sm.Configure(OrderState.New)
                .Permit(OrderTrigger.PlaceOrder, OrderState.PendingPayment);

            _sm.Configure(OrderState.PendingPayment)
                .Permit(OrderTrigger.PaymentAccepted, OrderState.PendingStock)
                .Permit(OrderTrigger.PaymentRejected, OrderState.Cancelled);

            _sm.Configure(OrderState.PendingStock)
                .Permit(OrderTrigger.StockConfirmed, OrderState.PendingDelivery)
                .Permit(OrderTrigger.StockRejected, OrderState.Cancelled);

            _sm.Configure(OrderState.PendingDelivery)
                .Permit(OrderTrigger.Delivered, OrderState.Completed);

            _sm.Configure(OrderState.Completed)
                .Permit(OrderTrigger.OrderCancelled, OrderState.Cancelled);

        }

        public void Fire(OrderTrigger trigger)
        {
            _sm.Fire(trigger);
        }

        public OrderState State => _sm.State;

    }
}
