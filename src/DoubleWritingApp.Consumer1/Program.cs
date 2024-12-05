using MassTransit.RabbitMqTransport;
using MassTransit;
using DoubleWritingApp.Consumer1.v1;
using DoubleWritingApp.Consumer1.v2;


namespace DoubleWritingApp.Consumer1
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            var busControl = Bus.Factory.CreateUsingRabbitMq(cfg =>
            {
                cfg.Host("localhost", "/", c =>
                {
                    c.Username("guest");
                    c.Password("guest");
                });

                //older version
                cfg.ReceiveEndpoint("email_event_q_v1", e =>
                {
                    e.ConfigureConsumeTopology = false;
                    e.Bind("email_event_exchange_v1", x => { x.ExchangeType = "fanout"; });
                    e.Consumer<NotificationEventReciverV1>();
                });

                //newer version
                cfg.ReceiveEndpoint("email_event_q_v2", e =>
                {
                    e.ConfigureConsumeTopology = false;
                    e.Bind("email_event_exchange_v2", x => { x.ExchangeType = "fanout"; });
                    e.Consumer<NotificationEventReciverV2>();
                });
                
            });

            await busControl.StartAsync();

            try
            {
                Console.WriteLine("Consumer is running. Press any key to exit");
                await Task.Run(() => Console.ReadKey());
            }
            finally { await busControl.StopAsync(); }
        }
    }
}
