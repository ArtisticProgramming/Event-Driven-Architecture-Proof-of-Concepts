using MassTransit;
using RabbitMQExchanges.BuildingBlocks;

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


//using MassTransit;
//using RabbitMQExchanges.BuildingBlocks;

//namespace DirectExchangeApp.Producer
//{
//    internal class Program
//    {
//        static async Task Main(string[] args)
//        {
//            var busControl = Bus.Factory.CreateUsingRabbitMq(cfg =>
//            {
//                cfg.BuildDefulatHost();

//            });

//            await busControl.StartAsync();

//            try
//            {
//                while (true)
//                {

//                    Console.ReadKey();
//                }
//            }
//            finally
//            {
//                await busControl.StopAsync();
//            }
//        }
//    }
//}
