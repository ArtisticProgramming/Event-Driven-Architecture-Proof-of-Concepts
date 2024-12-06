using MassTransit;
using RabbitMQExchanges.BuildingBlocks;
using RabbitMQExchanges.BuildingBlocks.Model;

namespace DirectExchangeApp.Producer
{
    public class MyMessageConsumer : IConsumer<UserRegisteredEvent>
    {
        public Task Consume(ConsumeContext<UserRegisteredEvent> context)
        {
            Utility.PrintMessagePayloadWithHeaders(context);

            return Task.CompletedTask;
        }
    }

    internal class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("This is just a Direct Exchange sample app. [Consumer1]");

            var busControl = Bus.Factory.CreateUsingRabbitMq(cfg =>
            {

                cfg.BuildDefulatHost();

                cfg.ReceiveEndpoint("direct_exchange_queue", e =>
                {
                    e.ConfigureConsumeTopology = false;
                    e.Bind("direct_exchange", x => { x.ExchangeType = "direct"; x.RoutingKey = "directexchange_key"; });
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
                Console.WriteLine($"Error connecting to RabbitMQ: {ex.Message}");
            }
            finally
            {
                await busControl.StopAsync();
            }
        }
    }
}
