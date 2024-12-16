using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;
using System.Threading;

class Consumer1
{
    static void Main(string[] args)
    {
        var factory = new ConnectionFactory() { HostName = "localhost" };
        using (var connection = factory.CreateConnection())
        using (var channel = connection.CreateModel())
        {
            channel.QueueDeclare(queue: "computation_queue", durable: false, exclusive: false, autoDelete: false, arguments: null);
            channel.QueueBind(queue: "computation_queue", exchange: "direct_exchange", routingKey: "computation");

            channel.BasicQos(prefetchSize: 0, prefetchCount: 1, global: false);

            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                Console.WriteLine(" [x] Received {0}", message);

                //Processing time
                Thread.Sleep(100);

                Console.WriteLine(" [x] Done");
              
                channel.BasicAck(deliveryTag: ea.DeliveryTag, multiple: false);
            };

            channel.BasicConsume(queue: "computation_queue", autoAck: false, consumer: consumer);

            Console.WriteLine(" Press Enter.");
            Console.ReadLine();
        }
    }
}
