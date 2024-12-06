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
            var arguments = new Dictionary<string, object>
            {
                { "x-dead-letter-exchange", "dead_letter_exchange" },
                { "x-dead-letter-routing-key", "dead_letter" }
            };

            channel.QueueDeclare(queue: "main_queue", durable: true, exclusive: false, autoDelete: false, arguments: arguments);

            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                Console.WriteLine(" [x] Received {0}", message);

                // Simulate processing failure
                if (message.Contains("Fail"))
                {
                    Console.WriteLine(" [x] Message processing failed, sending to DLQ");
                    channel.BasicNack(ea.DeliveryTag, false, false);
                }
                else
                {
                    Console.WriteLine(" [x] Message processed successfully");
                    channel.BasicAck(ea.DeliveryTag, false);
                }
            };

            channel.BasicConsume(queue: "main_queue", autoAck: false, consumer: consumer);

            Console.WriteLine(" Press [enter] to exit.");
            Console.ReadLine();
        }
    }
}
