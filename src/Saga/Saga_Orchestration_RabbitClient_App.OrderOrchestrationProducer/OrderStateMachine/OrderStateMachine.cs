using Stateless;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Threading.Tasks;

namespace Saga_Orchestration_RabbitClient_App.OrderOrchestrationProducer.OrderStateMachine
{
    public class OrderStateMachine
    {
        private StateMachine<OrderState, OrderTrigger> sm { get; set; }

        public OrderStateMachine()
        {
            //Init
            sm = new StateMachine<OrderState, OrderTrigger>(OrderState.New);

            //Config
            sm.Configure(OrderState.New)
                .Permit(OrderTrigger.PlaceOrder, OrderState.PendingPayment);

            sm.Configure(OrderState.PendingPayment)
                .Permit(OrderTrigger.PaymentAccepted, OrderState.PendingStock)
                .Permit(OrderTrigger.PaymentRejected, OrderState.Cancelled);

            sm.Configure(OrderState.PendingStock)
                .Permit(OrderTrigger.StockConfirmed, OrderState.PendingDelivery)
                .Permit(OrderTrigger.StockRejected, OrderState.Cancelled);

            sm.Configure(OrderState.PendingDelivery)
                .Permit(OrderTrigger.Delivered, OrderState.Completed);

            sm.Configure(OrderState.Completed)
                .Permit(OrderTrigger.OrderCancelled, OrderState.Cancelled);

        }

        public void Fire(OrderTrigger trigger)
        {
            var oldState = State;
            sm.Fire(trigger);
            PrintState(oldState);
        }

        private void PrintState(OrderState oldState)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"State changed from to [{oldState.ToString()}] to [{State.ToString()}]");
            Console.ResetColor();
        }

        public OrderState State => sm.State;

    }
}
