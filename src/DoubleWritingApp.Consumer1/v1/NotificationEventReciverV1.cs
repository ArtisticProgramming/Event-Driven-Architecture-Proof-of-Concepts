using DoubleWritingApp.Producer.v1;
using MassTransit;
using Newtonsoft.Json;
using RabbitMQExchanges.BuildingBlocks;
using System.Data;

namespace DoubleWritingApp.Consumer1.v1
{
    public class NotificationEventReciverV1 : IConsumer<IEmailEvent>
    {
        public Task Consume(ConsumeContext<IEmailEvent> context)
        {
            Console.WriteLine("-----------------------v1-----------------------------");

            Utility.PrintMessagePayloadWithHeaders(context.Message,
                                         context.Headers.GetAll(),
                                         context.CorrelationId,
                                         context.MessageId,
                                         context.CorrelationId);

            return Task.CompletedTask;

        }
    }
}