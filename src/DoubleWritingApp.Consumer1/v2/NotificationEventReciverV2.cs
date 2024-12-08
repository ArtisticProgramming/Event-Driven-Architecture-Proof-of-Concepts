using DoubleWritingApp.Producer.v2;
using MassTransit;
using Newtonsoft.Json;
using RabbitMQExchanges.BuildingBlocks;
using System.Data;

namespace DoubleWritingApp.Consumer1.v2
{
    public class NotificationEventReciverV2 : IConsumer<IEmailEvent>
    {
        public Task Consume(ConsumeContext<IEmailEvent> context)
        {
            Console.WriteLine("-----------------------v2-----------------------------");

            Utility.PrintMessagePayloadWithHeaders(context.Message, 
                                                   context.Headers.GetAll(), 
                                                   context.CorrelationId,
                                                   context.MessageId, 
                                                   context.CorrelationId);

            return Task.CompletedTask;

        }
    }
}