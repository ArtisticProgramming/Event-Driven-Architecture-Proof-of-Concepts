using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;

class Producer
{
    static void Main(string[] args)
    {
        var factory = new ConnectionFactory() { HostName = "localhost" };
        using (var connection = factory.CreateConnection())
        using (var channel = connection.CreateModel())
        {
         
            channel.ExchangeDeclare(exchange: "direct_exchange", type: ExchangeType.Direct,durable:true);

            while (true)
            {
                var message = $"Hello! {DateTime.Now.ToLongTimeString()} | {Guid.NewGuid()}";
                var body = Encoding.UTF8.GetBytes(message);

                channel.BasicPublish(exchange: "direct_exchange", 
                                     routingKey: "computation", 
                                     basicProperties: null, 
                                     body: body);

                Console.WriteLine(" [x] Sent {0}", message);

                Console.ReadLine();
            }
        }
    }
}
