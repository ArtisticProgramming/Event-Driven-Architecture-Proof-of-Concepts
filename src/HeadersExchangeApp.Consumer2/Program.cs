using RabbitMQ.Client;
using System;
using System.Text;
using Newtonsoft.Json;
using EDA_Utilities.Model;
using EDA_Utilities;
using RabbitMQ.Client.Events;

class Program : BaseRabbitMq
{
    static void Main(string[] args)
    {
        try
        {
            Start();

            var exchangeName = "header-exchange";
            var queueName = "header-queue-medium-warning";

            channel.ExchangeDeclare(exchange: exchangeName, type: "headers");
            channel.QueueDeclare(queue: queueName, durable: true, exclusive: false, autoDelete: false);

            var bindingHeaders = new Dictionary<string, object>
        {
            { "priority", "Medium" },
            { "type", "Warning" },
            { "x-match", "all" }
        };
            channel.QueueBind(queue: queueName, exchange: exchangeName, routingKey: "", arguments: bindingHeaders);

            var consumer = new EventingBasicConsumer(channel);

            consumer.Received += (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);

                Console.WriteLine($" [x] Received message: {message}");

                PrintMessageHeader(ea.BasicProperties.Headers);

            };

            channel.BasicConsume(queue: queueName, autoAck: true, consumer: consumer);

            Console.WriteLine(" Press enter to exit.");
            Console.ReadLine();

        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.ToString());
        }
        finally
        {
            Stop();
        }
    }
}

