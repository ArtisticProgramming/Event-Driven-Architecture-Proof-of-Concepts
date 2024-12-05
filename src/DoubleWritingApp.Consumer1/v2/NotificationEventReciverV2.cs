using DoubleWritingApp.Producer.v2;
using MassTransit;
using Newtonsoft.Json;
using System.Data;

namespace DoubleWritingApp.Consumer1.v2
{
    public class NotificationEventReciverV2 : IConsumer<IEmailEvent>
    {
        public Task Consume(ConsumeContext<IEmailEvent> context)
        {
            Thread.Sleep(100);
            Console.WriteLine("------------------------v2----------------------------");
            var message = context.Message;
            var messageJson = JsonConvert.SerializeObject(message, Formatting.Indented);
            var messageId = context.MessageId;
            var conversationId = context.ConversationId;
            var correlationId = context.CorrelationId;

            var metadata = new
            {
                MessageId = messageId,
                ConversationId = conversationId,
                CorrelationId = correlationId,
            };

            var metadataJson = JsonConvert.SerializeObject(metadata, Formatting.Indented);

            // Print the results
            Console.WriteLine("Message:");
            Console.WriteLine(messageJson);
            Console.WriteLine("Metadata:");
            Console.WriteLine(metadataJson);
            Console.WriteLine("------------------------------------------------------");

            return Task.CompletedTask;

        }
    }
}