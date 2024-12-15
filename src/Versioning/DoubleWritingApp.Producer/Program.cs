using DoubleWritingApp.Producer.v1;
using DoubleWritingApp.Producer.v2;
using MassTransit;

namespace DoubleWritingApp.Producer
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            var busControl = Bus.Factory.CreateUsingRabbitMq(cfg =>
            {
                cfg.Host("localhost", "/", h =>
                {
                    h.Username("guest");
                    h.Password("guest");
                });

                cfg.Message<v1.IEmailEvent>(x =>
                {
                    x.SetEntityName("email_event_exchange_v1");
                });
                cfg.Message<v2.IEmailEvent>(x =>
                {
                    x.SetEntityName("email_event_exchange_v2");
                });

                cfg.PublishTopology.GetMessageTopology<v1.IEmailEvent>().ExchangeType = "fanout";
                cfg.PublishTopology.GetMessageTopology<v2.IEmailEvent>().ExchangeType = "fanout";
            });

            await busControl.StartAsync();

            try
            {
                while (true)
                {
                    var eventMessage = new v1.EmailEvent("recipientv1@example.com", "Test Subject v1", "Hello, this is a test email! v1");
                    var eventMessageWithSignature = new v2.EmailEvent("recipient2@example.com", 
                        "Test Subject v2", 
                        "Hello, this is a test email! v2",
                        new Singnuture("Mostafa", "Software Developer"));

                    await busControl.Publish<v1.IEmailEvent>(eventMessage);
                    Console.WriteLine($"{DateTime.Now.ToLongTimeString()} Email v1 event published.");

                    await busControl.Publish<v2.IEmailEvent>(eventMessageWithSignature);
                    Console.WriteLine($"{DateTime.Now.ToLongTimeString()} Email v2 event published.");
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
