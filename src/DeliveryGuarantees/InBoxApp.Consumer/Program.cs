using DeliveryGuarantees.Repository;
using InBoxApp.Consumer.Service;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using RabbitMQ.Client.Events;
using RabbitMQExchanges.BuildingBlocks.Model;
using System;

class Program : BaseRabbitMq
{
    static void Main(string[] args)
    {

        LogInfo(@"
$$$$$$\           $$$$$$$\                      
\_$$  _|          $$  __$$\                     
  $$ |  $$$$$$$\  $$ |  $$ | $$$$$$\  $$\   $$\ 
  $$ |  $$  __$$\ $$$$$$$\ |$$  __$$\ \$$\ $$  |
  $$ |  $$ |  $$ |$$  __$$\ $$ /  $$ | \$$$$  / 
  $$ |  $$ |  $$ |$$ |  $$ |$$ |  $$ | $$  $$<  
$$$$$$\ $$ |  $$ |$$$$$$$  |\$$$$$$  |$$  /\$$\ 
\______|\__|  \__|\_______/  \______/ \__/  \__|

");

        try
        {
            Start();

            string exchangeName = "DeliveryGuarantees_exchange";
            string queueName = "UserRegisteredEvent-queue";
            string bindingKey = "delivery-guarantees";

            channel.ExchangeDeclare(exchange: exchangeName, type: ExchangeType.Direct);
            channel.QueueDeclare(queue: queueName, durable: false, exclusive: false, autoDelete: false);
            channel.QueueBind(queue: queueName, exchange: exchangeName, routingKey: bindingKey);

            using (var db = new AppDbContext())
            {
                var inboxService = new InBoxService(db);
                var dispatcher = new EventDispatcher();
                var inboxProcessor = new InboxProcessor(db, dispatcher);

                var consumer = new EventingBasicConsumer(channel);
                consumer.Received += (model, ea) =>
                {
                    var body = ea.Body.ToArray();
                    var message = Encoding.UTF8.GetString(body);
                    var messageId = ea.BasicProperties.MessageId;
                    var correlationId = ea.BasicProperties.CorrelationId;
                    var type = ea.BasicProperties.Type;

                    Console.WriteLine($"Message Recived. {message} | Type:{type} | correlationId:{correlationId}");

                    Console.WriteLine($"Total Number of UnProccessed InBox Messages:{db.InBoxMessages.Where(x=>x.ProcessedAt==null)
                        .Count()}");

                    inboxService.SaveToInBox(type,message, correlationId);

                    channel.BasicAck(ea.DeliveryTag, false);
                };

                channel.BasicConsume(queue: queueName, autoAck: false, consumer: consumer);

                while (true)
                {
                    Thread.Sleep(10000);

                    Console.WriteLine("Inbox Processor running...");
                    inboxProcessor.ProcessInboxMessages();
                }
            }
        }
        catch (Exception ex)
        {
            LogError(ex.ToString());
        }
        finally
        {
            Stop();
        }
    }
}


