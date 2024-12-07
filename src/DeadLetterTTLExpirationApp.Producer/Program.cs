using System;
using System.Text;
using System.Collections.Generic;
using RabbitMQ.Client;

class Producer
{
    static void Main()
    {
        IConnectionFactory factory = new ConnectionFactory() { HostName = "localhost" };
        using (IConnection connection = factory.CreateConnection())
        using (IModel channel = connection.CreateModel())
        {
            Setup(channel);

            while (true)
            {
                var message = "Hello!";

                var body = Encoding.UTF8.GetBytes(message);

                channel.BasicPublish(exchange: "", 
                    routingKey: "main_queue_ttl", 
                    basicProperties: null, 
                    body: body);

                Console.WriteLine(" [x] | [TTL-Expriration] | Sent {0}", message);
                Console.ReadKey();
            }
        }
    }

    private static void Setup(IModel channel)
    {
        ConfigureDeadLetterReciver(channel);

        var arguments = new Dictionary<string, object>
            {
                { "x-dead-letter-exchange", "dead_letter_exchange" },
                { "x-dead-letter-routing-key", "dead_letter" },
                { "x-message-ttl", 2000 }
            };

        channel.QueueDeclare(queue: "main_queue_ttl", durable: true, exclusive: false, autoDelete: false, arguments: arguments);
    }

    private static void ConfigureDeadLetterReciver(IModel channel)
    {
        //Dead Letter Exchange
        channel.ExchangeDeclare(exchange: "dead_letter_exchange", type: "direct");
        //Dead Letter Queue
        channel.QueueDeclare(queue: "dead_letter_queue", durable: true, exclusive: false, autoDelete: false, arguments: null);
        //Bind
        channel.QueueBind(queue: "dead_letter_queue", exchange: "dead_letter_exchange", routingKey: "dead_letter");
    }
}
