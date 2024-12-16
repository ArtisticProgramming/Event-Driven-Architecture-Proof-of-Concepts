using RabbitMQ.Client.Events;

namespace Saga_Orchestration_RabbitClient_App.PaymentConsumer
{
    public class PaymentConsumer
    {
        private readonly IConnection _connection;
        private readonly IModel _channel;
        const string OrderQueue = "order.place.queue";
        const string OrderStockBindingKey = "order.place";
        const string OrderExchangeName = "orders.exchange";
        const string OrderProccessRoutingKey = "order.process";
        const string OrderCancelledExchangeName = "order.cancelled.fanout.exchange";
        private string OrderCancelledQueue { get;  set; }

        public PaymentConsumer()
        {
            var factory = new ConnectionFactory() { HostName = "localhost" };
            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();

            _channel.ExchangeDeclare(exchange: OrderExchangeName,
                type: ExchangeType.Direct,
                durable: true,
                autoDelete: false,
                arguments: null);

    

            _channel.QueueDeclare(queue: OrderQueue, durable: false, exclusive: false, autoDelete: false);
            _channel.QueueBind(queue: OrderQueue, exchange: OrderExchangeName, routingKey: OrderStockBindingKey);

            OrderCancellationRabbitMqConfig();
        }
        private void OrderCancellationRabbitMqConfig()
        {
            _channel.ExchangeDeclare(exchange: OrderCancelledExchangeName,
                type: ExchangeType.Fanout,
                durable: true,
                autoDelete: false,
                arguments: null);
            //OrderCancelled
            OrderCancelledQueue = _channel.QueueDeclare().QueueName;
            //_channel.QueueDeclare(queue: OrderCancelledQueue, durable: false, exclusive: false, autoDelete: false);
            _channel.QueueBind(queue: OrderCancelledQueue, exchange: OrderCancelledExchangeName, routingKey: "");
        }

        public void Run()
        {
            var consumer = new EventingBasicConsumer(_channel);

            consumer.Received += (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                var paymentMessage = JsonConvert.DeserializeObject<OrderEvent>(message);

                Console.WriteLine($"Processing payment for OrderId: {paymentMessage.OrderId} | Type: {paymentMessage.Type}");

                var paymentAccepted = true;

                if (paymentAccepted)
                {
                    RespondWithPaymentAccepted(paymentMessage.OrderId);
                }
                else
                {
                    RespondWithPaymentRejected(paymentMessage.OrderId);
                }
            };

            _channel.BasicConsume(queue: "order.place.queue", autoAck: true, consumer: consumer);

            var cancelledConsumer = new EventingBasicConsumer(_channel);
            cancelledConsumer.Received += (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                var paymentMessage = JsonConvert.DeserializeObject<OrderEvent>(message);

                Console.WriteLine($"*** Order Cancelled. OrderId: {paymentMessage.OrderId} | Type: {paymentMessage.Type}");
            };

            _channel.BasicConsume(queue: OrderCancelledQueue, autoAck: true, consumer: cancelledConsumer);

            Console.WriteLine("Payment Consumer started. Press [enter] to exit.");
            Console.ReadLine();
        }

        private void RespondWithPaymentAccepted(Guid orderId)
        {
            var response = new { Type = "PaymentAccepted", OrderId = orderId };
            var body = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(response));
            _channel.BasicPublish(exchange: OrderExchangeName,
                                  routingKey: OrderProccessRoutingKey,
                                  basicProperties: null,
                                  body: body);
        }

        private void RespondWithPaymentRejected(Guid orderId)
        {
            var response = new { Type = "PaymentRejected", OrderId = orderId };
            var body = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(response));

            _channel.BasicPublish(exchange: OrderExchangeName,
                                  routingKey: OrderProccessRoutingKey,
                                  basicProperties: null,
                                  body: body);
        }

        public void Close()
        {
            _connection.Close();
        }

    }
}
