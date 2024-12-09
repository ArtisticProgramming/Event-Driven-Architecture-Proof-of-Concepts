using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;
using Newtonsoft.Json;
using EDA_Utilities.Model;

class Program : BaseRabbitMq
{
    static void Main(string[] args)
    {
        try
        {
            Start();

            string exchangeName = "logs_topic_exchange";
            string queueName = "error_logs";

            channel.ExchangeDeclare(exchange: exchangeName, type: "topic");
            channel.QueueDeclare(queue: queueName, durable: false, exclusive: false, autoDelete: false);

            string bindingKey = "*.error";
            channel.QueueBind(queue: queueName, exchange: exchangeName, routingKey: bindingKey);

            Console.WriteLine($"Waiting for messages in {queueName}...");

            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var jsonMessage = Encoding.UTF8.GetString(body);

                var logMessage = JsonConvert.DeserializeObject<LogMessage>(jsonMessage);
                Console.WriteLine($"Received Log: Source={logMessage.Source}, Level={logMessage.LogLevel}, Message={logMessage.Message}");
            };

            channel.BasicConsume(queue: queueName, autoAck: true, consumer: consumer);
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
