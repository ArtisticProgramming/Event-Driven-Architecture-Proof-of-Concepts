using DeliveryGuarantees.Repository;
using Newtonsoft.Json;
using RabbitMQ.Client;
using System.Text;
using System.Text.Unicode;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using IModel = RabbitMQ.Client.IModel;

namespace InBoxApp.Producer.Services
{
    public class OutboxProcessor
    {
        AppDbContext db { get; set; }
        public IModel channel { get; set; }
        public string ExchangeName { get; set; }

        public OutboxProcessor(AppDbContext db)
        {
            this.db=db;
            var factory = new ConnectionFactory() { HostName = "localhost" };
            var connection = factory.CreateConnection();
            channel = connection.CreateModel();

            ExchangeName = "DeliveryGuarantees_exchange";
            channel.ExchangeDeclare(exchange: ExchangeName, type: ExchangeType.Direct);
        }

        public void Process()
        {
            var messages = db.OutBoxMessages
                .Where(x => x.ProcessedAt==null)
                .OrderBy(x => x.CreatedAt)
                .ToList();

            foreach (var item in messages)
            {
                var body = Encoding.UTF8.GetBytes(item.Payload);

                var properties = channel.CreateBasicProperties();
                properties.CorrelationId = Guid.NewGuid().ToString();  // Unique correlation ID
                properties.Type =item.Type;  // Queue to reply to

                channel.BasicPublish(ExchangeName, "delivery-guarantees", properties, body);

                Console.WriteLine($"Message Processed: {item.Payload}");
                item.ProcessedAt = DateTime.Now;
                db.SaveChanges();
            }
        }
    }
}
