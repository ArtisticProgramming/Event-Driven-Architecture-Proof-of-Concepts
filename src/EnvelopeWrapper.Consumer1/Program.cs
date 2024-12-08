using MassTransit;
using RabbitMQExchanges.BuildingBlocks;
using RabbitMQExchanges.BuildingBlocks.Model;
using RabbitMQExchanges.BuildingBlocks.Service;

namespace EnvelopeWrapper.Consumer1
{
    public class MyMessageConsumer : IConsumer<EnvlopeWrapper>
    {
        public Task Consume(ConsumeContext<EnvlopeWrapper> context)
        {
            Utility.PrintMessagePayloadWithHeaders(context.Message,
                                 context.Headers.GetAll(),
                                 context.CorrelationId,
                                 context.MessageId,
                                 context.CorrelationId);

            Console.WriteLine("[][][][][][][][][][][] UnWrapping The Envlope... [][][][][][][][][][][]");

            UserPasswordCreatedEvent _event = UnWrap(context);

            Utility.PrintMessagePayloadWithHeaders(_event,
                     context.Headers.GetAll(),
                     context.CorrelationId,
                     context.MessageId,
                     context.CorrelationId);


            return Task.CompletedTask;
        }

        private UserPasswordCreatedEvent UnWrap(ConsumeContext<EnvlopeWrapper> context)
        {
            UserPasswordCreatedEvent _event = null;
            if (context.Message.IsEncrypted)
            {
                _event = EnvlopeWrapperService.XmlUnWrapWithDecryption<UserPasswordCreatedEvent>(context.Message.EnvlopeContent);
            }
            else
            {
                _event = EnvlopeWrapperService.XmlUnWrap<UserPasswordCreatedEvent>(context.Message.EnvlopeContent);
            }

            return _event;
        }
    }

    internal class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("This is just a Envelope Wrapper sample app. [Consumer1]");

            var busControl = Bus.Factory.CreateUsingRabbitMq(cfg =>
            {
                cfg.BuildDefulatHost();

                cfg.ReceiveEndpoint("direct_exchange_queue", e =>
                {
                    e.ConfigureConsumeTopology = false;
                    e.Bind("direct_exchange", x => { 
                        x.ExchangeType = "direct"; 
                        x.RoutingKey = "envelopewrapperdirectexchange_key"; 
                    });
                    e.Consumer<MyMessageConsumer>();
                });
            });

            await busControl.StartAsync();

            try
            {
                Console.WriteLine("Consumer is running. Press any key to exit.");
                await Task.Run(() => Console.ReadKey());
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                await Task.Run(() => Console.ReadKey());
            }
            finally
            {
                await busControl.StopAsync();
            }
        }
    }
}
