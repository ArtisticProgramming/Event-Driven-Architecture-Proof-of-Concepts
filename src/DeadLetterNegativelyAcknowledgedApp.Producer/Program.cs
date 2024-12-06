using RabbitMQ.Client;
using System;
using System.Text;
using System.Collections.Generic;

class Producer
{
    static void Main()
    {
        var factory = new ConnectionFactory() { HostName = "localhost" };
        using (var connection = factory.CreateConnection())
        using (var channel = connection.CreateModel())
        {
            ConfigureDeadLetterReciver(channel);

            //Configuring Arguments of Main Queue
            var arguments = new Dictionary<string, object>
            {
                { "x-dead-letter-exchange", "dead_letter_exchange" },
                { "x-dead-letter-routing-key", "dead_letter" }
            };

            channel.QueueDeclare(queue: "main_queue", durable: true, exclusive: false, autoDelete: false, arguments: arguments);

            while (true)
            {
                var message = "Hello!";

                //Message would be failed 50 persent of time.
                if (RandomBool())
                {
                    message = message + " Fail ";
                }

                var body = Encoding.UTF8.GetBytes(message);

                channel.BasicPublish(exchange: "", routingKey: "main_queue", basicProperties: null, body: body);
                Console.WriteLine(" [x] Sent {0}", message);
                Console.ReadKey();
            }
        }
    }

    public static bool RandomBool()
    {
        return new Random().Next(100) < 50;
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
