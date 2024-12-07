using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Drawing;
using System.Text;
using System.Threading;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Timers;

class Producer
{
    static void Main(string[] args)
    {
        var factory = new ConnectionFactory() { HostName = "localhost" };
        using (var connection = factory.CreateConnection())
        using (var channel = connection.CreateModel())
        {
            channel.QueueDeclare(queue: "computation_queue", durable: false, exclusive: false, autoDelete: false, arguments: null);
            channel.QueueBind(queue: "computation_queue", exchange: "direct_exchange", routingKey: "computation");


            //Control how messages are sent to consumers to ensure that they don't get overwhelmed.
            //prefetchSize: 0: Ignore the size limit of messages.
            //prefetchCount: 1: Only send one message at a time to the consumer. The consumer must acknowledge the message before receiving the next one.
            //global: false: Apply this setting to individual consumers rather than the whole channel.
            channel.BasicQos(prefetchSize: 0, prefetchCount: 1, global: false);

            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                Console.WriteLine(" [x] Received {0}", message);

                // Processing
                Thread.Sleep(1000); 

                Console.WriteLine(" [x] Done");

                // Acknowledge
                //The deliveryTag is a unique identifier for the message delivered to the consumer.
                //It's used by RabbitMQ to track which messages have been received by the consumer.
                channel.BasicAck(deliveryTag: ea.DeliveryTag, multiple: false);
            };

            channel.BasicConsume(queue: "computation_queue", autoAck: false, consumer: consumer);

            Console.WriteLine(" Press [enter] to exit.");
            Console.ReadLine();
        }
    }
}
