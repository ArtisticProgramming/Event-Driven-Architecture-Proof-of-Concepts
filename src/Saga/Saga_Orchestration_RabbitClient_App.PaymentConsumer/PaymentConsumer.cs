using RabbitMQ.Client.Events;

namespace Saga_Orchestration_RabbitClient_App.PaymentConsumer
{
    public class PaymentConsumer
    {
        private readonly IConnection _connection;
        private readonly IModel _channel;

        public PaymentConsumer()
        {
            var factory = new ConnectionFactory() { HostName = "localhost" };
            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();

            var exchangeName = "orders.exchange";
            var queueName = "order.place.queue";
            string bindingKey = "order.place";

            _channel.ExchangeDeclare(exchange: exchangeName,
                type: ExchangeType.Direct,
                durable: true,
                autoDelete: false,
                arguments: null);

            _channel.QueueDeclare(queue: queueName, durable: false, exclusive: false, autoDelete: false);
            _channel.QueueBind(queue: queueName, exchange: exchangeName, routingKey: bindingKey);

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

            Console.WriteLine("Payment Consumer started. Press [enter] to exit.");
            Console.ReadLine();
        }

        private void RespondWithPaymentAccepted(Guid orderId)
        {
            var response = new { Type = "PaymentAccepted", OrderId = orderId };
            var body = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(response));
            _channel.BasicPublish(exchange: "orders.exchange",
                                  routingKey: "order.process",
                                  basicProperties: null,
                                  body: body);
        }

        private void RespondWithPaymentRejected(Guid orderId)
        {
            var response = new { Type = "PaymentRejected", OrderId = orderId };
            var body = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(response));

            _channel.BasicPublish(exchange: "orders.exchange",
                                  routingKey: "order.process",
                                  basicProperties: null,
                                  body: body);
        }

        public void Close()
        {
            _connection.Close();
        }

    }
}
