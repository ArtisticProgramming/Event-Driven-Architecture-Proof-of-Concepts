using DeliveryGuarantees.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading.Tasks;

namespace InBoxApp.Consumer.Service
{
    public class InBoxService
    {
        public AppDbContext db { get; set; }
        public InBoxService(AppDbContext db)
        {
            this.db=db;
        }

        public void SaveToInBox(string type, string payload, string? correlationId = null, string? header = null)
        {
            InBoxMessage message = new InBoxMessage()
            {
                Payload = payload,
                MessageType=type,
                CreatedAt=DateTime.Now,
                CorrelationId=correlationId,
                Header= header,
            };

            db.InBoxMessages.Add(message);
            db.SaveChanges();
        }
    }
}
