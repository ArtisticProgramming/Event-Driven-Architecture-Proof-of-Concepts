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
            string exchangeName = "fanout_logs_exchange";

            //Random Uniqe Name
            string queueName = channel.QueueDeclare().QueueName;

            channel.ExchangeDeclare(exchange: exchangeName, type: ExchangeType.Fanout);
            channel.QueueBind(queue: queueName,
                              exchange: exchangeName,
                              routingKey: "");

            Console.WriteLine("Waiting for messages...");

            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                Console.WriteLine($" [x] Received: {message}");
            };
            channel.BasicConsume(queue: queueName,
                                 autoAck: true,
                                 consumer: consumer);

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

