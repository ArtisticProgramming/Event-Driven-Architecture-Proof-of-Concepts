using MassTransit;
using RabbitMQExchanges.BuildingBlocks.Model;
using RabbitMQExchanges.BuildingBlocks.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RabbitMQExchanges.BuildingBlocks
{
    public static class PublishExecuteExtensions
    {
        public static Task PublishInEnvlope<T>(this IPublishEndpoint endpoint, T message, Action<PublishContext<EnvlopeWrapper>> callback,
      CancellationToken cancellationToken = default)
      where T : class
        {
            var envlope = EnvlopeWrapperService.XmlWrap(message);
            Console.WriteLine("Message put inside the Envlope");

            return endpoint.Publish<EnvlopeWrapper>(envlope, callback.ToPipe(), cancellationToken);
        }

        public static Task PublishInEnvlopeWithEncryption<T>(this IPublishEndpoint endpoint, T message, Action<PublishContext<EnvlopeWrapper>> callback,
      CancellationToken cancellationToken = default)
      where T : class
        {
            EnvlopeWrapper envlope = EnvlopeWrapperService.XmlWrapWithEncryption(message);
            Console.WriteLine("Message put inside the Envlope");

            return endpoint.Publish<EnvlopeWrapper>(envlope, callback.ToPipe(), cancellationToken);
        }

    }
}
