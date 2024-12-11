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
            var conunter = 0;

            string exchangeName = "fanout_logs_exchange";
            channel.ExchangeDeclare(exchange: exchangeName, type: ExchangeType.Fanout);


            while (true)
            {
                string message = $"Hello, RabbitMQ Fanout! | {DateTime.Now.ToString()}";
                var body = GetBytes(message);

                channel.BasicPublish(exchange: exchangeName,
                                      // routingKey Not used for Fanout
                                      routingKey: "", 
                                      basicProperties: null,
                                      body: body);

                Console.WriteLine($"[x] Sent: {message}");
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

