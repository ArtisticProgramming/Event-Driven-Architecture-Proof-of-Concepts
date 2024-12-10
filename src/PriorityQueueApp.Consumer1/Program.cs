using EDA_Utilities;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQExchanges.BuildingBlocks;
using System;
using System.Text;

class Program : BaseRabbitMq
{
    static void Main(string[] args)
    {
        try
        {
            Start();

            var queueName = "priority-queue";

            channel.QueueDeclare(
               queue: "priority-queue",
               durable: true,
               exclusive: false,
               autoDelete: false,
               arguments: new Dictionary<string, object>
               {
                { "x-max-priority", 10 } 
               });

            var consumer = new EventingBasicConsumer(channel);

            consumer.Received += (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = GetString(body);

                PrintMessageHeader(ea.BasicProperties.Headers);

                Console.WriteLine($" [x] Received '{message}' with the Priority: {ea.BasicProperties.Priority}");
            };

            channel.BasicConsume(
                queue: queueName,
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
