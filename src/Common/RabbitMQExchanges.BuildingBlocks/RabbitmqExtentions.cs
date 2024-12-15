using MassTransit;
using RabbitMQExchanges.BuildingBlocks;
using RabbitMQExchanges.BuildingBlocks.Service;

namespace RabbitMQExchanges.BuildingBlocks
{
    public static class RabbitmqExtentions
    {
        public static void BuildDefulatHost(this IRabbitMqBusFactoryConfigurator cfg)
        {
            Console.WriteLine("Connecting to RabbitMq...");

            cfg.Host("localhost", "/", h =>
            {
                h.Username("guest");
                h.Password("guest");
            });
        }
    }
}
