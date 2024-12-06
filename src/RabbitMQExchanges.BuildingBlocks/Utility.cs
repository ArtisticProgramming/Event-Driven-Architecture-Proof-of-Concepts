using MassTransit;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RabbitMQExchanges.BuildingBlocks
{
    public static class Utility
    {
        public static void PrintMessagePayloadWithHeaders<T>(ConsumeContext<T> context) where T : class
        {
            var message = context.Message;

            var messageJson = JsonConvert.SerializeObject(message, Formatting.Indented);

            var headers = context.Headers.GetAll();
            var headersJson = JsonConvert.SerializeObject(headers, Formatting.Indented);

            var messageId = context.MessageId;
            var conversationId = context.ConversationId;
            var correlationId = context.CorrelationId;

            var metadata = new
            {
                MessageId = messageId,
                ConversationId = conversationId,
                CorrelationId = correlationId,
                Headers = headersJson
            };
            var metadataJson = JsonConvert.SerializeObject(metadata, Formatting.Indented);

            Console.WriteLine("Metadata:");
            Console.WriteLine(metadataJson);
            Console.WriteLine("Message:");
            Console.WriteLine(messageJson);
            Console.WriteLine("---------------------------------------------------------");
        }

       
    }
}
