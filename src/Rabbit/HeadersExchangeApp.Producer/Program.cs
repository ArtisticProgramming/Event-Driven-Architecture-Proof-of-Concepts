using RabbitMQ.Client;
using System;
using System.Text;
using Newtonsoft.Json;
using EDA_Utilities.Model;
using EDA_Utilities;

class Program : BaseRabbitMq
{
    static void Main(string[] args)
    {
        try
        {
            Start();

            var exchangeName = "header-exchange";
            channel.ExchangeDeclare(exchange: exchangeName, type: "headers");

            var properties = channel.CreateBasicProperties();

            var messages = new[]{
               new { Log="info log" , Priority="Low", Type ="Info"},
               new { Log="info log From Z app" , Priority="Medium", Type ="Info"},
               new { Log="warning log" , Priority="Medium", Type ="Warning"},
               new { Log="warning log From Y App" , Priority="High", Type ="Warning"},
               new { Log="error log" , Priority="High", Type ="Error"},
               new { Log="error log from X app" , Priority="Low", Type ="Error"},
            };

            while (true)
            {
                foreach (var message in messages)
                {
                    properties.Headers = new Dictionary<string, object>
                    {
                        { "type", message.Type },
                        { "priority", message.Priority }
                    };

                    var body = Encoding.UTF8.GetBytes(message.Log);

                    channel.BasicPublish(
                        exchange: exchangeName,
                        routingKey: "",
                        basicProperties: properties, body: body);
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

