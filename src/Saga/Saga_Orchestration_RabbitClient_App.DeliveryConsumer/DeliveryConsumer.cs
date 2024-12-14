using RabbitMQ.Client.Events;

namespace Saga_Orchestration_RabbitClient_App.DeliveryConsumer
{
    public class DeliveryConsumer
    {
        private IConnection _connection;
        private IModel _channel;
        const string OrderQueue = "order.Deliver.queue";
        const string OrderStockBindingKey = "order.Deliver";
        const string OrderExchangeName = "orders.exchange";
        const string OrderProccessRoutingKey = "order.process";

        public DeliveryConsumer()
        {
            RabbitMqInit();
        }

        private void RabbitMqInit()
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
        }

        public void Run()
        {
            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                var _message = JsonConvert.DeserializeObject<OrderEvent>(message);

                Console.WriteLine($"Processing Delivery for OrderId: {_message.OrderId} | Type: {_message.Type}");

                //Operation done successfully
                RespondWithDelivered(_message.OrderId);
            };

            _channel.BasicConsume(queue: OrderQueue, autoAck: true, consumer: consumer);

            Console.WriteLine("Delivery Consumer started. Press enter to exit.");
            Console.ReadLine();
        }

        private void RespondWithDelivered(Guid orderId)
        {
             var response = new { Type = "Delivered", OrderId = orderId };
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
