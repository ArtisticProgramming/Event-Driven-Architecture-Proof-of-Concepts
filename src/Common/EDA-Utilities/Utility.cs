using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Threading.Tasks;

namespace RabbitMQExchanges.BuildingBlocks
{
    public static class Utility
    {
        public static void PrintMessagePayloadWithHeaders(object? _message, 
            IEnumerable<KeyValuePair<string,object>> _headers,
            Guid? _correlationId,
            Guid? _messageId,
            Guid? _conversationId) 
        {
            var message = _message;

            var messageJson = JsonConvert.SerializeObject(message, Formatting.Indented);

            var headers = _headers;
            var headersJson = JsonConvert.SerializeObject(headers, Formatting.Indented);

            var conversationId = _conversationId;
            var messageId = _messageId;
            var correlationId = _correlationId;

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
