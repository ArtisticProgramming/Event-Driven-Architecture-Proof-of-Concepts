using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RabbitMQExchanges.BuildingBlocks.Model
{
    public interface IUserPasswordCreatedEvent
    {
        string UserName { get; set; }
        string Password { get; set; }
    }
}
