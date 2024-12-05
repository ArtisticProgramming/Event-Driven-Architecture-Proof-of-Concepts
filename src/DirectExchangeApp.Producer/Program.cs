using MassTransit;
using RabbitMQExchanges.BuildingBlocks;
using RabbitMQExchanges.BuildingBlocks.Model;

namespace DirectExchangeApp.Producer
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            var busControl = Bus.Factory.CreateUsingRabbitMq(cfg =>
            {
                cfg.BuildDefulatHost();

                cfg.Message<UserRegisteredEvent>(x =>
                {
                    x.SetEntityName("direct_exchange");
                });

                cfg.Publish<UserRegisteredEvent>(x => x.ExchangeType = "direct");


            });

            await busControl.StartAsync();

            try
            {
                while (true)
                {
                    var _event = new UserRegisteredEvent("John", "Fisher", 25);

                    await busControl.Publish(_event,
                         x =>
                         {
                             x.SetRoutingKey("directexchange_key");
                         });
                    ; 
                    Console.WriteLine("event sent");

                    Console.ReadKey();
                }
            }
            finally
            {
                await busControl.StopAsync();
            }
        }
    }
}
