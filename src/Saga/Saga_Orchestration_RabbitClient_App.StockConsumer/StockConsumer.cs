using RabbitMQ.Client.Events;

namespace Saga_Orchestration_RabbitClient_App.StockConsumer
{
    public class StockConsumer
    {
        private IConnection _connection;
        private IModel _channel;
        const string OrderStockQueue = "order.stock.queue";
        const string OrderStockBindingKey = "order.stock";
        const string OrderExchangeName = "orders.exchange";
        const string OrderProccessRoutingKey = "order.process";

        public StockConsumer()
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

            _channel.QueueDeclare(queue: OrderStockQueue, durable: false, exclusive: false, autoDelete: false);
            _channel.QueueBind(queue: OrderStockQueue, exchange: OrderExchangeName, routingKey: OrderStockBindingKey);
        }

        public void Run()
        {
            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                var _message = JsonConvert.DeserializeObject<OrderEvent>(message);

                Console.WriteLine($"Processing stock for OrderId: {_message.OrderId} | Type: {_message.Type}");

                //Operation done successfully
                var succeeded = false;

                if (succeeded)
                {
                    RespondWithStockConfirmed(_message.OrderId);
                }
                else
                {
                    RespondWithStockRejected(_message.OrderId);
                }
            };

            _channel.BasicConsume(queue: OrderStockQueue, autoAck: true, consumer: consumer);

            Console.WriteLine("Stock Consumer started. Press enter to exit.");
            Console.ReadLine();
        }

        private void RespondWithStockConfirmed(Guid orderId)
        {
            var response = new { Type = "StockConfirmed", OrderId = orderId };
            var body = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(response));
            _channel.BasicPublish(exchange: OrderExchangeName,
                                  routingKey: OrderProccessRoutingKey,
                                  basicProperties: null,
                                  body: body);
        }

        private void RespondWithStockRejected(Guid orderId)
        {
            var response = new { Type = "StockRejected", OrderId = orderId };
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
