using MassTransit;
using RabbitMQExchanges.BuildingBlocks;
using RabbitMQExchanges.BuildingBlocks.Model;

namespace DirectExchangeApp.Producer
{
    public class MyMessageConsumer : IConsumer<UserRegisteredEvent>
    {
        public Task Consume(ConsumeContext<UserRegisteredEvent> context)
        {
            var message = context.Message;
            Console.WriteLine("Received message: {0}", message.LastName);
            return Task.CompletedTask;
        }
    }

    internal class Program
    {
        static async Task Main(string[] args)
        {
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
            finally
            {
                await busControl.StopAsync();
            }
        }
    }
}
