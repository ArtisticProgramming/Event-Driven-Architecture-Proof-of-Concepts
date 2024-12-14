using RabbitMQ.Client.Events;
using System.Threading.Channels;

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
        const string OrderCancelledExchangeName = "order.cancelled.fanout.exchange";
        private string OrderCancelledQueue { get; set; }

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

            var cancelledConsumer = new EventingBasicConsumer(_channel);
            cancelledConsumer.Received += (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                var _message = JsonConvert.DeserializeObject<OrderEvent>(message);

                Console.WriteLine($"*** Order Cancelled. OrderId: {_message.OrderId} | Type: {_message.Type}");
            };

            _channel.BasicConsume(queue: OrderCancelledQueue, autoAck: true, consumer: cancelledConsumer);

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
