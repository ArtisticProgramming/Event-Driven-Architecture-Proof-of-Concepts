using RabbitMQ.Client;
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
            channel.ExchangeDeclare(exchange: exchangeName, type: "topic");

            var logMessages = new[]
            {
                new LogMessage { Source = "MicroserviceX", LogLevel = "info", 
                    Message = "MicroserviceX started successfully" ,Priority="low"},
                new LogMessage { Source = "MicroserviceY", LogLevel = "warning", 
                    Message = "MicroserviceY might be slow" ,Priority="Medium"},
                new LogMessage { Source = "MicroserviceZ", LogLevel = "error", 
                    Message = "MicroserviceZ has an error" ,Priority="High"}
            };

            var conunter = 0;

            foreach (var log in logMessages)
            {
                Thread.Sleep(1000);
                string routingKey = $"{log.Source}.{log.LogLevel}";
                string jsonMessage = JsonConvert.SerializeObject(log);
                var body = Encoding.UTF8.GetBytes(jsonMessage);


                channel.BasicPublish(exchange: exchangeName,
                                     routingKey: routingKey,
                                     basicProperties: null,
                                     body: body);
                conunter++;
                Console.WriteLine($"[#{conunter}] | {DateTime.Now.ToLongTimeString()} | Sent: {jsonMessage} with Routing Key: {routingKey}");
            }

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

