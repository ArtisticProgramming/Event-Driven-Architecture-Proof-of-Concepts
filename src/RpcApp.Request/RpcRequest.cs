using EDA_Utilities;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RpcApp.Request
{
    internal class RpcRequest : BaseRabbitMq
    {
        private readonly string replyQueueName;
        private readonly EventingBasicConsumer consumer;
        private readonly IBasicProperties props;

        public RpcRequest()
        {
            Start();

            replyQueueName = channel.QueueDeclare().QueueName;
            Console.WriteLine($"Reply Queue Name: {replyQueueName}");
            consumer= new EventingBasicConsumer(channel);
            consumer = new EventingBasicConsumer(channel);

            var correlationId = Guid.NewGuid().ToString();
            props= channel.CreateBasicProperties();
            props.ReplyTo=replyQueueName;
            props.CorrelationId=correlationId;


            consumer.Received += (model, eq) =>
            {
                //Check CorrelationIds of both side.
                if (eq.BasicProperties.CorrelationId == correlationId)
                {

                    var res = GetString(eq.Body.ToArray());
                    Console.WriteLine($"{DateTime.Now.ToLongTimeString()} | Response Recived: '{res}'");
                }

            };

            channel.BasicConsume(replyQueueName, autoAck: true, consumer);
        }

        public void Close()
        {
            Stop();
        }

        public void Call(string message)
        {
            var messageBytes = GetByts(message);
            channel.BasicPublish(exchange: "", routingKey: "rpc_queue", basicProperties: props, body: messageBytes);
            Console.WriteLine($"Sent request: {message}");
        }

    }
}
