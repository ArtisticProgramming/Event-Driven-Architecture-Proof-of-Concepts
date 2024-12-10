using EDA_Utilities;
using RabbitMQ.Client;
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
                queue: queueName,
                durable: true,
                exclusive: false,
                autoDelete: false,
                arguments: new Dictionary<string, object>
                {
                //max priority (0-10)
                { "x-max-priority", 10 }
                });

            var messages = new[]
            {
                new { Body = "Low priority message", Priority = 1 },
                new { Body = "Low priority message", Priority = 1 },
                new { Body = "Low priority message ", Priority = 2 },
                new { Body = "Low priority message", Priority = 3 },
                new { Body = "Medium priority message ", Priority = 5 },
                new { Body = "Medium priority message ", Priority = 7 },
                new { Body = "High priority message",  Priority = 9 },
                new { Body = "Low priority message", Priority = 1 },
                new { Body = "High priority message" , Priority = 10 },
                new { Body = "Medium priority message", Priority = 6 },
                new { Body = "Medium priority message", Priority = 5 },
            };

            Console.WriteLine("Proccessing by the highest priority");

            while (true)
            {
                foreach (var message in messages)
                {
                    var body = GetByts(message.Body);

                    var properties = channel.CreateBasicProperties();

                    //set the Priority
                    properties.Priority = (byte)message.Priority;

                    channel.BasicPublish(
                        exchange: "",
                        routingKey: queueName,
                        basicProperties: properties,
                        body: body);

                    Console.WriteLine($" [x] Sent '{message.Body}' with priority {message.Priority}");
                }
                Console.ReadLine();
            }


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
