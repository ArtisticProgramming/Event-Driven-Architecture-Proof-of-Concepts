using MassTransit;
using RabbitMQExchanges.BuildingBlocks;
using RabbitMQExchanges.BuildingBlocks.Model;
using RabbitMQExchanges.BuildingBlocks.Service;

namespace EnvelopeWrapper.Producer
{
    internal class Program
    {
        
        static async Task Main(string[] args)
        {
            Console.WriteLine("This is just a Direct Exchange sample app. [Producer]");

            IBusControl busControl = Bus.Factory.CreateUsingRabbitMq(cfg =>
            {
                cfg.BuildDefulatHost();

                cfg.Message<EnvlopeWrapper>(x =>
                {
                    x.SetEntityName("direct_exchange");
                });

                cfg.Publish<EnvlopeWrapper>(x => x.ExchangeType = "direct");


            });

            await busControl.StartAsync();

            try
            {
                while (true)
                {
                    var _event = new UserPasswordCreatedEvent() { UserName="Mostafa",Password="123!@#$%%%"};

                    await busControl.PublishInEnvlopeWithEncryption(_event,
                         x =>
                         {
                             x.SetRoutingKey("envelopewrapperdirectexchange_key");
                         });
                    ;
                    Console.WriteLine("Event in envlope sent");

                    Console.ReadKey();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error connecting to RabbitMQ: {ex.Message}");
            }
            finally
            {
                await busControl.StopAsync();
            }
        }
    }
}
