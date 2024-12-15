using DeliveryGuarantees.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InBoxApp.Consumer.Service
{
    public class InboxProcessor
    {
        private readonly AppDbContext _dbContext;
        private readonly EventDispatcher _dispatcher;

        public InboxProcessor(AppDbContext dbContext, EventDispatcher dispatcher)
        {
            _dbContext = dbContext;
            _dispatcher = dispatcher;
        }

        public void ProcessInboxMessages()
        {
            var messages = _dbContext.InBoxMessages.Where(m => m.ProcessedAt == null).ToList();

            foreach (var message in messages)
            {
                _dispatcher.Dispatch(message.Payload, message.MessageType);
                message.ProcessedAt = DateTime.UtcNow;
                _dbContext.SaveChanges();
            }
        }
    }

}
