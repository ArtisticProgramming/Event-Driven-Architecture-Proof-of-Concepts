using RabbitMQ.Client.Events;
using Saga_Orchestration_RabbitClient_App.OrderOrchestrationProducer.OrderStateMachine;
using System.Threading.Channels;
using OrderStateMachineService = Saga_Orchestration_RabbitClient_App.OrderOrchestrationProducer.OrderStateMachine.OrderStateMachine;

namespace Saga_Orchestration_RabbitClient_App.OrderOrchestrationProducer
{
    public class OrderSagaOrchestrator
    {
        private readonly OrderStateMachineService _stateMachine;
        private readonly RabbitMqServiceBus _rabbitMqServiceBusService;

        public OrderSagaOrchestrator()
        {

            _stateMachine = new OrderStateMachineService();
            _rabbitMqServiceBusService= new RabbitMqServiceBus();
        }

        public void Run()
        {
            PlaceOrder();
        }

        private void PlaceOrder()
        {
            _stateMachine.Fire(OrderTrigger.PlaceOrder);
            var _event = new { OrderId = Guid.NewGuid()};
            _rabbitMqServiceBusService.PublishMessage("orders.exchange", "order.place", _event);
            _rabbitMqServiceBusService.BasicConsume(queue: "order.process.queue", consumer: CreateConsumer());
            _rabbitMqServiceBusService._channel.BasicQos(prefetchSize: 0, prefetchCount: 1, global: false);

        }

        private EventingBasicConsumer CreateConsumer()
        {
            var consumer = new EventingBasicConsumer(_rabbitMqServiceBusService._channel);
            consumer.Received += (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                var orderEvent = JsonConvert.DeserializeObject<OrderEvent>(message);

                Console.WriteLine($"Order Message Recicived. OrderId:{orderEvent.OrderId}, Type:{orderEvent.Type}");
             
                HandleEvent(orderEvent);
            };
            return consumer;
        }

        private void HandleEvent(OrderEvent orderEvent)
        {
            Console.WriteLine($"HandleEvent called.");

            switch (orderEvent.Type)
            {
                #region Payment
                case "PaymentAccepted":
                    _stateMachine.Fire(OrderTrigger.PaymentAccepted);
                    _rabbitMqServiceBusService.PublishMessage("orders.exchange",
                        "order.stock", orderEvent);
                    break;

                case "PaymentRejected":
                    _stateMachine.Fire(OrderTrigger.PaymentRejected);
                    CompensatePayment(orderEvent);
                    break;
                #endregion

                #region Stock
                case "StockConfirmed":
                    _stateMachine.Fire(OrderTrigger.StockConfirmed);
                    _rabbitMqServiceBusService.PublishMessage("orders.exchange",
                        "order.Deliver", orderEvent);
                    break;

                case "StockRejected":
                    _stateMachine.Fire(OrderTrigger.StockRejected);
                    CompensateStock(orderEvent);
                    break;
                #endregion

                #region Delivered
                case "Delivered":
                    _stateMachine.Fire(OrderTrigger.Delivered);
                    break;
                #endregion

                default:
                    break;
            }
        }

        private void CompensatePayment(OrderEvent orderEvent)
        {
            Console.WriteLine($"CompensatePaymentRejected called");
        }
        private void CompensateStock(OrderEvent orderEvent)
        {
            Console.WriteLine($"CompensateStockRejected called");
        }

        internal void Close()
        {
            _rabbitMqServiceBusService.Close();
        }
    }
}

