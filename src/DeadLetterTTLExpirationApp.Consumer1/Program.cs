using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;
using System.Collections.Generic;

class Consumer
{
    static void Main()
    {
        var factory = new ConnectionFactory() { HostName = "localhost" };
        using (var connection = factory.CreateConnection())
        using (var channel = connection.CreateModel())
        {
            // Declare Main Queue with Dead Letter Exchange configuration and TTL
            var arguments = new Dictionary<string, object>
            {
                { "x-dead-letter-exchange", "dead_letter_exchange" },
                { "x-dead-letter-routing-key", "dead_letter" },
                { "x-message-ttl", 2000 }
            };

            channel.QueueDeclare(queue: "main_queue_ttl", durable: true, exclusive: false, autoDelete: false, arguments: arguments);

            channel.QueueDeclare(queue: "dead_letter_queue", durable: true, exclusive: false, autoDelete: false, arguments: null);

            var consumer = new EventingBasicConsumer(channel);
           
            MainConsumer(channel, consumer);

            var deadLetterConsumer = new EventingBasicConsumer(channel);

            DeadLetterConsumer(channel, deadLetterConsumer);

            channel.BasicConsume(queue: "main_queue_ttl", autoAck: false, consumer: consumer);
            channel.BasicConsume(queue: "dead_letter_queue", autoAck: false, consumer: deadLetterConsumer);

            Console.WriteLine(" Press [enter] to exit.");
            Console.ReadLine();
        }
    }

    private static void MainConsumer(IModel channel, EventingBasicConsumer consumer)
    {
        consumer.Received += (model, ea) =>
        {
            System.Threading.Thread.Sleep(10000);

            var body = ea.Body.ToArray();
            var message = Encoding.UTF8.GetString(body);
            Console.WriteLine(" [x] Received {0}", message);

            // Process message normally
            Console.WriteLine(" [x] Message processed: {0}", message);
            channel.BasicAck(ea.DeliveryTag, false);
        };
    }

    private static void DeadLetterConsumer(IModel channel, EventingBasicConsumer deadLetterConsumer)
    {
        deadLetterConsumer.Received += (model, ea) =>
        {
            var body = ea.Body.ToArray();
            var message = Encoding.UTF8.GetString(body);
            Console.WriteLine(" [x] Dead Letter Queue - Received {0}", message);

            channel.BasicAck(ea.DeliveryTag, false);
        };
    }
}
