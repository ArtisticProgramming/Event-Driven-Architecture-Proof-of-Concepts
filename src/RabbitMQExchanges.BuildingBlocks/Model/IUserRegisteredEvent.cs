using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RabbitMQExchanges.BuildingBlocks.Model
{
    public interface IUserRegisteredEvent
    {
        public string FirstName { get;  }
        public string LastName { get;  }
        public int Age{ get; }
    }
}
