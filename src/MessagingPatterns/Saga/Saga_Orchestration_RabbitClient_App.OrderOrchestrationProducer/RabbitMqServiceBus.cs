using RabbitMQ.Client;
using Stateless;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace Saga_Orchestration_RabbitClient_App.OrderOrchestrationProducer
{
    public class RabbitMqServiceBus
    {
        private readonly IConnection _connection;
        public readonly IModel _channel;
        public RabbitMqServiceBus()
        {
            Console.WriteLine("Connecting to Rabbitmq");
            var factory = new ConnectionFactory() { HostName = "localhost" };
            _connection  = factory.CreateConnection();
            _channel = _connection.CreateModel();

            // Declare the exchange
            _channel.ExchangeDeclare(exchange: "orders.exchange",
                type: ExchangeType.Direct,
                durable: true,
                autoDelete: false,
                arguments: null);

            // Declare the fanout exchange
            _channel.ExchangeDeclare(exchange: "order.cancelled.fanout.exchange",
                type: ExchangeType.Fanout,
                durable: true,
                autoDelete: false,
                arguments: null);

            var queueName = "order.process.queue";
            _channel.QueueDeclare(queue: queueName, durable: true, exclusive: false, autoDelete: false);
            _channel.QueueBind(queue: queueName, exchange: "orders.exchange", routingKey: "order.process");
        }

        public void PublishMessage(string exchange, string routingKey, object message)
        {

            var mes = JsonConvert.SerializeObject(message);
            var body = Encoding.UTF8.GetBytes(mes);
            Console.WriteLine($"PublishMessage is calling... | [exchange]:{exchange}, [routingKey]:{routingKey},[message]: {mes} ");
            _channel.BasicPublish(exchange, routingKey, null, body);
        }

        public string BasicConsume(string queue, IBasicConsumer consumer)
        {
           return _channel.BasicConsume(queue: queue, autoAck: true, consumer);
        }

        public void Close()
        {
            _connection.Close();
        }
    }
}
