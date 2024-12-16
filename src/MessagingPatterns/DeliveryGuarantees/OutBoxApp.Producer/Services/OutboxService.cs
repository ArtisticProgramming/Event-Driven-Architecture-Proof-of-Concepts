using DeliveryGuarantees.Repository;

namespace InBoxApp.Producer.Services
{
    internal class OutboxService
    {
        AppDbContext db { get; set; }
        public OutboxService(AppDbContext db)
        {
            this.db=db;
        }

        public void AddMessage(string type, string payload)
        {
            Console.WriteLine($"Payload: {payload}");
            var outBoxMessage = new OutBoxMessage()
            {
                Type=type,
                Payload = payload,
            };

            db.OutBoxMessages.Add(outBoxMessage);
            db.SaveChanges();
        }
    }
}
